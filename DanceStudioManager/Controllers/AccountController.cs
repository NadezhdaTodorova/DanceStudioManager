using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace DanceStudioManager
{
    public class AccountController : Controller
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly StudioDataAccess _studioDataAccess;
        private byte[] salt;

        public AccountController(UserDataAccess userDataAccess, StudioDataAccess studioDataAccess)
        {
            _userDataAccess = userDataAccess;
            _studioDataAccess = studioDataAccess;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                ViewBag.register = true;
                Studio newStudio = new Studio();

                int saltLength = 10;
                byte[] saltBytes = GenerateRandomCryptographicBytes(saltLength);
                salt = saltBytes;
                user.Password = HashWithSalt(user.Password, saltLength, SHA256.Create(), saltBytes);
                user.Salt = salt;

                newStudio.Name = user.StudioName;
                newStudio.Password = user.Password;

                var allUsers = _userDataAccess.GetAllUsers();
                var allStudios = _studioDataAccess.GetAllStudios();

                if (allUsers.Any(x => x.Username == user.Username))
                {
                    ModelState.AddModelError(string.Empty, "User with this username already exists");
                }

                if (allUsers.Any(x => x.Email == user.Email))
                {
                    ModelState.AddModelError(string.Empty, "User with this email already exists");
                }

                if (allStudios.Any(x => x.Name == user.StudioName))
                {
                    ModelState.AddModelError(string.Empty, "This studio name already exists");
                }

                else
                {
                    _studioDataAccess.AddNewStudio(newStudio);

                    int studioId = _studioDataAccess.GetStudioId(newStudio);

                    _userDataAccess.AddNewUser(user, studioId);

                    int userId = _userDataAccess.GetUserId(user);

                    SendEmail("nadezhdatodorova55@gmail.com", user);

                    return View("Views/Home/ConfirmEmail.cshtml");
                }
            }

            return View("Views/Home/RegisterLogin.cshtml");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            ViewBag.login = true;
            var allUsers = _userDataAccess.GetAllUsers();

            if (!allUsers.Any(x => x.Email == user.Email))
            {
                ModelState.AddModelError(string.Empty, "User with this email doesn't exists!");
            }
            else
            {
                var userId = _userDataAccess.GetUserId(user);
                user.Id = userId;
                var userInfo = _userDataAccess.GetUserById(user.Id);
                var userSalt = userInfo.Salt;
                var userPass = userInfo.Password;

                var userEnteredPass = HashWithSalt(user.Password, 10, SHA256.Create(), userSalt);

                if (userPass != userEnteredPass)
                {
                    ModelState.AddModelError("WrongPassword", "Wrong password, please try again!");
                }
                else
                {
                    return View("Views/Studio/Dashboard.cshtml");
                }
            }
            return View("Views/Home/RegisterLogin.cshtml");
        }


        public byte[] GenerateRandomCryptographicBytes(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return randomBytes;
        }

        public string HashWithSalt(string password, int saltLength, HashAlgorithm hashAlgo, byte[] saltBytes)
        {
            byte[] passwordAsBytes = Encoding.UTF8.GetBytes(password);
            List<byte> passwordWithSaltBytes = new List<byte>();
            passwordWithSaltBytes.AddRange(passwordAsBytes);
            passwordWithSaltBytes.AddRange(saltBytes);
            byte[] digestBytes = hashAlgo.ComputeHash(passwordWithSaltBytes.ToArray());
            return new string(Convert.ToBase64String(saltBytes) + Convert.ToBase64String(digestBytes));
        }

        public void SendEmail(string emailTo, User user)
        {
            MailMessage mail = new MailMessage();
            string mailFrom = "nadezhdatodorova55@gmail.com";
            mail.From = new MailAddress("nadezhdatodorova55@gmail.com");

            // The important part -- configuring the SMTP client
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;   // [1] You can try with 465 also, I always used 587 and got success
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // [2] Added this
            smtp.UseDefaultCredentials = false; // [3] Changed this
            smtp.Credentials = new NetworkCredential(mailFrom, "kosara15");  // [4] Added this. Note, first parameter is NOT string.
            smtp.Host = "smtp.gmail.com";

            //recipient address
            mail.To.Add(new MailAddress(emailTo));

            //Formatted mail body
            mail.IsBodyHtml = true;
            string st = $"You just confirmed your account in DanceStudioManager. Please log in to access it!" + Url.Action("Login", "Account", null); ;

            mail.Body = st;
            smtp.Send(mail);
            user.ConfirmAccount = 1;
            _userDataAccess.UpdateUser(user);
        }
    }
}

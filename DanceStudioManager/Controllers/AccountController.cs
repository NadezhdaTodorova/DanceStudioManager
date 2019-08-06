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
        private readonly HashPassword _hashPassword;
        private byte[] salt;

        public AccountController(UserDataAccess userDataAccess, StudioDataAccess studioDataAccess, HashPassword hashPassword)
        {
            _userDataAccess = userDataAccess;
            _studioDataAccess = studioDataAccess;
            _hashPassword = hashPassword;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Register(User user)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                ViewBag.register = true;
                Studio newStudio = new Studio();
                SendEmail email = new SendEmail();

                int saltLength = 10;
                byte[] saltBytes = _hashPassword.GenerateRandomCryptographicBytes(saltLength);
                salt = saltBytes;
                user.Password = _hashPassword.HashWithSalt(user.Password, saltLength, SHA256.Create(), saltBytes);
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

                else if (ModelState.IsValid)
                {
                    _studioDataAccess.AddNewStudio(newStudio);

                    int studioId = _studioDataAccess.GetStudioId(newStudio);

                    _userDataAccess.AddNewUser(user, studioId);

                    int userId = _userDataAccess.GetUserId(user);

                    var path = Url.Action("AuthenticateLogin", "Home", new { userId = user.Id }, protocol: HttpContext.Request.Scheme);
                    string message = "Please confirm your account by clicking <a href=\"" + path + "\">here</a>";

                    email.Send(user.Email, user, message);

                    return View("Views/Home/ConfirmEmail.cshtml");
                }
            }

            return View("Views/Home/RegisterLogin.cshtml");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            ModelState.Clear();
            ViewBag.login = true;
            var allUsers = _userDataAccess.GetAllUsers();
            var userId = _userDataAccess.GetUserId(user);
            user.Id = userId;
            var userInfo = _userDataAccess.GetUserById(user.Id);

            if (!allUsers.Any(x => x.Email == user.Email))
            {
                ModelState.AddModelError(string.Empty, "User with this email doesn't exists!");
            }
            else if (!userInfo.ConfirmAccount)
            {
                ModelState.AddModelError(string.Empty, "We have send an email to your email account, please confirm it!");
            }
            else if (ModelState.IsValid)
            {
                var userEnteredPass = _hashPassword.HashWithSalt(user.Password, 10, SHA256.Create(), userInfo.Salt);

                if (userInfo.Password != userEnteredPass)
                {
                    ModelState.AddModelError("WrongPassword", "Wrong password, please try again!");
                }
                else
                {
                    try
                    {
                        _userDataAccess.SignIn(HttpContext, user.Id);
                        return RedirectToAction("Dashboard", "Studio");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("summary", ex.Message);
                        return View("Views/Home/RegisterLogin.cshtml");
                    }
                }
            }
            return View("Views/Home/RegisterLogin.cshtml");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DanceStudioManager.Models;
using System.Transactions;
using System.Threading;

namespace DanceStudioManager
{
    public class HomeController : Controller
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly SendEmail _email;

        public HomeController(UserDataAccess userDataAccess, SendEmail email)
        {
            _userDataAccess = userDataAccess;
            _email = email;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ContactForm(string firstName, string lastName, string Email, string Subject, string Text)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _email.SendContacts(Email, firstName, lastName, Subject, Text);

                    scope.Complete();
                }
            }
            catch (ThreadAbortException ex)
            {
                ModelState.AddModelError(ex.Message, "");
            }
            return View("Views/Home/SendEmailForContacts.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult RegisterLogin()
        {
            ViewBag.register = true;
            return View("Views/Account/RegisterLogin.cshtml");
        }

        
        public IActionResult AuthenticateLogin(int userId)
        {
            ModelState.Clear();
            ViewBag.login = true;
            if (userId != 0)
            {
                User user = _userDataAccess.GetUserById(userId);
                user.ConfirmAccount = true;
                _userDataAccess.UpdateUser(user);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please enter your email and password!");
            }
            return View("Views/Account/RegisterLogin.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DanceStudioManager.Models;

namespace DanceStudioManager
{
    public class HomeController : Controller
    {
        private readonly UserDataAccess _userDataAccess;
        public HomeController(UserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
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

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult RegisterLogin()
        {
            ViewBag.register = true;
            return View();
        }

        
        public IActionResult Login(int userId)
        {
            ViewBag.login = true;
            User user = _userDataAccess.GetUserById(userId);
            user.ConfirmAccount = true;
            _userDataAccess.UpdateUser(user);
            return View("Views/Home/RegisterLogin.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

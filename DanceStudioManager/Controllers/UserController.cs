using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager.Controllers
{
    public class UserController : Controller
    {
        private readonly UserDataAccess _userDataAccess;

        public UserController(UserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        public IActionResult Index()
        {
            var a = _userDataAccess.GetAllUsers();
            return View();
        }
    }
}
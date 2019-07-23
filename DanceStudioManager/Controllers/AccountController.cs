using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class AccountController : Controller
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly StudioDataAccess _studioDataAccess;

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
                Studio newStudio = new Studio();

                newStudio.Name = user.StudioName;
                newStudio.Password = user.Password;

                var allUsers = _userDataAccess.GetAllUsers();
                var allStudios = _studioDataAccess.GetAllStudios();

                if (allUsers.Any(x => x.Username == user.Username))
                {
                    ModelState.AddModelError(string.Empty, "User with this username already exists");
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

                    return View("Views/Home/ConfirmEmail.cshtml");
                }
            }

            return View("Views/Home/RegisterLogin.cshtml");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(User user)
        {
            return View();
        }
    }
}

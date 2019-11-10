using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserDataAccess _userDataAccess;
        private readonly StudioDataAccess _studioDataAccess;
        private readonly StudentsDataAccess _studentDataAccess;
        private readonly ClassDataAccess _classDataAccess;
        private readonly InstructorDataAccess _instructorDataAccess;

        public UserController(UserDataAccess userDataAccess, StudioDataAccess studioDataAccess, StudentsDataAccess studentDataAccess, ClassDataAccess classDataAccess,
            InstructorDataAccess instructorDataAccess)
        {
            _userDataAccess = userDataAccess;
            _studioDataAccess = studioDataAccess;
            _studentDataAccess = studentDataAccess;
            _classDataAccess = classDataAccess;
            _instructorDataAccess = instructorDataAccess;
        }

        public IActionResult Index()
        {
            ViewBag.text = "User profile";
            var user = GetCurrentUser();
            var studio = _studioDataAccess.GetStudioInfo(user.StudioId);

            user.StudioName = studio.Name;
            user.PhotoUrl = studio.Photo_url;
            user.NumberOfStudents = _studentDataAccess.GetAllStudents(studio.Id).Count();
            user.NumberOfClasses = _classDataAccess.GetAllClasses(studio.Id).Count();
            user.NumberOfInstructors = _instructorDataAccess.GetAllInstructors(studio.Id).Count();

            return View("Views/Studio/User.cshtml",user);
        }

        public IActionResult Edit(User user)
        {
            var userId = _userDataAccess.GetUserId(user);
            var studio = new Studio();

            user.Id = userId;
            user.ConfirmAccount = true;
            studio.Name = user.StudioName;

            _userDataAccess.UpdateUser(user);
            _studioDataAccess.UpdateStudio(studio, userId);

            return RedirectToAction("Index");
        }

        public IActionResult Delete()
        {
            return View();
        }

        private User GetCurrentUser()
        {
            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);
            var user = _userDataAccess.GetUserById(userId);

            return user;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Web;

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
        private readonly IHostingEnvironment _appEnvironment;

        public UserController(UserDataAccess userDataAccess, StudioDataAccess studioDataAccess, StudentsDataAccess studentDataAccess, ClassDataAccess classDataAccess,
            InstructorDataAccess instructorDataAccess, IHostingEnvironment appEnvironment)
        {
            _userDataAccess = userDataAccess;
            _studioDataAccess = studioDataAccess;
            _studentDataAccess = studentDataAccess;
            _classDataAccess = classDataAccess;
            _instructorDataAccess = instructorDataAccess;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.text = "User profile";
            var user = GetCurrentUser();
            ViewBag.StudioName = _studioDataAccess.GetStudioInfo(user.StudioId).Name;
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
            var userId = GetCurrentUser().Id;
            var studio = new Studio();

            user.Id = userId;
            user.ConfirmAccount = true;
            studio.Name = user.StudioName;

            _userDataAccess.UpdateUser(user);

            var studioId = _userDataAccess.GetUserById(userId).StudioId;

            _studioDataAccess.UpdateStudio(studio, studioId);

            return RedirectToAction("Index");
        }

        public IActionResult Delete()
        {
            int userId = GetCurrentUser().Id;
            _userDataAccess.DeleteUser(userId);
            return RedirectToAction("LogOut", "Account");
        }

        [HttpPost]
        public IActionResult ChangePhoto(IFormFile PhotoUrl)
        {
            if (PhotoUrl != null)
            {

                // File to be deleted    
                string authorsFile = PhotoUrl.FileName;
                string rootFolder = Path.Combine(_appEnvironment.WebRootPath, "images", Path.GetFileName(PhotoUrl.FileName));

                try
                {
                    // Check if file exists with its full path    
                    if (System.IO.File.Exists(rootFolder))
                    {
                        // If file found, delete it    
                        System.IO.File.Delete(rootFolder);
                    }
                }
                catch (IOException ioExp)
                {
                    ModelState.AddModelError(ioExp.Message, "");
                    return RedirectToAction("Index");
                }

                var studio = new Studio();
                if (PhotoUrl != null)
                {
                    PhotoUrl.CopyTo(new FileStream(rootFolder, FileMode.Create));

                    var lastPart = rootFolder.Substring(rootFolder.IndexOf("images") - 1);
                    studio.Photo_url = lastPart;
                    _studioDataAccess.UpdateStudio(studio, GetCurrentUser().StudioId);
                }
            }
            return RedirectToAction("Index");

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
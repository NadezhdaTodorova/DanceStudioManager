using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePhoto(User emp)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        //There is an error here
                        var uploads = Path.Combine(_appEnvironment.WebRootPath, "uploads\\img");
                        if (file.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
                            using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                                emp.PhotoUrl = fileName;
                            }

                        }
                    }
                }
                
                 _userDataAccess.UpdateUser(emp);
                return RedirectToAction("Index");
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
            }
            return View(emp);

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
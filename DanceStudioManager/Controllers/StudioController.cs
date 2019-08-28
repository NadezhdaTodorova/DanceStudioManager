using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DanceStudioManager
{
    [Authorize]
    public class StudioController : Controller
    {
        private readonly StudentsDataAccess _studentDataAccess;
        private readonly StudioDataAccess _studioDataAccess;
        private readonly InstructorDataAccess _instructorDataAccess;
        private readonly ClassDataAccess _classDataAccess;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserDataAccess _userDataAccess;

        public StudioController(StudentsDataAccess studentDataAccess, StudioDataAccess studioDataAccess, InstructorDataAccess instructorDataAccess,
            ClassDataAccess classDataAccess, IHttpContextAccessor httpContextAccessor, UserDataAccess userDataAccess)
        {
            _studentDataAccess = studentDataAccess;
            _studioDataAccess = studioDataAccess;
            _instructorDataAccess = instructorDataAccess;
            _classDataAccess = classDataAccess;
            _httpContextAccessor = httpContextAccessor;
            _userDataAccess = userDataAccess;
        }
        public IActionResult Dashboard()
        {
            ViewBag.text = "Dashboard";
            return View();
        }
        public IActionResult Students()
        {
            ViewBag.text = "Students";
            return View();
        }

        public IActionResult GetStudents(Student student)
        {
            List<Student> studentList = new List<Student>();
            if (student.Firstname == null || student.Lastname == null || student.Email == null)
            {
                studentList = _studentDataAccess.GetAllStudents();
            }
            else
            {
                studentList = _studentDataAccess.SearchStudents(student);
            }

            return Json(studentList);
        }

        public IActionResult AddNewStudent(Student student)
        {
            int studioId = GetCurrentStudioId();

            _studentDataAccess.AddNewStudent(student, studioId);
            return RedirectToAction("Students");
        }
        public IActionResult Instructor()
        {
            ViewBag.text = "Instructors";
            return View();
        }

        public IActionResult GetInstructors(Instructor instructor)
        {
            List<Instructor> instructorList = new List<Instructor>();
            if (instructor.Firstname == null || instructor.Lastname == null || instructor.Email == null)
            {
                instructorList = _instructorDataAccess.GetAllInstructors();
            }
            else
            {
                instructorList = _instructorDataAccess.SearchInstructors(instructor);
            }

            return Json(instructorList);
        }

        public IActionResult AddNewInstructor(Instructor instructor)
        {

            int studioId = GetCurrentStudioId();
            _instructorDataAccess.AddNewInstructor(instructor, studioId);
            return RedirectToAction("Instructor");
        }

        public IActionResult Classes()
        {
            ViewBag.text = "Classes";
            var _class = new ClassStudentVM();
            var studentsList = _studentDataAccess.GetAllStudents();
            var instructorList = _instructorDataAccess.GetAllInstructors();
            
            foreach(var s in studentsList)
            {
                var sL = new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Firstname} {s.Lastname}"
                };


                _class.Students.Add(sL);
            }

            foreach(var i in instructorList)
            {
                var iL = new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.Firstname} {i.Lastname}"
                };

                _class.Instructors.Add(iL);
            }

            return View(_class);
        }

        public IActionResult GetClasses()
        {
            List<Class> classesList = new List<Class>();

            classesList = _classDataAccess.GetAllClasses();

            return Json(classesList);
        }

        [HttpPost]
        public IActionResult AddNewClass(ClassStudentVM _class)
        {
            var newclass = new Class();
            newclass.Genre = _class.Genre;
            newclass.Level = _class.Level;
            newclass.PricePerHour = _class.PricePerHour;

            int studioId = GetCurrentStudioId();

            _classDataAccess.AddNewClass(newclass, studioId);
            //_classDataAccess.AddNewShedule(,studioId,newclass.Shedule)
            return RedirectToAction("Classes");
        }


        public IActionResult Events()
        {
            ViewBag.text = "Events";
            return View();
        }

        private int GetCurrentStudioId()
        {
            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);
            var studioId = _userDataAccess.GetUserById(userId).StudioId;

            return studioId;
        }
    }
}
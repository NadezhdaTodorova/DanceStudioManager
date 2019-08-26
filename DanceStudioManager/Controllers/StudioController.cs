using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Dashboard(int studioId)
        {
            ViewBag.text = "Dashboard";
            ViewBag.StudioName = _studioDataAccess.GetStudioInfo(studioId).Name;
            return View();
        }
        public IActionResult Students()
        {
            ViewBag.text = "Students";
            var userId = _userDataAccess.GetUserId(User);
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
            _studentDataAccess.AddNewStudent(student, 62);
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
            _instructorDataAccess.AddNewInstructor(instructor, 62);
            return RedirectToAction("Instructor");
        }

        public IActionResult Classes()
        {
            ViewBag.text = "Classes";
            return View();
        }

        public IActionResult GetClasses()
        {
            List<Class> classesList= new List<Class>();

            classesList = _classDataAccess.GetAllClasses();

            return Json(classesList);
        }

        public IActionResult AddNewClass(Class _class)
        {
            //_instructorDataAccess.AddNewInstructor(instructor, 62);
            return RedirectToAction("Classes");
        }

        public IActionResult Events()
        {
            ViewBag.text = "Events";
            return View();
        }
    }
}
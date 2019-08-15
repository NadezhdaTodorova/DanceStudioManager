using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DanceStudioManager
{
    [Authorize]
    public class StudioController : Controller
    {
        private readonly StudentsDataAccess _studentDataAccess;
        private readonly StudioDataAccess _studioDataAccess;
        private readonly InstructorDataAccess _instructorDataAccess;
        private readonly ClassDataAccess _classDataAccess;

        public StudioController(StudentsDataAccess studentDataAccess, StudioDataAccess studioDataAccess, InstructorDataAccess instructorDataAccess,
            ClassDataAccess classDataAccess)
        {
            _studentDataAccess = studentDataAccess;
            _studioDataAccess = studioDataAccess;
            _instructorDataAccess = instructorDataAccess;
            _classDataAccess = classDataAccess;
        }
        public IActionResult Dashboard(int studioId)
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
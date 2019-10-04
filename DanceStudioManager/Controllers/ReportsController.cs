using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly ClassDataAccess _classDataAccess;
        private readonly StudentsDataAccess _studentDataAccess;

        public ReportsController (ClassDataAccess classDataAccess, StudentsDataAccess studentDataAccess)
        {
            _classDataAccess = classDataAccess;
            _studentDataAccess = studentDataAccess;
        }
        public IActionResult ClassStudent()
        {
            ViewBag.text = "Class-Student report";
            return View("Views/Studio/ClassStudentReport.cshtml");
        }

        public IActionResult SearchStudent(string genre, string level, string type)
        {
            var students = new List<ClassStudentVM>();

            var classes = _classDataAccess.SearchClass(genre, level, type);

            foreach(var _class in classes)
            {
                foreach(var id in _classDataAccess.GetStudentsConnectedToClass(_class.Id))
                {
                    var s = _studentDataAccess.GetStudentById(id);
                    ClassStudentVM student = new ClassStudentVM();

                    student.Firstname = s.Firstname;
                    student.Lastname = s.Lastname;
                    student.Email = s.Email;
                    student.Genre = _class.Genre;
                    student.Level = _class.Level;
                    students.Add(student);
                }
            }
            return Json(students);
        }

        public IActionResult Profit()
        {
            ViewBag.text = "Profit report";
            return View("Views/Studio/ProfitReport.cshtml");
        }
    }
}
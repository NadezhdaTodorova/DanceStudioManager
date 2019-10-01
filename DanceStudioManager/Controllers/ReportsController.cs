using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager
{
    public class ReportsController : Controller
    {
        public IActionResult ClassStudent()
        {
            ViewBag.text = "Class-Student report";
            return View("Views/Studio/ClassStudentReport.cshtml");
        }

        public IActionResult SearchStudent(string genre, string level, string type)
        {
            ClassStudentVM student = new ClassStudentVM();
            student.Firstname = "nadi";
            student.Lastname= "nadi";
            student.Email = "nadi";
            student.Gender = "nadi";
            student.CellPhone = "08985728147";
            student.Genre = genre;
            return Json(student);
        }

        public IActionResult Profit()
        {
            ViewBag.text = "Profit report";
            return View("Views/Studio/ProfitReport.cshtml");
        }
    }
}
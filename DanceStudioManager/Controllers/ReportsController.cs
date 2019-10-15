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
        private readonly AttendanceDataAccess _attendanceDataAccess;
        private readonly InstructorDataAccess _instructorDataAccess;

        public ReportsController (ClassDataAccess classDataAccess, StudentsDataAccess studentDataAccess, AttendanceDataAccess attendanceDataAccess,
            InstructorDataAccess instructorDataAccess)
        {
            _classDataAccess = classDataAccess;
            _studentDataAccess = studentDataAccess;
            _attendanceDataAccess = attendanceDataAccess;
            _instructorDataAccess = instructorDataAccess;
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

        public IActionResult SearchProfitForPeriod(DateTime dateFrom, DateTime dateTo, string classGenre, string level)
        {
            Profit finalProfit = new Profit();
            string group = "group";

            finalProfit.ProfitForPeriod = 0;
            finalProfit.NumberOfStudents = 0;

            var _class = _classDataAccess.SearchClass(classGenre, level, group);
            var attendances = _attendanceDataAccess.SearchAttendancesByClassId(_class.First().Id);

            finalProfit.Level = _class.First().Level;
            finalProfit.ClassGenre = _class.First().Genre;
            finalProfit.Type = _class.First().ClassType;


            if (dateFrom != default(DateTime) && dateTo != default(DateTime))
            {
                for (DateTime date = dateFrom; date >= dateTo; date = date.AddDays(-1))
                {
                    foreach (var at in attendances)
                    {
                        double profit = 0;
                        var numberOfStudents = 0;

                        if (at.Date == date)
                        {
                            double instructorPay = 0;
                            double procent = 0;

                            numberOfStudents += _classDataAccess.GetStudentsConnectedToClass(at.ClassId).Count;
                            foreach (var i in _classDataAccess.GetInstructorsConnectedToClass(at.ClassId))
                            {
                                var instructor = _instructorDataAccess.GetInstructorById(i);
                                procent += instructor.procentOfProfit;
                                finalProfit.instructors.Add(instructor.Firstname);
                            }

                            profit = numberOfStudents * (_classDataAccess.SearchClass(at.ClassId).PricePerHour);
                            instructorPay = profit * (procent / 100);
                            profit = profit - instructorPay;
                        }
                        finalProfit.NumberOfStudents += numberOfStudents;
                        finalProfit.ProfitForPeriod += Math.Round(profit);
                    }
                }
            }
            return Json(finalProfit);
        }
    }
}
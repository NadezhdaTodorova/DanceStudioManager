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
    public class ReportsController : Controller
    {
        private readonly ClassDataAccess _classDataAccess;
        private readonly StudentsDataAccess _studentDataAccess;
        private readonly AttendanceDataAccess _attendanceDataAccess;
        private readonly InstructorDataAccess _instructorDataAccess;
        private readonly UserDataAccess _userDataAccess;
        private readonly StudioDataAccess _studioDataAccess;

        public ReportsController(ClassDataAccess classDataAccess, StudentsDataAccess studentDataAccess, AttendanceDataAccess attendanceDataAccess,
            InstructorDataAccess instructorDataAccess, UserDataAccess userDataAccess, StudioDataAccess studioDataAccess)
        {
            _classDataAccess = classDataAccess;
            _studentDataAccess = studentDataAccess;
            _attendanceDataAccess = attendanceDataAccess;
            _instructorDataAccess = instructorDataAccess;
            _userDataAccess = userDataAccess;
            _studioDataAccess = studioDataAccess;
        }
        public IActionResult ClassStudent()
        {
            ViewBag.text = "Class-Student report";
            ViewBag.StudioName = _studioDataAccess.GetStudioInfo(GetCurrentStudioId()).Name;
            return View("Views/Studio/ClassStudentReport.cshtml");
        }

        public IActionResult SearchStudent(string genre, string level, string type)
        {
            var students = new List<ClassStudentVM>();
            var classes = new List<Class>();

            if (genre != null && level != null && type != null)
            {
                classes = _classDataAccess.SearchClass(genre, level, type, GetCurrentStudioId());
            }
            else
            {
                classes = _classDataAccess.GetAllClasses(GetCurrentStudioId());
            } 

            foreach (var _class in classes)
            {
                foreach (var id in _classDataAccess.GetStudentsConnectedToClass(_class.Id, GetCurrentStudioId()))
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

        public IActionResult Profit(string classError)
        {
            ViewBag.text = "Profit report";
            ViewBag.StudioName = _studioDataAccess.GetStudioInfo(GetCurrentStudioId()).Name;
            if (classError != null) ViewBag.classError = classError;
            return View("Views/Studio/ProfitReport.cshtml");
        }

        public IActionResult SearchProfitForPeriod(DateTime dateFrom, DateTime dateTo, string classGenre, string level, string type)
        {
            List<Profit> finalProfit = new List<Profit>();
            var _class = new List<Class>();
            DateTime defaultDatetime = default(DateTime);
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (dateFrom != defaultDatetime && dateTo != defaultDatetime || classGenre != null || level != null)
            {
                _class = _classDataAccess.SearchClass(classGenre, level, type, GetCurrentStudioId());
            }
            else
            {
                _class = _classDataAccess.GetAllClasses(GetCurrentStudioId());
                dateFrom = firstDayOfMonth;
                dateTo = DateTime.Now.Date;
            }
            if (_class.Count > 0)
            {
                foreach (var c in _class)
                {
                    Profit profit = new Profit();
                    var attendances = _attendanceDataAccess.SearchAttendancesByClassId(c.Id);

                    profit.ProfitForPeriod = 0;
                    profit.NumberOfStudents = 0;
                    profit.Attendances = 0;
                    profit.Level = c.Level;
                    profit.ClassGenre = c.Genre;
                    profit.Type = c.ClassType;
                    profit.DateFrom = dateFrom.ToString("MM-dd-yyyy");
                    profit.DateTo = dateTo.ToString("MM-dd-yyyy");

                    for (DateTime date = dateFrom; date <= dateTo; date = date.AddDays(1))
                    {
                        foreach (var at in attendances)
                        {
                            double sum = 0;
                            var numberOfStudents = 0;

                            if (at.Date == date)
                            {
                                double instructorPay = 0;
                                double procent = 0;

                                numberOfStudents += _classDataAccess.GetStudentsConnectedToClass(at.ClassId, GetCurrentStudioId()).Count;
                                foreach (var i in _classDataAccess.GetInstructorsConnectedToClass(at.ClassId, GetCurrentStudioId()))
                                {
                                    var instructor = _instructorDataAccess.GetInstructorById(i);
                                    procent += instructor.procentOfProfit;
                                }

                                sum = numberOfStudents * (_classDataAccess.SearchClass(at.ClassId, GetCurrentStudioId()).PricePerHour);
                                instructorPay = sum * (procent / 100);
                                sum = sum - instructorPay;
                                profit.Attendances++;
                            }
                            profit.ProfitForPeriod += Math.Round(sum);
                        }
                    }
                    profit.NumberOfStudents = _classDataAccess.GetStudentsConnectedToClass(c.Id, GetCurrentStudioId()).Count;
                    finalProfit.Add(profit);
                }
            }
            else
            {
                string classError = "A class with this genre, level or type does not exists!";
                return RedirectToAction("Profit", new { classError });
            }

            return Json(finalProfit);
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
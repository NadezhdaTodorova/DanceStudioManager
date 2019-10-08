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
        private readonly AttendanceDataAccess _attendanceDataAccess;

        public StudioController(StudentsDataAccess studentDataAccess, StudioDataAccess studioDataAccess, InstructorDataAccess instructorDataAccess,
            ClassDataAccess classDataAccess, IHttpContextAccessor httpContextAccessor, UserDataAccess userDataAccess, AttendanceDataAccess attendanceDataAccess)
        {
            _studentDataAccess = studentDataAccess;
            _studioDataAccess = studioDataAccess;
            _instructorDataAccess = instructorDataAccess;
            _classDataAccess = classDataAccess;
            _httpContextAccessor = httpContextAccessor;
            _userDataAccess = userDataAccess;
            _attendanceDataAccess = attendanceDataAccess;
        }
        public IActionResult Dashboard()
        {
            ViewBag.text = "Dashboard";
            DashboardNeeds dashboardNeeds = new DashboardNeeds();
            var allClasses = _classDataAccess.GetAllClasses();
            var allStudents = _studentDataAccess.GetAllStudents();
            var allInstructors = _instructorDataAccess.GetAllInstructors();

            foreach (var _class in allClasses)
            {
                var classShedule = _classDataAccess.GetClassShedule(_class.Id);
                foreach (var s in classShedule)
                {
                    if (s.Day == DateTime.Now.DayOfWeek.ToString())
                    {
                        _class.Hour = s.Hour;
                        dashboardNeeds.classesForToday.Add(_class);
                    }
                }
            }

            foreach (var st in allStudents)
            {
                var sL = new SelectListItem()
                {
                    Value = st.Id.ToString(),
                    Text = $"{st.Firstname} {st.Lastname}"
                };

                dashboardNeeds.AllStudents.Add(sL);
            }

            foreach (var ins in allInstructors)
            {
                var iL = new SelectListItem()
                {
                    Value = ins.Id.ToString(),
                    Text = $"{ins.Firstname} {ins.Lastname}"
                };

                dashboardNeeds.AllInstructors.Add(iL);
            }
            return View(dashboardNeeds);
        }

        [HttpPost]
        public IActionResult Dashboard(int classId)
        {
            DashboardNeeds dashboardNeeds = new DashboardNeeds();
            var instructorList = _classDataAccess.GetInstructorsConnectedToClass(classId);
            var studentsIdList = _classDataAccess.GetStudentsConnectedToClass(classId);
            var _class = _classDataAccess.SearchClass(classId);
           
            foreach (var idsIns in instructorList)
            {
                var instructor = _instructorDataAccess.GetInstructorById(idsIns);
                dashboardNeeds.Instructors.Add(instructor);
            }

            foreach (var idsStu in studentsIdList)
            {
                var student = _studentDataAccess.GetStudentById(idsStu);
                dashboardNeeds.Students.Add(student);
            }

            dashboardNeeds.PricePerHour = _class.PricePerHour;
            dashboardNeeds.Level = _class.Level;

            
            return Json(dashboardNeeds);
        }
        public IActionResult Students()
        {
            ViewBag.text = "Students";
            return View();
        }

        public IActionResult GetStudents(string firstname, string lastname, string email)
        {
            List<Student> studentList = new List<Student>();
            Student student = new Student();

            if (firstname == null && lastname == null && email == null)
            {
                studentList = _studentDataAccess.GetAllStudents();
            }
            else
            {
                student.Firstname = firstname;
                student.Lastname = lastname;
                student.Email = email;
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

        //public IActionResult SearchStudent([FromBody]Student student)
        //{
        //    List<Student> studentList = new List<Student>();
        //    if (student.Firstname == null || student.Lastname == null || student.Email == null)
        //    {
        //        studentList = _studentDataAccess.GetAllStudents();
        //    }
        //    else
        //    {
        //        studentList = _studentDataAccess.SearchStudents(student);
        //    }

        //    return Json(studentList);
        //}

        public IActionResult Instructor()
        {
            ViewBag.text = "Instructors";
            return View();
        }

        public IActionResult GetInstructor(string firstname, string lastname, string email)
        {
            List<Instructor> instructorList = new List<Instructor>();
            Instructor instructor = new Instructor();

            if (firstname == null && lastname == null && email == null)
            {
                instructorList = _instructorDataAccess.GetAllInstructors();
            }
            else
            {
                instructor.Firstname = firstname;
                instructor.Lastname = lastname;
                instructor.Email = email;
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

        public IActionResult Classes(string classError)
        {
            ViewBag.text = "Classes";
            var _class = new ClassStudentVM();
            var studentsList = _studentDataAccess.GetAllStudents();
            var instructorList = _instructorDataAccess.GetAllInstructors();

            foreach (var s in studentsList)
            {
                var sL = new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Firstname} {s.Lastname}"
                };


                _class.Students.Add(sL);
            }

            foreach (var i in instructorList)
            {
                var iL = new SelectListItem()
                {
                    Value = i.Id.ToString(),
                    Text = $"{i.Firstname} {i.Lastname}"
                };

                _class.Instructors.Add(iL);
            }

            if (classError != null) ViewBag.classError = classError;

            return View(_class);
        }

        public IActionResult GetClasses(string genre, string level, string type)
        {
            List<Class> classesList = new List<Class>();

            if (genre == null && level == null && type == null)
            {
                classesList = _classDataAccess.GetAllClasses();
                foreach (var c in classesList)
                {
                    var instructorsIds = _classDataAccess.GetInstructorsConnectedToClass(c.Id);
                    foreach (var id in instructorsIds)
                    {
                        var instructor = _instructorDataAccess.GetInstructorById(id);
                        c.Instructors.Add($" {instructor.Firstname} ");
                    }

                    var shedule = _classDataAccess.GetClassShedule(c.Id);
                    foreach (var s in shedule)
                    {
                        c.Shedule.Add($" {s.Day} - {s.Hour} ");
                    }
                }
            }else
            {
               List<Class> _classes = new List<Class> ();

                _classes = _classDataAccess.SearchClass(genre, level, type);
                foreach (var _class in _classes)
                {
                    var instructorsIds = _classDataAccess.GetInstructorsConnectedToClass(_class.Id);
                    foreach (var id in instructorsIds)
                    {
                        var instructor = _instructorDataAccess.GetInstructorById(id);
                        _class.Instructors.Add($" {instructor.Firstname} ");
                    }

                    var shedule = _classDataAccess.GetClassShedule(_class.Id);
                    foreach (var s in shedule)
                    {
                        _class.Shedule.Add($" {s.Day} - {s.Hour} ");
                    }

                    classesList.Add(_class);
                }
            }

            

            return Json(classesList);
        }

        [HttpPost]
        public IActionResult AddNewClass(ClassStudentVM _class)
        {
            var newclass = new Class();
            newclass.Genre = _class.Genre;
            newclass.Level = _class.Level;
            newclass.PricePerHour = _class.PricePerHour;
            newclass.ClassType = _class.ClassType;

            int studioId = GetCurrentStudioId();

            var classes = _classDataAccess.GetAllClasses();

            foreach (var c in classes)
            {
                if ((c.Genre == newclass.Genre) && (c.Level == newclass.Level))
                {
                    string classError = "There is already a class with the same genre and level!";
                    return RedirectToAction("Classes", new { classError });
                }
            }

            _classDataAccess.AddNewClass(newclass, studioId);
            int classId = _classDataAccess.GetClassId(newclass);

            foreach (var day in _class.SheduleDays)
            {
                _classDataAccess.AddDayToShedule(day, classId, _class.Hour, studioId);
            }

            //ToDo
            //getSheduleId
            //updateClass with sheduleId

            foreach (var studentId in _class.StudentsIds)
            {
                _classDataAccess.AddStudentToClass(studentId, classId);
            }

            foreach (var instructorId in _class.InstructorsIds)
            {
                _classDataAccess.AddInstructorToClass(instructorId, classId);
            }
            return RedirectToAction("Classes");
        }
        [HttpPost]
        public IActionResult StartClass(int id)
        {
            var studioId = GetCurrentStudioId();
            var todayDate = DateTime.Now.Date;
            var instructorList = _classDataAccess.GetInstructorsConnectedToClass(id);
            var studentsIdList = _classDataAccess.GetStudentsConnectedToClass(id);

            //ToDo
            //before adding attendance I have to check if this class is not added already!!!

            _attendanceDataAccess.AddAttendance(id, studioId, todayDate);
            var attendanceId = _attendanceDataAccess.GetAttendanceId(id);

            foreach (var idIns in instructorList)
            {
                _attendanceDataAccess.AddInstructorAttendance(idIns, attendanceId);
            }

            foreach (var idStu in studentsIdList)
            {
                _attendanceDataAccess.AddStudentAttendance(idStu, attendanceId, id);
            }

            return Json(id);
        }

        public IActionResult DashboardChart()
        {
            int[] data = new int[13];
            int[] months = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            List<Attendance> attendances = _attendanceDataAccess.GetAllAttendances();
            foreach (var month in months)
            {
                int studentsCount = 0;
                foreach (var a in attendances)
                {
                    if (month == a.Date.Month)
                    {
                        foreach (var s in _classDataAccess.GetStudentsConnectedToClass(a.ClassId))
                        {
                            studentsCount++;
                        }
                    }
                }
                data[month-1] = studentsCount;
            }

            return Json(data);
        }

        public IActionResult ProfitDashboardChart ()
        {
            double[] dataProfit = new double[7];
            string[] dataDaysOfWeek = new string[7];
            string[] dates = new string[7];

            int[] days = new int[7] { 1, 2, 3, 4, 5, 6, 7 };
            DateTime currentDay = DateTime.Now.Date.AddDays(-1);
            DateTime before7Days = currentDay.AddDays(-6);
            var allAtendances = _attendanceDataAccess.GetAllAttendances();
            var numberOfStudents = 0;


            for (int d = 0; d < 7; d++)
            {
                double profit = 0;


                if (currentDay >= before7Days)
                {
                    foreach (var at in allAtendances)
                    {
                        if (at.Date == currentDay)
                        {
                            double instructorPay = 0;
                            var procent = 0;

                            numberOfStudents += _classDataAccess.GetStudentsConnectedToClass(at.ClassId).Count;
                            foreach (var i in _classDataAccess.GetInstructorsConnectedToClass(at.ClassId))
                            {
                                var instructor = _instructorDataAccess.GetInstructorById(i);
                                procent += instructor.procentOfProfit = 10;
                            }

                            profit = numberOfStudents * (_classDataAccess.SearchClass(at.ClassId).PricePerHour);
                            instructorPay = (procent / profit) * 100;
                            profit = profit - instructorPay;
                        }
                    }
                    dataDaysOfWeek[d] = currentDay.DayOfWeek.ToString();
                    dataProfit[d] = Math.Round(profit);
                    dates[d] = currentDay.Date.ToString("dd/MM/yyyy");
                }

                currentDay = currentDay.AddDays(-1);
            }

            return Json(new { dataDays = dataDaysOfWeek, dataProft = dataProfit, dats = dates });
        }

        [HttpPost]
        public IActionResult RemoveClass(DashboardNeeds dashboardNeeds)
        {
            _classDataAccess.RemoveClass(dashboardNeeds.ClassId);
            return RedirectToAction("Dashboard");
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            ViewBag.StudioName = _studioDataAccess.GetStudioInfo(GetCurrentStudioId()).Name;
            DashboardNeeds dashboardNeeds = new DashboardNeeds();
            var allClasses = _classDataAccess.GetAllClasses(GetCurrentStudioId());
            var allStudents = _studentDataAccess.GetAllStudents(GetCurrentStudioId());
            var allInstructors = _instructorDataAccess.GetAllInstructors(GetCurrentStudioId());

            foreach (var _class in allClasses)
            {
                var classShedule = _classDataAccess.GetClassShedule(_class.Id);
                foreach (var s in classShedule)
                {
                    if (s.Day == DateTime.Now.DayOfWeek.ToString() && _class.StartDay <= DateTime.Now)
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
            ClassStudentVM dashboardNeeds = new ClassStudentVM();
            var instructorList = _classDataAccess.GetInstructorsConnectedToClass(classId, GetCurrentStudioId());
            var studentsIdList = _classDataAccess.GetStudentsConnectedToClass(classId, GetCurrentStudioId());
            var _class = _classDataAccess.SearchClass(classId, GetCurrentStudioId());
           
            foreach (var idsIns in instructorList)
            {
                var instructor = _instructorDataAccess.GetInstructorById(idsIns);
                dashboardNeeds.InstructorsList.Add(instructor);
            }

            foreach (var idsStu in studentsIdList)
            {
                var student = _studentDataAccess.GetStudentById(idsStu);
                dashboardNeeds.StudentsList.Add(student);
            }

            dashboardNeeds.PricePerHour = _class.PricePerHour;
            dashboardNeeds.Level = _class.Level;
            dashboardNeeds.ClassId = classId;
            var shedule  = _classDataAccess.GetClassShedule(classId);

            foreach(var s in shedule)
            {
                dashboardNeeds.SheduleDays.Add($" {s.Day} - {s.Hour} ");
            }

            return Json(dashboardNeeds);
        }

        public IActionResult Students()
        {
            ViewBag.text = "Students";
            ViewBag.StudioName = _studioDataAccess.GetStudioInfo(GetCurrentStudioId()).Name;
            return View();
        }

        public IActionResult GetStudents(string firstname, string lastname, string email)
        {
            List<Student> studentList = new List<Student>();
            Student student = new Student();

            if ((string.IsNullOrEmpty(firstname) || firstname == "null") 
                && (string.IsNullOrEmpty(lastname) || lastname == "null") 
                && (string.IsNullOrEmpty(email) || email == "null"))
            {
                studentList = _studentDataAccess.GetAllStudents(GetCurrentStudioId());
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

            student.DateOfBirth = DateTime.ParseExact(student.DateOfBirthToString, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            _studentDataAccess.AddNewStudent(student, studioId);
            return RedirectToAction("Students");
        }

        public void EditStudent(Student student)
        {
            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);

            _studentDataAccess.UpdateStudent(student, userId);
        }

        /// <summary>
        /// Students page.
        /// </summary>
        /// <param name="student"></param>
        public void DeleteStudent(Student student)
        {
            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);

            _studentDataAccess.DeleteStudent(student, userId);
        }

        /// <summary>
        /// Edit class modal.
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public void DeleteStudentFromClass(int id, int classid)
        {
            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);

            _studentDataAccess.DeleteStudentFromClass(id, classid, userId);
        }

        public IActionResult Instructor()
        {
            ViewBag.text = "Instructors";
            ViewBag.StudioName = _studioDataAccess.GetStudioInfo(GetCurrentStudioId()).Name;
            return View();
        }

        public IActionResult GetInstructor(string firstname, string lastname, string email)
        {
            List<Instructor> instructorList = new List<Instructor>();
            Instructor instructor = new Instructor();

            if ((string.IsNullOrEmpty(firstname) || firstname == "null")
                && (string.IsNullOrEmpty(lastname) || lastname == "null")
                && (string.IsNullOrEmpty(email) || email == "null"))
            {
                instructorList = _instructorDataAccess.GetAllInstructors(GetCurrentStudioId());
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
            instructor.DateOfBirth = DateTime.ParseExact(instructor.DateOfBirthToString, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            int studioId = GetCurrentStudioId();
            _instructorDataAccess.AddNewInstructor(instructor, studioId);
            return RedirectToAction("Instructor");
        }

        public void EditInstructor(Instructor instructor)
        {
            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);

            _instructorDataAccess.UpdateInstructor(instructor, userId);
        }
        
        /// <summary>
        /// Instructor page.
        /// </summary>
        /// <param name="instructor"></param>
        public void DeleteInstructor(Instructor instructor)
        {
            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);

            _instructorDataAccess.DeleteInstructor(instructor, userId);
        }

        /// <summary>
        /// Edit class modal.
        /// </summary>
        /// <param name="id"></param>
        [HttpPost]
        public void DeleteInstructorFromClass(int id, int classId)
        {
            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);

            _instructorDataAccess.DeleteInstructorFromClass(id, userId, classId);
        }

        public IActionResult Classes(string classError)
        {
            ViewBag.text = "Classes";
            ViewBag.StudioName = _studioDataAccess.GetStudioInfo(GetCurrentStudioId()).Name;
            var _class = new ClassStudentVM();
            var studentsList = _studentDataAccess.GetAllStudents(GetCurrentStudioId());
            var instructorList = _instructorDataAccess.GetAllInstructors(GetCurrentStudioId());

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
            var allStudents = _studentDataAccess.GetAllStudents(GetCurrentStudioId());

            if (genre == null && level == null && type == null)
            {
                classesList = _classDataAccess.GetAllClasses(GetCurrentStudioId());
                foreach (var c in classesList)
                {
                    var instructorsIds = _classDataAccess.GetInstructorsConnectedToClass(c.Id, GetCurrentStudioId());
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

                    var studentsIds = _classDataAccess.GetStudentsConnectedToClass(c.Id, GetCurrentStudioId());
                    foreach (var sId in studentsIds)
                    {
                        var student = _studentDataAccess.GetStudentById(sId);
                        c.Students.Add($" {student.Firstname} + {student.Lastname} ");
                    }

                }

            }else
            {
               List<Class> _classes = new List<Class> ();

                _classes = _classDataAccess.SearchClass(genre, level, type, GetCurrentStudioId());
                foreach (var _class in _classes)
                {
                    var instructorsIds = _classDataAccess.GetInstructorsConnectedToClass(_class.Id, GetCurrentStudioId());
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

                    var studentsIds = _classDataAccess.GetStudentsConnectedToClass(_class.Id, GetCurrentStudioId());
                    foreach (var sId in studentsIds)
                    {
                        var student = _studentDataAccess.GetStudentById(sId);
                        _class.Students.Add($" {student.Firstname} + {student.Lastname} ");
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
            newclass.StartDay = _class.StartDay;
            newclass.StartDay = DateTime.ParseExact(_class.StartDayToString, "MM/dd/yyyy", CultureInfo.InvariantCulture);

            int studioId = GetCurrentStudioId();
            int classId = 0;

            var classes = _classDataAccess.GetAllClasses(GetCurrentStudioId());

            foreach (var c in classes)
            {
                var shedule = _classDataAccess.GetClassShedule(c.Id);

                if ((c.Genre == newclass.Genre) && (c.Level == newclass.Level))
                {
                    string classError = "There is already a class with the same genre and level!";
                    return RedirectToAction("Classes", new { classError });
                }

                foreach (var day in _class.SheduleDays)
                {
                    foreach (var s in shedule)
                    {
                        if (day.Equals(s.Day) && _class.Hour == s.Hour)
                        {
                            string classError = "There is already a class on the same day and hour!";
                            return RedirectToAction("Classes", new { classError });
                        }
                    }
                    
                }
            }

            _classDataAccess.AddNewClass(newclass, studioId);

            foreach (var day in _class.SheduleDays)
            {
                classId = _classDataAccess.GetClassId(newclass);
                _classDataAccess.AddDayToShedule(day, classId, _class.Hour, studioId);
            }

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
            var instructorList = _classDataAccess.GetInstructorsConnectedToClass(id, GetCurrentStudioId());
            var studentsIdList = _classDataAccess.GetStudentsConnectedToClass(id, GetCurrentStudioId());

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

        public IActionResult UpdateClass(ClassStudentVM _class)
        {
            var _classToUpdate = new Class();

            ClaimsPrincipal currentUser = User;
            var claims = currentUser.Claims;
            var userEmail = "";
            foreach (var c in claims) userEmail = c.Value;
            var newUser = new User();
            newUser.Email = userEmail;
            var userId = _userDataAccess.GetUserId(newUser);

            _classToUpdate.Id = _class.ClassId;

            if (_class.PricePerHour != 0) _classToUpdate.PricePerHour = _class.PricePerHour;

            var classes = _classDataAccess.GetAllClasses(GetCurrentStudioId());

            foreach (var c in classes)
            {
                var shedule = _classDataAccess.GetClassShedule(c.Id);

                if ((c.Genre == _class.Genre) && (c.Level == _class.Level))
                {
                    string classError = "There is already a class with the same genre and level!";
                    return RedirectToAction("Classes", new { classError });
                }

                foreach (var day in _class.SheduleDays)
                {
                    foreach (var s in shedule)
                    {
                        if (day.Equals(s.Day) && _class.Hour == s.Hour)
                        {
                            string classError = "There is already a class on the same day and hour!";
                            return RedirectToAction("Classes", new { classError });
                        }
                    }

                }
            }

            if (_class.Level != null) _classToUpdate.Level = _class.Level;

            if (_class.ClassType != null) _classToUpdate.ClassType = _class.ClassType;

            _classDataAccess.UpdateClass(_classToUpdate, userId);

            if (_class.SheduleDays != null)
            {
                var classShedule = _classDataAccess.GetClassShedule(_class.ClassId);

                if (classShedule.Count > 1)
                {
                    for (int day = 0; day <= _class.SheduleDays.Count - 1; day++)
                    {
                        _classDataAccess.UpdateShedule(_class.SheduleDays[day], _class.Hour, _class.ClassId, userId, classShedule[day].Id);
                    }
                }
                else
                {
                    for (int day = 0; day < _class.SheduleDays.Count - 1; day++)
                    {
                        _classDataAccess.UpdateShedule(_class.SheduleDays[day], _class.Hour, _class.ClassId, userId, classShedule[0].Id);
                        if (_class.Hour != null)
                        {
                            _classDataAccess.AddDayToShedule(_class.SheduleDays[day + 1], _class.ClassId, _class.Hour, GetCurrentStudioId());
                        }else
                        {
                            _classDataAccess.AddDayToShedule(_class.SheduleDays[day + 1], _class.ClassId,"18:30-19:30", GetCurrentStudioId());
                        }
                    }
                }
            }

            if (_class.InstructorsIds != null) {
                foreach (var id in _class.InstructorsIds)
                {
                    _classDataAccess.AddInstructorToClass(id, _class.ClassId);
                }
            }

            if (_class.StudentsIds != null)
            {
                foreach (var id in _class.StudentsIds)
                {
                    _classDataAccess.AddStudentToClass(id, _class.ClassId);
                }
            }

            return RedirectToAction("Classes");
        }

        public void DeleteClass(int classId)
        {
            _classDataAccess.RemoveClass(classId);
        }

        public IActionResult DashboardChart()
        {
            int[] data = new int[13];
            int[] months = new int[12] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            List<Attendance> attendances = _attendanceDataAccess.GetAllAttendances(GetCurrentStudioId());
            foreach (var month in months)
            {
                int studentsCount = 0;
                foreach (var a in attendances)
                {
                    if (month == a.Date.Month)
                    {
                        foreach (var s in _classDataAccess.GetStudentsConnectedToClass(a.ClassId, GetCurrentStudioId()))
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
            var allAtendances = _attendanceDataAccess.GetAllAttendances(GetCurrentStudioId());


            for (int d = 0; d < 7; d++)
            {
                double finalProfit = 0;

                if (currentDay >= before7Days)
                {
                    foreach (var at in allAtendances)
                    {
                        double profit = 0;
                        if (at.Date == currentDay)
                        {
                            var numberOfStudents = 0;
                            double instructorPay = 0;
                            var procent = 0;

                            numberOfStudents += _classDataAccess.GetStudentsConnectedToClass(at.ClassId, GetCurrentStudioId()).Count;
                            foreach (var i in _classDataAccess.GetInstructorsConnectedToClass(at.ClassId, GetCurrentStudioId()))
                            {
                                var instructor = _instructorDataAccess.GetInstructorById(i);
                                procent += instructor.procentOfProfit;
                            }

                            profit = numberOfStudents * (_classDataAccess.SearchClass(at.ClassId,GetCurrentStudioId()).PricePerHour);
                            instructorPay = (procent / 100) * profit;
                            profit = profit - instructorPay;
                        }
                        finalProfit += profit;
                    }
                    dataDaysOfWeek[d] = currentDay.DayOfWeek.ToString();
                    dataProfit[d] = Math.Round(finalProfit);
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
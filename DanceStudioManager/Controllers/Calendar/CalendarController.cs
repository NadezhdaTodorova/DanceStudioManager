using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DanceStudioManager
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly CalendarHelp calendarHelp;
        private readonly CalendarDataAccess calendarDataAccess;
        private readonly UserDataAccess _userDataAccess;
        public CalendarController(CalendarHelp _calendarHelp, CalendarDataAccess _calendarDataAccess,
            UserDataAccess userDataAccess)
        {
            calendarDataAccess = _calendarDataAccess;
            calendarHelp = _calendarHelp;
            _userDataAccess = userDataAccess;
        }
        public IActionResult Index(int? year, int? month)
        {
            ViewBag.text = "Calendar";
            //calendarHelp.AddDaysToCalendar();
            List<DayVM> days = calendarDataAccess.GetDays();

            var model = new CalendarSearchVM();
            model.Year  =  year ?? DateTime.Now.Year;
            model.Month = month ?? DateTime.Now.Month;
            model.Days = days;

            model.CalendarData = calendarDataAccess.GetAllClassesShedule(model, GetCurrentStudioId());

            return View("Views/Studio/Calendar.cshtml", model);
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
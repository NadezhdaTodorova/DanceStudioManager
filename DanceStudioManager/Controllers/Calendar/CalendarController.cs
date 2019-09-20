using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DanceStudioManager
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly CalendarHelp calendarHelp;
        private readonly CalendarDataAccess calendarDataAccess;
        public CalendarController(CalendarHelp _calendarHelp, CalendarDataAccess _calendarDataAccess)
        {
            calendarDataAccess = _calendarDataAccess;
            calendarHelp = _calendarHelp;
        }
        public IActionResult Index(int? year, int? month)
        {
            ViewBag.text = "Calendar";

            List<DayVM> days = calendarDataAccess.GetDays();

            var model = new CalendarSearchVM();
            model.Year  =  year ?? DateTime.Now.Year;
            model.Month = month ?? DateTime.Now.Month;
            model.Days = days;

            model.CalendarData = calendarDataAccess.GetAllClassesShedule(model);

            return View("Views/Studio/Calendar.cshtml", model);
        }


    }
}
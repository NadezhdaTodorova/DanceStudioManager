using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager
{
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
            var model = new CalendarSearchVM();
            model.Year  =  year ?? DateTime.Now.Year;
            model.Month = month ?? DateTime.Now.Month;

            model.CalendarData = calendarDataAccess.GetAllClassesShedule(model);

            return View("Views/Studio/Calendar.cshtml", model);
        }
    }
}
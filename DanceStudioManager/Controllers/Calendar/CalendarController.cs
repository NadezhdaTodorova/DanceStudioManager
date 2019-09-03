using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DanceStudioManager
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.text = "Calendar";
            var model = new CalendarSearchVM();
            model.Year = 2019;
            model.Month = 8;
            var calendarData = new CalendarData();
            calendarData.Name = "Salsa";
            calendarData.Hour = "17:30";
            model.CalendarData.Add(calendarData);
            var day = new DayVM();
            day.Day = DateTime.Now;
            day.IsHoliday = false;
            day.WorkDay = true;
            model.Days.Add(DateTime.Now, day);
            return View("Views/Studio/Calendar.cshtml", model);


            //ReportsCalendarSearchVM model = new ReportsCalendarSearchVM();

            //model.Year = year ?? DateTime.Now.Year;
            //model.Month = month ?? DateTime.Now.Month;
            //GetDaysForYear(model);

            //var calendarData = _requestService.GetReportsCalendarData(new { model.Year, model.Month });
            //model.CalendarData = _mapper.Map<List<ReportsCalendarDataVM>>(calendarData);

            //return View(model);

        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public  class CalendarHelp
    {
        private readonly CalendarDataAccess calendarDataAccess;

        public CalendarHelp(CalendarDataAccess _calendarDataAccess)
        {
            calendarDataAccess = _calendarDataAccess;
        }

        public void AddDaysToCalendar()
        {
            var year = 2020;
            var DateFrom = new DateTime(year, 1, 1);
            var DateTo = new DateTime(year, 12, 31);

            List<DayVM> days = new List<DayVM>();

            DateTime day = DateFrom;

            while (day <= DateTo)
            {
                var d = new DayVM();

                d.Day = day;
                d.WorkDay = IsBusinessDay(day);

                days.Add(d);
                day = day.AddDays(1);
            }

            calendarDataAccess.AddDaysToCalendar(days);

        }

        public bool IsBusinessDay(DateTime date)
        {
            return
                date.DayOfWeek != DayOfWeek.Saturday &&
                date.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DanceStudioManager
{
    public class CalendarEventGenerator
    {
        readonly CalendarSearchVM _calendarData;
        readonly DateTime _day;
        public CalendarEventGenerator(CalendarSearchVM calendarData, DateTime day)
        {
            _calendarData = calendarData;
            _day = day;
        }

        public List<XElement> GetEventElements()
        {
            List<XElement> elements = new List<XElement>();
            foreach (var calData in _calendarData.CalendarData.OrderBy(x => x.Name))
            {
                if (_day.Date == calData.DateFrom.Date)
                {
                    elements = GetEventForDay(calData).AddEventElement();
                }
            }

            return elements;
        }

        public EventState GetEventForDay (CalendarData data)
        {
            return new EventState(data);
        }

    }
}

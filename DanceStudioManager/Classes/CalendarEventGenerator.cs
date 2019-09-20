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
            foreach (var calData in _calendarData.CalendarData.OrderBy(x => x.Hour))
            {
                if (calData.SheduleDays.Any(x => x.Day.Date == _day.Date))
                {
                    var title = GetEventForDay(calData).GetPoupHtmlDescription();
                    var element = GetEventForDay(calData).AddEventElement(title);
                    elements.Add(element);
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

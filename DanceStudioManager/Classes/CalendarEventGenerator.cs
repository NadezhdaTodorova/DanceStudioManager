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
            var calendarData = new CalendarData();
            calendarData.Name = "Salsa";
            calendarData.Hour = "17:30";
            elements.Add(new XElement("p",
                        new XAttribute("class", "d-lg-none"),
                        "Salsa"));

            return elements;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DanceStudioManager
{
    public class EventState
    {
        protected readonly CalendarData data;
        public EventState(CalendarData _data)
        {
            data = _data;
        }

        public XElement AddEventElement(XAttribute title)
        {
            var element = new XElement("a",
                        new XElement("mark",
                        new XAttribute("data-placement", "top"),
                        new XAttribute("class", $" event d-block p-1 pl-2 pr-2 mb-1 rounded text-truncate font-italic name"), title,
                        new XAttribute("data-html", "true"),
                        new XAttribute("data-toggle", "tooltip"),
                            $"{data.Name} {data.Hour}"));
            return element;

        }

        public XAttribute GetPoupHtmlDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<em class='d-block'>{data.Name}</em>");
            sb.Append($"<i> Level: </i> <strong> <i>{data.Level}<i></strong>");
            sb.Append($"<div><i> Hour: </i><strong><i>{data.Hour}</i></strong></div>");
            sb.Append($"<div><i>Number of students: </i><small><strong>{data.NumberOfStudents}</strong></small></div>");
            foreach (var i in data.Instructors)
            {
                sb.Append($"<div><i>Instructor: </i><small><strong>{i.Firstname}</strong></small></div>");
            }
            return new XAttribute("title", sb.ToString());
        }
    }
}

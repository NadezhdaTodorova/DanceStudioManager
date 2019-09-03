using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<XElement> AddEventElement()
        {
            List<XElement> elements = new List<XElement>();
            var element =  new XElement("a",
                        new XElement("mark",
                        new XAttribute("class", $" event d-block p-1 pl-2 pr-2 mb-1 rounded text-truncate font-italic"),
                        new XAttribute("data-html", "true"),
                        new XAttribute("data-toggle", "tooltip"),
                            data.Name));

            elements.Add(element);

            return elements;
        }
    }
}

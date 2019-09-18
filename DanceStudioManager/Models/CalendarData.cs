using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class CalendarData
    {
        public string Name { get; set; }
        public List<DayVM> SheduleDays { get; set; } = new List<DayVM>();
        public Dictionary<DateTime, DayVM> Days { get; set; } = new Dictionary<DateTime, DayVM>();
        public string Hour { get; set; }
        public string Shedule { get; set; }
        public string  Level { get; set; }
        public int  NumberOfStudents { get; set; }
        public List<Instructor>  Instructors { get; set; }
    }
}

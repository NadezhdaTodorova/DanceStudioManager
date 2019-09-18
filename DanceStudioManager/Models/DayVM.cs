using System;

namespace DanceStudioManager
{
    public class DayVM
    {
        public DateTime Day { get; set; }
        public bool WorkDay { get; set; }

        public bool IsHoliday
        {
            get
            {
                return !WorkDay;
            }
            set
            {
                WorkDay = !value;
            }
        }
    }
}
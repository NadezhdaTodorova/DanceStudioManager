using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class Attendance
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int StudioId { get; set; }
        public DateTime Date { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class Shedule
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public int StudioId { get; set; }
    }
}

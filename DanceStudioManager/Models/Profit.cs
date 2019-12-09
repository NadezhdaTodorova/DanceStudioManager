using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class Profit
    {
        [Required]
        public string DateFrom{ get; set; }
        [Required]
        public string DateTo{ get; set; }
        public string ClassGenre { get; set; }
        public string Level { get; set; }
        public double ProfitForPeriod { get; set; }
        public string Type { get; set; }
        public string ClassType { get; set; }
        public int NumberOfStudents { get; set; }
        public int Attendances { get; set; }
        public List<string> instructors { get; set; } = new List<string>();

    }
}

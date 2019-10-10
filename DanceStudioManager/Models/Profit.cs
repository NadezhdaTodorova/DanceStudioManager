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
        public DateTime DateFrom{ get; set; }
        [Required]
        public DateTime DateTo{ get; set; }
        public string ClassGenre { get; set; }
        public string Level { get; set; }
        public double ProfitForPeriod { get; set; }
        public string Type { get; set; }
        public int NumberOfStudents { get; set; }
        public List<Instructor> instructors { get; set; } = new List<Instructor>();

    }
}

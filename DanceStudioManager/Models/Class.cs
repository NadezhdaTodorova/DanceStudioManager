using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class Class
    {
        [Required]
        public string Genre { get; set; }
        public int Id { get; set; }
        [Required]
        public string Level { get; set; }
        [Required]
        public double PricePerHour { get; set; }
        [Required]
        public int SheduleId { get; set; }
        [Required]
        public int ClassTypeId { get; set; }
        public List<string> Shedule { get; set; } = new List<string>();
        public string  ClassType  { get; set; }
        public int  NumberOfStudents  { get; set; }
        public List<string> Instructors { get; set; } = new List<string>();
        public string Hour{ get; set; }

        public string  Day { get; set; }

    }
}

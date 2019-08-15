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
        [Required]
        public string Level { get; set; }
        [Required]
        public float PricePerHour { get; set; }
        [Required]
        public int SheduleId { get; set; }
        [Required]
        public int ClassTypeId { get; set; }
        public string Shedule{ get; set; }
        public string  ClassType  { get; set; }
    }
}

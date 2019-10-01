using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class ClassStudentVM
    {
        [Required]
        public string Genre { get; set; }
        public string Level { get; set; }
        [Required]
        public double PricePerHour { get; set; }
        [Required]
        public int SheduleId { get; set; }
        [Required]
        public int ClassTypeId { get; set; }
        public string ClassType { get; set; }
        public int NumberOfStudents { get; set; }
        public int[] StudentsIds { get; set; }
        public int[] InstructorsIds { get; set; }
        public string[] SheduleDays { get; set; }
        public List<SelectListItem> Students { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Instructors { get; set; } = new List<SelectListItem>();
        public string Hour { get; set; }
        public string Firstname{ get; set; }
        public string Lastname{ get; set; }
        public string Email{ get; set; }
        public string CellPhone{ get; set; }
        public string Gender{ get; set; }



    }
}

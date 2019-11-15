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
        [Required]
        public string Level { get; set; }
        [Required]
        public double PricePerHour { get; set; }
        [Required]
        public int SheduleId { get; set; }
        [Required]
        public int ClassTypeId { get; set; }
        [Required]
        public string ClassType { get; set; }
        public int NumberOfStudents { get; set; }
        public int[] StudentsIds { get; set; }
        public int[] InstructorsIds { get; set; }
        public List<string> SheduleDays { get; set; } = new List<string>();
        public List<SelectListItem> Students { get; set; } = new List<SelectListItem>();
        public List<Student> StudentsList { get; set; } = new List<Student>();
        public List<SelectListItem> Instructors { get; set; } = new List<SelectListItem>();
        public List<Instructor> InstructorsList { get; set; } = new List<Instructor>();
        public string Hour { get; set; }
        public string Firstname{ get; set; }
        public string Lastname{ get; set; }
        public string Email{ get; set; }
        public string CellPhone{ get; set; }
        public string Gender{ get; set; }
        public int Id { get; set; }
        public int ClassId { get; set; }
        [Required]
        public DateTime StartDay { get; set; }



    }
}

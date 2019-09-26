using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class DashboardNeeds
    {
        public List<Class> classesForToday = new List<Class>();
        public int ClassId { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();
        public List<SelectListItem> AllStudents { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AllInstructors { get; set; } = new List<SelectListItem>();
        public int[] AllStudentsIds { get; set; }
        public int[] AllInstructorsIds { get; set; }
        public double PricePerHour { get; set; }
        public string Level { get; set; }
    }
}

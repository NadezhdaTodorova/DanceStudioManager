using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class Student
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CellPhone { get; set; }
        public string Email{ get; set; }
        public bool SendEmail { get; set; }
        public string Gender { get; set; }
        public int StudioId { get; set; }
    }
}

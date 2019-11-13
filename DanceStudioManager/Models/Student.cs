using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DanceStudioManager
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string CellPhone { get; set; }
        public string Email{ get; set; }
        public bool SendEmail { get; set; }
        public string Gender { get; set; }
        public int StudioId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DateOfBirthToString { get; set; }
    }
}

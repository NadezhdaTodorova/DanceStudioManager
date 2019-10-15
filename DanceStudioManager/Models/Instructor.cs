using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class Instructor
    {
        public int Id { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public bool SendEmail { get; set; }
        public string Gender { get; set; }
        public int StudioId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DateOfBirthToString { get; set; }
        [Required]
        public int procentOfProfit { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string StudioName { get; set; }
        public int StudioId { get; set; }
        public bool ConfirmAccount  { get; set; }
        public string Salt { get; set; }
        public string CellPhone { get; set; }
        public string PhotoUrl { get; set; }
        public int NumberOfStudents { get; set; }
        public int NumberOfClasses { get; set; }
        public int NumberOfInstructors { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
    }
}

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
        public string Password { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string StudioName { get; set; }
        public bool ConfirmAccount  { get; set; }
        public byte[] Salt { get; set; }
    }
}

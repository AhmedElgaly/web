using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "user Name")]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public int Ssn { get; set; }
        public string Address { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public int Password { get; set; }
        public int Age { get; set; }
        public string Role { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    public class trip
    {
        public int id { get; set; }
        [Required(ErrorMessage = "You have enter distance")]
        public int distance { get; set; }
        [Required(ErrorMessage = "You have enter date")]
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        [Required(ErrorMessage = "You have select city")]
        public string from { get; set; }
        [Required(ErrorMessage = "You have select city")]
        public string to { get; set; }
        [Required(ErrorMessage = "You have enter price")]
        public int price { get; set; }
        [Required(ErrorMessage = "You have enter duration")]
        public int duration { get; set; }
        public int num_buses { get; set; } 
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    [Table("Bus")] 
    public class Bus
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "A Driver name is required")]
        public string Driver_name { get; set; }

        [Required(ErrorMessage = "A Color of Bus is required")]
        public string color { get; set; }

        [Required(ErrorMessage = "A Capacity of Bus is required")]
        public int Capacity { get; set; }

        [Required(ErrorMessage = "A Number chairs of Bus is required")]
       // [Compare("Capacity", ErrorMessage = "A Number chairs of Bus is Must Equal Capacity of it")]
        public int Num_Chairs { get; set; }

        [Required(ErrorMessage = "You have select tripid")]
        [ForeignKey("trip")]
        public int tripid { get; set; }
        public trip trip { get; set; }

       
    }
}
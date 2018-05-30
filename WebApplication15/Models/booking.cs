using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
     [Table("booking")] 
    public class booking
    {
          public int Id { get; set; }
        
        [Required(ErrorMessage ="you have enter number of chairs")]
        [Range(1, 24, ErrorMessage = "The value must be between 1 - 24")]
        public int NumOFChair { get; set; }

        public bool? booked { get; set; } 

        [Display(Name = "pay credit")]
        public bool payment { get; set; } 

        public int cost { get; set; }
       
        public IEnumerable<trip> trips { get; set; }

       
        [ForeignKey("Bus")]
        public int? busid { get; set; }
        public Bus Bus { get; set; }
        
        [ForeignKey("User")]
        public int personid { get; set; }
        public User User { get; set; }

        public DateTime time { get; set; }

        [ForeignKey("trip")]     
        public int? t_id { get; set; }
        public trip trip { get; set; }
    }
}
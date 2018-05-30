using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    [Table("Bill")]
    public class Bill
    {
        public int id { get; set; }
        public int? cost { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string type { get; set; }
        
        
        public DateTime? time { get; set; }

        [ForeignKey("Bus")]
        public int? busid { get; set; }
        public Bus Bus { get; set; }

        [ForeignKey("User")]
        public int? personid { get; set; }
        public User User { get; set; }

      
        [ForeignKey("trip")]
        public int? t_id { get; set; }
        public trip trip { get; set; }
    }
}
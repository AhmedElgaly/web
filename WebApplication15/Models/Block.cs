using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace WebApplication15.Models
{   [Table("Block")]
    public class Block
    {
    [Key]
    public int Id { get; set; }
        [ForeignKey("Bus")]
        public int busid { get; set; }
        public Bus Bus { get; set; }

        [ForeignKey("User")]
        public int personid { get; set; }
        public User User { get; set; }

        public IEnumerable<trip> trips { get; set; }

      //  public IEnumerable<Bus> Buss { get; set; }

        [ForeignKey("trip")]
        public int? t_id { get; set; }
        public trip trip { get; set; }
    }
}
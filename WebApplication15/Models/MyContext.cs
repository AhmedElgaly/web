using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication15.Models
{
    public class MyContext:DbContext
    {
        public DbSet<User> User { get; set; }
       public DbSet<contries> contries { get; set; }
       public DbSet<trip> trips { get; set; }
       public DbSet<Bus> Bus { get; set; }
       public DbSet<booking> booking { get; set; }
        public DbSet<Bill> Bill { get; set; }
        public DbSet<Block> Block { get; set; }
        public MyContext()
            : base("DefaultConnection")
        {
        }
    }
}
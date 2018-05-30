using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication15.Models;

namespace WebApplication15.ModelView
{
    public class BusTrip
    {
        public IEnumerable<trip> trip { get; set; }

        public Bus bus { get; set; }
    }
}
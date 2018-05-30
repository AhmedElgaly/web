using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication15.Models;

namespace WebApplication15.ModelView
{
    public class bookbus
    {
        public booking booking { get; set; }

        public IEnumerable<Bus> bus { get; set; }
    }
}
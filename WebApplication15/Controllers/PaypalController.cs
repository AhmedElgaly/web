using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication15.Models;

namespace WebApplication13.Controllers
{
    public class PaypalController : Controller
    {
        // GET: Paypal
        public ActionResult Index()
        {
            if (Session["cart"] == null)
            {
                RedirectToAction("Index", "Book");
            }
            var ls = Session["cart"] as List<booking>;
            return View(ls);
        }

        public ViewResult get_data_paypal()
        {
            
            return View();
        }


    }
}
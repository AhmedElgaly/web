using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WebApplication15.Models;

namespace WebApplication11.Controllers
{
    public class HomeController : Controller
    {
        MyContext db = new MyContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
       [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
      /*  [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {

                user.UserName = user.UserName;
                user.Password = user.Password;
                user.Ssn = user.Ssn;
                user.Mobile = user.Mobile;
                user.Email = user.Email;
                user.Address = user.Address;
                if (db.user.Any(o => o.Email == user.Email && o.UserName == user.UserName))
                {
                    ViewBag.Exist = "Your Email ";
                }
                else
                {
                    user.Role = "Passenger";
                    db.user.Add(user);
                    db.SaveChanges();
                }
                //return RedirectToAction("login", "Home");
                ViewBag.message = "You Are Successfully Registered";
                return Json(new { result = 1 });
            }
            return View();
        }
        [HttpGet]
        public ActionResult login()
        {
            if (Session["UserName"] != null)
            {
                return RedirectToAction("mainpage", "Home", new { username = Session["UserName"].ToString() });
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult login(User user)
        {
            var loggedin = db.user.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password && x.Role == "Passenger");
            var loggedadmin = db.user.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password && x.Role == "Admin");
            if (loggedin != null)
            {
                ViewBag.message = "Loggedin";
                ViewBag.triedOnce = "yes";
                Session["UserName"] = user.UserName;
                return RedirectToAction("mainpage", "Home", new { username = user.UserName });
            }
            else if (loggedadmin != null)
            {
                ViewBag.message = "Loggedin";
                ViewBag.triedOnce = "yes";
                Session["UserName"] = user.UserName;
                return RedirectToAction("adminpage", "Home", new { username = user.UserName });
            }
            else
            {
                ViewBag.triedOnce = "yes";
                return View();
            }
        }
        public ActionResult mainpage(string username)
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "Home");
            }
            else
            {
                ViewBag.username = Session["UserName"];
                return View();
            }
        }
        public ActionResult adminpage(string username)
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("login", "Home");
            }
            else
            {
                ViewBag.username = Session["UserName"];
                return View();
            }
        }
        public ActionResult logoff()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }*/
        public ActionResult send_Sms()
        {
            var accountSid = "AC29616f78fe083eea49e28e52f4c8b99e";
            // Your Auth Token from twilio.com/console
            var authToken = "561dd1a36f8ea5e77adba28e1325889f";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                to: new PhoneNumber("+201152406053"),
                from: new PhoneNumber("+18506080156"),
                body: "Notification SMS from graduation Project System, A new team request to register project with you");
            return View();
        } 
    }
}
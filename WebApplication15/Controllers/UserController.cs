using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication15.Models;
using WebApplication15.Report;

namespace WebApplication15.Controllers
{

    public class UserController : Controller
    {
        MyContext db = new MyContext();

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
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
                user.Age = user.Age;
                if (db.User.Any(o => o.Email == user.Email && o.UserName == user.UserName))
                {
                    ViewBag.Exist = "You have Registered  ";
                }
                else
                {
                    user.Role = "Passenger";
                    db.User.Add(user);
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
                return RedirectToAction("mainpage", "User", new { username = Session["UserName"].ToString() });
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult login(User user)
        {
            var loggedin = db.User.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password && x.Role == "Passenger");
            var loggedadmin = db.User.SingleOrDefault(x => x.UserName == user.UserName && x.Password == user.Password && x.Role == "Admin");
            if (loggedin != null)
            {
                ViewBag.message = "Loggedin";
                ViewBag.triedOnce = "yes";
                Session["UserName"] = user.UserName;
                var row = db.User.Where(c => c.Password == user.Password).SingleOrDefault();
                Session["ID"] = row.Id;
                Session["Role"]=row.Role;
                return RedirectToAction("Index", "Home", new { username = user.UserName });
            }
            else if (loggedadmin != null)
            {
                ViewBag.message = "Loggedin";
                ViewBag.triedOnce = "yes";
                var row = db.User.Where(c => c.Password == user.Password).SingleOrDefault();
                Session["Role"] = row.Role;
                Session["UserName"] = user.UserName;
                return RedirectToAction("adminpage", "User", new { username = user.UserName });
            }
            else
            {
                ViewBag.triedOnce = "yes";
                return View();
            }
        }
        public ActionResult mainpage(string username)
        {

            if (Session["Role"].ToString() != "Passenger")
            {
                return RedirectToAction("login", "User");
            }
            else
            {
                ViewBag.username = Session["UserName"];
                return View();
            }
        }
        public ActionResult adminpage(string username)
        {

            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("login", "User");
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
            return RedirectToAction("Index","Home");
        }
        /**************************************Cancel (User)***********************************/
        public ActionResult showBookings()
        {
            var userid = (int)Session["ID"];
            var user_trips = db.booking.Where(v => v.personid == userid).ToList();

            return View(user_trips);
        }
        public ActionResult Cancel(int id)
        {
            var book = db.booking.Single(c => c.Id == id);
            var bill = db.Bill.Single(v => v.busid == book.busid && v.t_id == book.t_id);
            var bus_chairs = db.Bus.Where(n => n.ID == book.busid).SingleOrDefault();
            bus_chairs.Num_Chairs += book.NumOFChair;
          //  db.Buses.Add(bus_chairs);
            db.Bill.Remove(bill);
            db.booking.Remove(book);
            db.SaveChanges();
            return RedirectToAction("showBookings");

        }
        /*****************************ShowBill*********************************/
        public ActionResult ShowBill(int id)
        {
            var booked = db.booking.Where(x => x.Id == id).SingleOrDefault();
            var bill = db.Bill.Where(c => c.personid == booked.personid && c.t_id == booked.t_id && c.busid == booked.busid).SingleOrDefault();
            return View(bill);
        }
        public ActionResult report(Bill bill,int id)
        {
            BusReport br = new BusReport();
            var bill_id = db.Bill.Where(v => v.id == id).SingleOrDefault();
            byte[] abytes = br.PrepareReport(GetBus(bill_id.id));
            return File(abytes, "application/pdf");
        }
        public Bill GetBus(int id)
        {

            var b = db.Bill.Where(v => v.id == id).SingleOrDefault();
            return b;
        }

    }
}
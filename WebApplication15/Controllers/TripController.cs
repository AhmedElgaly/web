using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication15.Models;
using System.Web.Helpers;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types; 

namespace WebApplication15.Controllers
{
    public class TripController : Controller
    {
        MyContext db = new MyContext();
        // GET: Trip
        public ActionResult Index()
        {
            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("login", "User");
            }
            else
            {
                var trips = get_trip();
                return View(trips);
            }
        }

        public IEnumerable<trip> get_trip()
        {
            var trip = db.trips.ToList();

            return trip;
        }

       

        [HttpGet]
        public ActionResult newtrip()
        {
            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("login", "User");
            }
            else
            {
                var trip = db.contries.ToList();
                SelectList list = new SelectList(trip, "from", "from");
                ViewBag.from = list;

                SelectList list1 = new SelectList(trip, "to", "to");
                ViewBag.to = list1;
                return View();
            }
        }

        [HttpPost]
        public ActionResult newtrip(trip trip)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.trips.Add(trip);
                    db.SaveChanges();
                    return Json(new { result = 1 });
                }
                catch (Exception)
                {
                    var trips = db.contries.ToList();
                    SelectList list = new SelectList(trips, "from", "from");
                    ViewBag.from = list;

                    SelectList list1 = new SelectList(trips, "to", "to");
                    ViewBag.to = list1;
                    return View("newtrip", trip);
                }
            }
            else
            {
                var trips = db.contries.ToList();
                SelectList list = new SelectList(trips, "from", "from");
                ViewBag.from = list;

                SelectList list1 = new SelectList(trips, "to", "to");
                ViewBag.to = list1;
                return Json(new { result = 0 });
            }

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("login", "User");
            }
            else
            {

                var trip = db.trips.Single(x => x.id == id);
                SelectList list = new SelectList(db.trips.Where(x => x.id == id), "id", "from");
                ViewBag.from = list;

                SelectList list1 = new SelectList(db.trips.Where(x => x.id == id), "id", "to");
                ViewBag.to = list1;
                return View(trip);
            }
        }

        [HttpPost]
        public ActionResult Edit(trip trip)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var trips = db.trips.Single(x => x.id == trip.id);

                    trips.date = trip.date;
                    trips.distance = trip.distance;
                    trips.duration = trip.duration;
                    trips.from = trip.from;
                    trips.to = trip.to;
                    trips.price = trip.price;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    SelectList list = new SelectList(db.trips.Where(x => x.id == trip.id), "id", "from");
                    ViewBag.from = list;

                    SelectList list1 = new SelectList(db.trips.Where(x => x.id == trip.id), "id", "to");
                    ViewBag.to = list1;
                    return View("Edit", trip);
                }
            }
            else
            {

                SelectList list = new SelectList(db.trips.Where(x => x.id == trip.id), "id", "from");
                ViewBag.from = list;

                SelectList list1 = new SelectList(db.trips.Where(x => x.id == trip.id), "id", "to");
                ViewBag.to = list1;
                return View("Edit", trip);
            }


        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var trip = db.trips.Single(c => c.id == id);
            var bus = db.Bus.Where(c => c.tripid == id).ToList();
            foreach (var item in bus)
            {
                db.Bus.Remove(item);
            }
            db.trips.Remove(trip);
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }
        /********************************cancel Admin**********************/
        public ActionResult show_Booked_trip()
        {
            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("login", "User");
            }
            else
            {

                var trips = get_Booked_trip();
                return View(trips);
            }
        }
        public IEnumerable<trip> get_Booked_trip()
        {
            var trip = db.trips.Where(x => x.num_buses > 0).ToList();

            return trip;
        }
        /******************************Cancel Trip To Admin************************/

        public ActionResult Cancel(int id)
        {
            var trip_cancelled = db.trips.Single(z => z.id == id);
            trip_cancelled.num_buses = 0;
            db.SaveChanges();
            var busId = db.Bus.Where(m => m.tripid == trip_cancelled.id).ToList();
            foreach (var item in busId)
            {
             // ليست فيها الاتوبيسات اللى تبع الرحلات اللى اتلغت المفروض اجيب ليست تانيه فيها الناس اللى حاجزين فى الاتوبيس عشان اجيب الايميلات بتاعتهم//
                
                var passengers = db.booking.Where(n => n.busid == item.ID).ToList();
                item.Num_Chairs =item.Capacity; 
                foreach (var item11 in passengers)
                {
                    db.booking.Remove(item11);
                    db.SaveChanges();
                    var bills = db.Bill.Where(v => v.t_id == item11.t_id).ToList();
                    foreach (var item10 in bills)
                    {
                        db.Bill.Remove(item10);
                        db.SaveChanges();
                         var users = db.User.Where(x => x.Id == item10.personid).ToList();
                    foreach (var item12 in users)
                    {
                        SendEmail(item12.Email,id);
                        send_Sms(item12.Id);
                    }
                    }
         
                }
            }

            return RedirectToAction("show_Booked_trip");
        }
        public void SendEmail(string email,int id)
        {

            try
            {
                var trip_name = db.trips.Where(v => v.id == id).SingleOrDefault();
              
                WebMail.SmtpServer = "smtp.gmail.com";
    
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                
                WebMail.EnableSsl = true;
            
                WebMail.UserName = "busresevation@gmail.com";
                WebMail.Password = "20150559";

             
                WebMail.From = "busresevation@gmail.com";

             
                WebMail.Send(email, "subject", "Your Trip  "+trip_name.from+"  to  "+trip_name.to+ "  is Cancelled");
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }

        }
        /**********************************SMS**************************/
        public void send_Sms(int id)
        {
            var user = db.User.Where(c => c.Id == id).SingleOrDefault();
            var Mobile = user.Mobile;
            var accountSid = "AC29616f78fe083eea49e28e52f4c8b99e";
            // Your Auth Token from twilio.com/console
            var authToken = "561dd1a36f8ea5e77adba28e1325889f";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                to: new PhoneNumber("+2" + Mobile),
                from: new PhoneNumber("+18506080156"),
                body: "Notification SMS from Bus Online Resveration Your Trip is cancelled");

        } 

	}

}
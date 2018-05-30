using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using WebApplication15.Models;
using WebApplication15.ModelView;

namespace WebApplication13.Controllers
{
    public class BookController : Controller
    {
        //
        // GET: /Book/
        MyContext db =new MyContext();
      
        public ActionResult Index()
        {
            Block b = new Block();
            return View(db.trips.Where(c => c.num_buses > 0 && c.id !=b.t_id).ToList());
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            var bus = db.Bus.Where(c => c.tripid == id && c.Num_Chairs > 0);
            bookbus bb = new bookbus
            {
                bus = bus
            };


            return View(bb);
        }

        [HttpPost]
        public ActionResult Book(Bill bill, bookbus bb, int id)
        {

            if (ModelState.IsValid)
            {

                var bus = db.Bus.Where(c => c.ID == bb.booking.busid).SingleOrDefault();
                var trip = db.trips.Where(c => c.id == id).SingleOrDefault();
                /*if (db.booking.Any(o => o.busid == bus.ID && o.t_id == trip.id) && db.Bill.Any(z => z.t_id == trip.id && z.busid == bus.ID && z.personid == bill.personid))
                {
                    ViewBag.Exist = "have booked ";
                }
                else
                {*/
                    //var bus = db.Bus.Where(c => c.ID == bb.booking.busid).SingleOrDefault();
                    //var trip = db.trips.Where(c => c.id == id).SingleOrDefault();
                    var tickets = bb.booking.NumOFChair;
                    var chairs = bus.Num_Chairs;
                    bus.Num_Chairs = chairs - tickets;
                    var userid = bb.booking.personid;
                    var total = trip.price * tickets;
                    var cost = discount(userid, tickets, total);
                    bb.booking.time = DateTime.Now;
                    bb.booking.cost = cost;
                    bb.booking.booked = true;
                    bb.booking.t_id = trip.id;
                    bb.booking.booked = true;
                    bill.from = trip.from;
                    bill.to = trip.to;
                    bill.cost = cost;
                    bill.time = DateTime.Now;
                    bill.busid = bb.booking.busid;
                     bill.t_id = id;

                    bill.personid = bb.booking.personid;

                    if (bb.booking.payment == true)
                    {
                        bill.type = "credit";
                        payment(bb.booking);
                        if (db.booking.Any(o => o.busid == bb.booking.busid && o.t_id == bb.booking.t_id && o.personid==bb.booking.personid))
                        {
                            ViewBag.message = "you have booked";
                        }
                        else
                        {
                            SendEmail(userid, id, cost);
                            send_Sms(userid);
                            db.booking.Add(bb.booking);
                            db.Bill.Add(bill);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Paypal");
                        }
                    }
                    else
                    {
                        bill.type = "cash";
                        if (db.booking.Any(o => o.busid == bb.booking.busid && o.t_id == bb.booking.t_id && o.personid == bb.booking.personid))
                        {
                            ViewBag.message = "you have booked";
                        }
                        else
                        {
                            db.booking.Add(bb.booking);
                            db.Bill.Add(bill);
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

                            SendEmail(userid, id, cost);
                            send_Sms(userid);
                            return RedirectToAction("showBookings", "User");

                        }
                        return RedirectToAction("Book", bb);
                    }

                    //db.SaveChanges();


                    /* db.booking.Add(bb.booking);
                     db.Bill.Add(bill);
                     db.SaveChanges();*/
                    //  SendEmail(userid, id, cost);
                    //send_Sms(userid);
                    // Bill(id, (int)bb.booking.busid);

                    //return RedirectToAction("showBookings","User");
                
                return RedirectToAction("showBookings", "User");
            }
            else
            {

                var bus1 = db.Bus.Where(c => c.tripid == id && c.Num_Chairs > 0).ToList();
                bb.bus = bus1;
                return RedirectToAction("Book", bb);
            }

        }

        public int discount(int id, int tickets, int cost)
        {
            var user = db.User.Where(c => c.Id == id).SingleOrDefault();
            var age = user.Age;

            if (tickets < 4 && age >= 50)
            {
                var discount = cost - ((cost * 20) / 100);
                return discount;
            }
            else if (tickets >= 4 && tickets <= 8)
            {
                var discount = cost - ((cost * 30) / 100);
                return discount;
            }
            else if (tickets > 8)
            {
                var discount = cost - ((cost * 50) / 100);
                return discount;
            }
            else
            {
                return cost;
            }

        }


        public void SendEmail(int id, int tripid, int cost)
        {

            try
            {
                var user = db.User.Where(c => c.Id == id).SingleOrDefault();
                var trip = db.trips.Where(c => c.id == tripid).SingleOrDefault();
                var email = user.Email;
                //Configuring webMail class to send emails  
                //gmail smtp server  
                WebMail.SmtpServer = "smtp.gmail.com";
                //gmail port to send emails  
                WebMail.SmtpPort = 587;
                WebMail.SmtpUseDefaultCredentials = true;
                //sending emails with secure protocol  
                WebMail.EnableSsl = true;
                //EmailId used to send emails from application  
                WebMail.UserName = "busresevation@gmail.com";
                WebMail.Password = "20150559";

                //Sender email address.  
                WebMail.From = "busresevation@gmail.com";

                //Send email  
                WebMail.Send(email, "confirmation email", "dear " + Session["Username"] + "<br></br> your trip will be from " + trip.from + " to " + trip.to + " in this date " + trip.date + " and your cost will be " + cost + "$");
                ViewBag.Status = "Email Sent Successfully.";
            }
            catch (Exception)
            {
                ViewBag.Status = "Problem while sending email, Please check details.";

            }

        }

        
      /*public ActionResult Bill(int? tripid, int? busid)
        {
            var bus = busid;
            var trip = tripid;
            var userid = (int)Session["ID"];
            var bill = db.Bill.Where(c => c.personid == userid && c.t_id == trip && c.busid == bus).ToList();
           
            return View();
        }*/
        /**********************Block Passenger Admin******************************/
      [HttpGet]
      public ActionResult showtrips()
      {
          
              booking pt = new booking();
              pt.trips = db.trips.ToList();

              return View(pt);
          
          
      }
     [HttpPost]
      public ActionResult showtrips(Block b)
      {
         
          
          if (ModelState.IsValid)
          {
       
             
                 db.Block.Add(b);
                 var bus = db.Bus.Where(c => c.ID == b.busid).SingleOrDefault();
                 
                 var trip = db.trips.Where(c => c.id == b.t_id).SingleOrDefault();
                 var u_id = db.User.Where(c => c.Id == b.personid).SingleOrDefault();
                 var Row_booking = db.booking.Where(c => c.busid == bus.ID && c.t_id == trip.id && c.personid == u_id.Id).SingleOrDefault();
                    bus.Num_Chairs+=Row_booking.NumOFChair;
                 
             var Row_bill = db.Bill.Where(c => c.busid == bus.ID && c.t_id == trip.id && c.personid == u_id.Id).SingleOrDefault();
             
              db.booking.Remove(Row_booking);
                 db.Bill.Remove(Row_bill);
                 db.User.Remove(u_id);
                 SendNotification(u_id.Email,trip.id);
                 send_notification(u_id.Id);
              db.SaveChanges();
                
         
                   
                  return Json(new { result = 1 });
  
       
          }
          return Json(new { result = 0 });
      }
      public ActionResult GetLineBuses(int id)
      {
          var buses = db.Bus.Where(m => m.tripid == id).ToList();
          return Json(buses, JsonRequestBehavior.AllowGet);
      }
       
             public ActionResult GetUserBuses(int id)
      {
          var Users = db.booking.Where(m => m.busid == id).ToList();
          return Json(Users, JsonRequestBehavior.AllowGet);
      }
             public void SendNotification(string email,int id)
             {
                 var trip = db.trips.Where(c=>c.id==id).SingleOrDefault();
                 try
                 {
                     

                     WebMail.SmtpServer = "smtp.gmail.com";

                     WebMail.SmtpPort = 587;
                     WebMail.SmtpUseDefaultCredentials = true;

                     WebMail.EnableSsl = true;

                     WebMail.UserName = "busresevation@gmail.com";
                     WebMail.Password = "20150559";


                     WebMail.From = "busresevation@gmail.com";


                     WebMail.Send(email, "subject", "Your Are Blocked from Trip that going to "+trip.to+" from "+trip.from );
                     ViewBag.Status = "Email Sent Successfully.";
                 }
                 catch (Exception)
                 {
                     ViewBag.Status = "Problem while sending email, Please check details.";

                 }

             }
             public ActionResult payment(booking b)
             {

                 Session["cart"] = new List<booking>() { b };

                 ModelState.Clear();
                 return View();
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
                     to: new PhoneNumber("+2"+Mobile),
                     from: new PhoneNumber("+18506080156"),
                     body: "Notification SMS from Bus Online Resveration You Booked Trip");
                 
             }
             public void send_notification(int id)
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
                     body: "Notification SMS from Bus Online Resveration You are blocked from Trip and the system");

             } 

    }
	}

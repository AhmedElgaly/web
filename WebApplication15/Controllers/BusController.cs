using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication15.Models;
using WebApplication15.ModelView;

namespace WebApplication13.Controllers
{
    public class BusController : Controller
    {
        MyContext db = new MyContext();
        // GET: Bus
        public ActionResult Index()
        {
            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("login", "User");
            }
            else { 
            return View(db.Bus.ToList());
        }
            }

        [HttpGet]
        public ActionResult Add_New()
        {
            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("login", "User");
            }
            else
            {
                var lines = db.trips.ToList();
                BusTrip bt = new BusTrip()
                {
                    trip = lines
                };
                return View(bt);
            }
        }

        [HttpPost]
        public ActionResult Add_New(BusTrip bt)
        {

            if (ModelState.IsValid)
            {
                try
                {
                   var buses = db.trips.Where(c => c.id == bt.bus.tripid).SingleOrDefault();
                    buses.num_buses += 1;
                   
                    db.Bus.Add(bt.bus);
                    db.SaveChanges();
                    return Json(new { result = 1 });
                }
                catch (Exception)
                {
                    var lines = db.trips.ToList();
                    bt.trip = lines;
                    return View("Add_New", bt);
                }
            }
            else
            {
                var lines = db.trips.ToList();
                bt.trip = lines;
                return Json(new { result = 0 });
            }

        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("login", "User");
            }
            else
            {

                var bus = db.Bus.Single(a => a.ID == id);
                var lines = db.trips.ToList();
                BusTrip bt = new BusTrip
                {
                    trip = lines,
                    bus = bus
                };
                return View(bt);
            }
        }

        [HttpPost]
        public ActionResult Edit(BusTrip b)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var bus = db.Bus.Single(a => a.ID == b.bus.ID);

                    bus.Driver_name = b.bus.Driver_name;
                    bus.color = b.bus.color;
                    bus.Num_Chairs = b.bus.Num_Chairs;
                    bus.tripid = b.bus.tripid;

                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception)
                {
                    var lines = db.trips.ToList();
                    b.trip = lines;

                    return View("Edit", b);
                }
            }
            else
            {
                var lines = db.trips.ToList();
                b.trip = lines;
                return View("Edit", b);
            }

        }

        public ActionResult Delete(int id)
        {
            var bus = db.Bus.Single(c => c.ID == id);

            var trip = db.trips.Where(c => c.id == bus.tripid).SingleOrDefault();
            trip.num_buses -= 1;
            db.Bus.Remove(bus);
            db.SaveChanges();
            return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
        }

        
	}
}
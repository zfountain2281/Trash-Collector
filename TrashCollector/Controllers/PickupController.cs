using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TrashCollector.Extensions;

namespace TrashCollector.Controllers
{
    public class PickupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pickup
        public ActionResult Index()
        {
            return View();
        }

        // Post: Pickup/Add/Id
        //[HttpPost]
        public ActionResult Add(int trashCollectionId)
        {
            //get the trash collection based on address id
            Pickup pickup = new Pickup();
            pickup.DateCompleted = DateTime.Now;
            //TO DO: DON'T FIGURE OUT A WAY TO ELIMIATE THE MAGIC NUMBER BELOW
            //Ideally, the admin should be able to change the price? Maybe calculat the price based on Zip code?
            pickup.Price = 5.25d;
            db.Pickups.Add(pickup);
            db.SaveChanges();

            TrashCollection trashCollection = db.TrashCollections.First(t => t.TrashCollectionId == trashCollectionId);

            if (trashCollection.Pickups == null)
            {
                trashCollection.Pickups = new List<Pickup>();
            }
            trashCollection.Pickups.Add(pickup);
            db.SaveChanges();

            return RedirectToAction("Pickups", "Profile", new { todayOnly = true });
        }

        public ActionResult Delete(int? pickupId)
        {
            if (pickupId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pickup pickup = db.Pickups.Find(pickupId);
            if (pickup == null)
            {
                return HttpNotFound();
            }

            db.Pickups.Remove(pickup);
            db.SaveChanges();

            return RedirectToAction("Pickups", "Profile", new { todayOnly = true });
        }

    }
}
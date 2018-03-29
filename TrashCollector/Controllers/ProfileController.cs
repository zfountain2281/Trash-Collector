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
    public class ProfileController : Controller
    {
        //member variables
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profile
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            int profileId = Convert.ToInt32(User.Identity.GetProfileId());
            //var userProfile = db.Profiles.Include(p => p.Addresses).Include(p => p.TrashCollections).First(p => p.ProfileId == profileId);
            var userProfile = db.Profiles
                .Include(p => p.Addresses)
                .Include("Addresses.City")
                .Include("Addresses.State")
                .Include("Addresses.ZipCode")
                .Include("Addresses.TrashCollection")
                .First(p => p.ProfileId == profileId);
            return View(userProfile);
        }

        // GET: Profile/Details
        public ActionResult Details()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            //This is where the user's account information will go.
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "User");
            }
            else if (User.IsInRole("Employee"))
            {
                return RedirectToAction("Pickups", "Profile", new { todayOnly = true });
            }
            else
            {
                //This is the customer role view. Not a great structure. Should re-do this.
                //get list of invoices that the user owns that are unpaid. Calculate total amount due.
                var userId = User.Identity.GetUserId();
                var invoices = db.Invoices
                    .Include(i => i.Pickups)
                    .Where(i => i.UserId == userId)
                    .Where(i => i.IsPaid == false)
                    .ToList();

                double totalAmountDue = 0d;
                foreach (Invoice invoice in invoices)
                {
                    totalAmountDue += invoice.AmountDue;
                }
                //create a view model
                //populate it with the list of invoices and total amount due. Unless you want to get cray and put that logic in the view.
                var viewModel = new ProfileDetailsViewModel();
                viewModel.Invoices = invoices;
                viewModel.AmountDue = totalAmountDue;

                return View(viewModel);
            }
        }

        // GET: Profile/Addresses
        public ActionResult Addresses()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            int profileId = Convert.ToInt32(User.Identity.GetProfileId());
            var userProfile = db.Profiles
                .Include(p => p.Addresses)
                .Include("Addresses.City")
                .Include("Addresses.State")
                .Include("Addresses.ZipCode")
                .Include("Addresses.TrashCollection")
                .First(p => p.ProfileId == profileId);

            //test data
            //StringBuilder testData = new StringBuilder();
            //end of test data

            //return Content( userProfile.ToString() );
            return View(userProfile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Pickups(bool? todayOnly)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (User.IsInRole("Customer"))
            {
                return HttpNotFound();
            }

            //get all addresses that have a start date and are in the user's zipcodes
            int profileId = Convert.ToInt32(User.Identity.GetProfileId());
            var profile = db.Profiles.Find(profileId);

            string[] employeeZipcodes = profile.ZipCodes.Split(Convert.ToChar(","));

            List<Address> addresses;
            if ( todayOnly.HasValue && (bool)todayOnly )
            {
                string dayOfWeek = DateTime.Now.DayOfWeek.ToString();
                addresses = db.Addresses
                    .Include(a => a.TrashCollection)
                    .Include(a => a.TrashCollection.Pickups)
                    .Include(a => a.City)
                    .Include(a => a.State)
                    .Include(a => a.ZipCode)
                    .Where(a => employeeZipcodes.Contains(a.ZipCode.Number))
                    .Where(a => a.TrashCollection.PickUpDay == dayOfWeek)
                    .Where(a => a.TrashCollection.StartDate <= DateTime.Now)
                    .ToList();
            }
            else
            {
                addresses = db.Addresses
                    .Include(a => a.TrashCollection)
                    .Include(a => a.TrashCollection.Pickups)
                    .Include(a => a.City)
                    .Include(a => a.State)
                    .Include(a => a.ZipCode)
                    .Where(a => employeeZipcodes.Contains(a.ZipCode.Number))
                    .Where(a => a.TrashCollection.StartDate <= DateTime.Now)
                    .ToList();
            }

            //remove addresses from the list where the customer is on vacay
            for (int i=0; i<addresses.Count; i++)
            {
                if (addresses[i].TrashCollection.VacationStartDate.HasValue && addresses[i].TrashCollection.VacationEndDate.HasValue)
                {
                    DateTime vacationStart = (DateTime)addresses[i].TrashCollection.VacationStartDate;
                    DateTime vacationEnd = (DateTime)addresses[i].TrashCollection.VacationEndDate;
                    if (vacationStart < DateTime.Now)
                    {
                        if (vacationEnd > DateTime.Now)
                        {
                            addresses.RemoveAt(i);
                        }
                    }
                }
            }

            return View(addresses);
        }
    }
}

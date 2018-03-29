using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;

namespace TrashCollector.Controllers
{
    public class InvoiceController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invoice
        public ActionResult Index()
        {
            return View(db.Invoices.ToList());
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoice/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceId,UserId,AmountDue,IsPaid")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceId,UserId,AmountDue,IsPaid")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Generate()
        {
            //Zack - stubbing this out saved you a ton of time. Do it more often.
            //get all of the users
            //foreach user
                //get all of their addresses, including trash collections and pickups that are not invoiced
                //if they have un-invoiced pickups
                //create a new invoice
                //foreach of the users pickups
                    //add it to the invoice
                //save the invoice

            var users = db.Users
                .Include(u => u.Profile)
                .Include(u => u.Profile.Addresses)
                .ToList();

            foreach (ApplicationUser user in users)
            {
                List<Pickup> unInvoicedPickups = new List<Pickup>();
                foreach (Address address in user.Profile.Addresses)
                {
                    var trashCollection = GetTrashCollection(address);
                    if (trashCollection.Pickups !=null)
                    {
                        foreach (Pickup pickup in address.TrashCollection.Pickups)
                        {
                            if (!pickup.IsInvoiced)
                            {
                                unInvoicedPickups.Add(pickup);
                            }
                        }
                    }
                }
                if (unInvoicedPickups.Count > 0)
                {
                    Invoice invoice = new Invoice();
                    invoice.UserId = user.Id;
                    invoice.Pickups = new List<Pickup>();
                    invoice.IsPaid = false;
                    invoice.AmountDue = 0;
                    invoice.DateCreated = DateTime.Now;
                    invoice.DueDate = DateTime.Now.AddDays(30);
                    foreach (Pickup pickup in unInvoicedPickups)
                    {
                        pickup.IsInvoiced = true;
                        invoice.Pickups.Add(pickup);
                        invoice.AmountDue += pickup.Price;
                    }
                    db.Invoices.Add(invoice);
                    db.SaveChanges();
                }
                //var trashCollections = db.TrashCollections
                //    .Include(t => t.Pickups)
                //    .ToList();

                //List<Pickup> pickups = new List<Pickup>();
                //foreach (TrashCollection trashCollection in trashCollections)
                //{
                //    pickups = db.Pickups
                //        .Where(p => p.IsInvoiced == false)
                //        .ToList();
                //}
                //if ( pickups.Count > 0 )
                //{
                //    Invoice invoice = new Invoice();
                //    invoice.UserId = user.Id;
                //    invoice.Pickups = new List<Pickup>();
                //    invoice.IsPaid = false;
                //    invoice.AmountDue = 0;
                //    invoice.DateCreated = DateTime.Now;
                //    invoice.DueDate = DateTime.Now.AddDays(30);
                //    foreach (Pickup pickup in pickups)
                //    {
                //        pickup.IsInvoiced = true;
                //        invoice.Pickups.Add(pickup);
                //        invoice.AmountDue += pickup.Price;
                //    }
                //    db.Invoices.Add(invoice);
                //    db.SaveChanges();
                //}
            }

            return RedirectToAction("Index", "Invoice");
        }

        public ActionResult Pay(int invoiceId)
        {
            var invoice = db.Invoices.Where(i => i.InvoiceId == invoiceId).FirstOrDefault();
            return View(invoice);
        }

        public ActionResult MarkPaid(int invoiceId)
        {
            var invoice = db.Invoices.Where(i => i.InvoiceId == invoiceId).FirstOrDefault();
            invoice.IsPaid = true;
            invoice.AmountDue = 0;
            db.SaveChanges();

            return RedirectToAction("Index", "Invoice");
        }

        TrashCollection GetTrashCollection(Address address)
        {
            var trashCollection = db.TrashCollections.Include(t => t.Pickups).Where(t => t.TrashCollectionId == address.TrashCollectionId).FirstOrDefault();
            return trashCollection;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyEshop.Areas.Admin.Controllers
{
    public class DeliveryWaysController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        // GET: Admin/DeliveryWays
        public ActionResult Index()
        {
            return View(db.DeliveryWays.ToList());
        }

        // GET: Admin/DeliveryWays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryWays deliveryWays = db.DeliveryWays.Find(id);
            if (deliveryWays == null)
            {
                return HttpNotFound();
            }
            return View(deliveryWays);
        }

        // GET: Admin/DeliveryWays/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DeliveryWays/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeliveryID,Title,Price,Description,PayByCustomer,IsActive")] DeliveryWays deliveryWays)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryWays.Add(deliveryWays);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deliveryWays);
        }

        // GET: Admin/DeliveryWays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryWays deliveryWays = db.DeliveryWays.Find(id);
            if (deliveryWays == null)
            {
                return HttpNotFound();
            }
            return View(deliveryWays);
        }

        // POST: Admin/DeliveryWays/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeliveryID,Title,Price,Description,PayByCustomer,IsActive")] DeliveryWays deliveryWays)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryWays).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deliveryWays);
        }

        // GET: Admin/DeliveryWays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryWays deliveryWays = db.DeliveryWays.Find(id);
            if (deliveryWays == null)
            {
                return HttpNotFound();
            }
            return View(deliveryWays);
        }

        // POST: Admin/DeliveryWays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryWays deliveryWays = db.DeliveryWays.Find(id);
            db.DeliveryWays.Remove(deliveryWays);
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
    }
}

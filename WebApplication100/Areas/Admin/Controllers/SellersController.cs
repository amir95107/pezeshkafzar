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
    public class SellersController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        // GET: Admin/Sellers
        public ActionResult Index()
        {
            return View(db.Sellers.Where(s => s.IsAcceptedByAdmin).ToList());
        }

        // GET: Admin/Sellers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sellers sellers = db.Sellers.Find(id);
            if (sellers == null)
            {
                return HttpNotFound();
            }
            return View(sellers);
        }

        // GET: Admin/Sellers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sellers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SellerID,SellerFullName,StoreName,StoreAddress,Mobile,Telephone,Email,StartDate,IsActive")] Sellers sellers)
        {
            if (ModelState.IsValid)
            {
                sellers.StartDate = DateTime.Now;

                db.Sellers.Add(sellers);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sellers);
        }

        // GET: Admin/Sellers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sellers sellers = db.Sellers.Find(id);
            if (sellers == null)
            {
                return HttpNotFound();
            }
            return View(sellers);
        }

        // POST: Admin/Sellers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SellerID,SellerFullName,StoreName,StoreAddress,Mobile,Telephone,Email,StartDate,IsActive")] Sellers sellers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sellers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sellers);
        }

        // GET: Admin/Sellers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sellers sellers = db.Sellers.Find(id);
            if (sellers == null)
            {
                return HttpNotFound();
            }
            return View(sellers);
        }

        // POST: Admin/Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sellers sellers = db.Sellers.Find(id);
            db.Sellers.Remove(sellers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult NewSellers()
        {
            return View();
        }

        public ActionResult NewSellerPartial()
        {
            return PartialView(db.Sellers.Where(s => !s.IsAcceptedByAdmin).ToList());
        }

        public ActionResult AcceptSeller(int id)
        {
            var seller = db.Sellers.Find(id);
            if (seller == null)
            {
                return HttpNotFound();
            }
            Sellers sellers = new Sellers();
            seller.IsAcceptedByAdmin = true;
            seller.StartDate = DateTime.Now;
            db.SaveChanges();
            var isMobileConfirmed = seller.Users.IsMobileConfirmed;
            if (isMobileConfirmed)
            {
                string name = seller.SellerFullName != null ? seller.SellerFullName.Split(' ')[0]:"فروشنده";
                SendSMS.ConfirmSeller(seller.Users.Mobile,name);
            }
            ViewBag.Name = seller.SellerFullName;
            return PartialView("NewSellerPartial", db.Sellers.Where(s => !s.IsAcceptedByAdmin).ToList());
        }

        public int NumberOfSellerProducts(int sellerId)
        {
            Sellers seller = db.Sellers.Find(sellerId);
            int pCount = db.Products.Where(p => p.SellerID == seller.SellerID).Count();
            return pCount;
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

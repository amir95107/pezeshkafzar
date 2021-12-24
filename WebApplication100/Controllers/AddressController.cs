using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace WebApplication100.Controllers
{
    [Authorize]
    public class AddressController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        private int GetUserIdWithUserName()
        {
            var userName = User.Identity.Name;
            var id = db.Users.FirstOrDefault(u => u.Mobile == userName).UserID;
            return id;
        }
        // GET: Address
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddressesList()
        {
            var userId = GetUserIdWithUserName();
            var addresses = db.Addresses.Where(a => a.UserID == userId);
            if (addresses.Any()&&!addresses.Any(a => a.IsDefault))
            {
                var firstAddress = addresses.FirstOrDefault();
                firstAddress.IsDefault = true;
                db.Entry(firstAddress).State = EntityState.Modified;
                db.SaveChanges();
            }
            return PartialView(addresses.ToList());
        }

        // GET: Address/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return Redirect("/PageNotFound");
            }
            Addresses addresses = db.Addresses.Find(id);
            int userId = GetUserIdWithUserName();
            if (addresses == null || !db.Addresses.Where(a => a.UserID == userId).Any(ad => ad.AddressID == id))
            {
                return Redirect("/PageNotFound");
            }
            return View(addresses);
        }

        // GET: Address/Create
        public ActionResult Create()
        {
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AddressID,UserID,Address,Long,Lat,IsDefault,State,City,PostalCode")] Addresses addresses, string returnUrl)
        {
            
            if (ModelState.IsValid)
            {
                var userId = GetUserIdWithUserName();
                var address = db.Addresses;

                foreach (var item in db.Addresses.Where(u => u.UserID == userId))
                {
                    item.IsDefault = false;
                    db.Entry(item).State = EntityState.Modified;
                }
                addresses.IsDefault = true;
                addresses.UserID = userId;


                address.Add(addresses);
                db.SaveChanges();
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index");
            }

            TempData["AddressError"] = "یک یا چند فیلد نیاز به بازبینی مجدد دارند.";
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", addresses.UserID);
            return View(addresses);
        }

        public void SetDefaultAddress(int id)
        {
            var userId = GetUserIdWithUserName();
            foreach (var item in db.Addresses.Where(u => u.UserID == userId))
            {
                item.IsDefault = false;
                db.Entry(item).State = EntityState.Modified;
            }

            db.Addresses.Find(id).IsDefault = true;

            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult SetDefaultAddressWithReturn(int id)
        {
            var userId = GetUserIdWithUserName();
            foreach (var item in db.Addresses.Where(u => u.UserID == userId))
            {
                item.IsDefault = false;
                db.Entry(item).State = EntityState.Modified;
            }
            db.Addresses.Find(id).IsDefault = true;
            db.SaveChanges();
            return PartialView("AddressesList", db.Addresses.Where(a => a.UserID == userId).ToList());
        }

        // GET: Address/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return Redirect("/PageNotFound");
            }
            Addresses addresses = db.Addresses.Find(id);
            int userId = GetUserIdWithUserName();
            if (addresses == null || !db.Addresses.Where(a=>a.UserID == userId).Any(ad=>ad.AddressID==id))
            {
                return Redirect("/PageNotFound");
            }
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", addresses.UserID);
            return View(addresses);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AddressID,UserID,Address,Long,Lat,IsDefault,State,City,PostalCode")] Addresses addresses, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(addresses).State = EntityState.Modified;
                if (db.Addresses.Find(addresses.AddressID).IsDefault && addresses.IsDefault == false)
                {
                    addresses.IsDefault = true;
                }
                if (addresses.IsDefault)
                {
                    SetDefaultAddress(addresses.AddressID);
                }

                db.SaveChanges();
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index");
            }
            //ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", addresses.UserID);
            return View(addresses);
        }

        // GET: Address/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return Redirect("/PageNotFound");
            }
            Addresses addresses = db.Addresses.Find(id);
            int userId = GetUserIdWithUserName();
            if (addresses == null || !db.Addresses.Where(a => a.UserID == userId).Any(ad => ad.AddressID == id))
            {
                return Redirect("/PageNotFound");
            }
            return View(addresses);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var userId = GetUserIdWithUserName();
            var addresses = db.Addresses;
            Addresses address = addresses.Find(id);
            db.Addresses.Remove(address);
            db.SaveChanges();
            if (!addresses.Where(a => a.UserID == userId).Any(a => a.IsDefault))
            {
                var firstAddress = addresses.FirstOrDefault();
                firstAddress.IsDefault = true;
                db.Entry(firstAddress).State = EntityState.Modified;
                db.SaveChanges();
            }
            db.SaveChanges();
            return PartialView("AddressesList", db.Addresses.Where(a => a.UserID == userId).ToList());
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

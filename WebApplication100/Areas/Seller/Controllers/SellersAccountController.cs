using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;

namespace MyEshop.Areas.Seller.Controllers
{
    public class SellersAccountController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        public int SellerId()
        {
            string user = User.Identity.Name;
            int sellerId = db.Sellers.Single(s => s.Users.Mobile == user).SellerID;
            return sellerId;
        }
        // GET: Seller/SellersAccount
        public ActionResult Index()
        {
            var sellerId = SellerId();
            return View(db.Sellers.Find(sellerId));
        }

        // GET: Seller/SellersAccount/Details/5
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

        // GET: Seller/SellersAccount/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id != SellerId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sellers sellers = db.Sellers.Find(id);
            if (sellers == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", sellers.UserID);
            return View(sellers);
        }

        // POST: Seller/SellersAccount/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SellerID,SellerFullName,StoreName,StoreAddress,Telephone,StartDate,IsAcceptedByAdmin,Lat,Long,Point,UserID,State,City,Email")] Sellers sellers, string Email)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(sellers.UserID);
                user.Email = Email;
                db.Entry(user).State = EntityState.Modified;
                db.Entry(sellers).State = EntityState.Modified;
                if (sellers.StoreAddress != null && sellers.City != null && sellers.State != null && sellers.SellerFullName != null && sellers.Telephone != null)
                {
                    TempData["SuccessEditInfo"] = "اطلاعات شما با موفقیت بروزرسانی شد. لطفا جهت بررسی توسط ادمین صبر  کنید.";
                    SendEmail.Send("a.janmohammadi@gmail.com", "", "تکمیل اطلاعات فروشنده", $"اطلاعات فروشگاه {sellers.StoreName} تکمیل شد.");
                }
                else
                {
                    TempData["WarningEditInfo"] = "بعضی از اطلاعات شما بدرستی تکمیل نشده است.";
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", sellers.UserID);
            return View(sellers);
        }

        // GET: Seller/SellersAccount/Delete/5
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

        // POST: Seller/SellersAccount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sellers sellers = db.Sellers.Find(id);
            db.Sellers.Remove(sellers);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public bool ActivateEmail()
        {
            var user = db.Users.FirstOrDefault(u => u.Mobile == User.Identity.Name);
            if (user.Email == null)
            {
                return false;
            }
            if (!user.IsActive)
            {
                string body = PartialToStringClass.RenderPartialView("ManageEmails", "SellerActivationEmail", user);
                SendEmail.Send(user.Email, "", "ایمیل فعالسازی", body);
                return true;
            }
            return false;
        }

        public ActionResult ActiviationEmail()
        {
            return PartialView();
        }


        public ActionResult ActiveSellersEmail(string activeCode)
        {
            if (activeCode == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.FirstOrDefault(u => u.ActiveCode == activeCode);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            TempData["SuccessEmailActive"] = "ایمیل شما با موفقیت تایید شد.";
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

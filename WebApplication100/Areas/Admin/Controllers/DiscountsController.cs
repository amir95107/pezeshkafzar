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
    public class DiscountsController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        // GET: Admin/Discounts
        public ActionResult Index()
        {
            var discounts = db.Discounts.Include(d => d.Users);
            return View(discounts.ToList());
        }

        // GET: Admin/Discounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discounts discounts = db.Discounts.Find(id);
            if (discounts == null)
            {
                return HttpNotFound();
            }
            return View(discounts);
        }

        // GET: Admin/Discounts/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users.Where(u=>u.RoleID==1), "UserID", "Mobile");
            return View();
        }

        // POST: Admin/Discounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiscountID,UserID,DiscountCode,ExpireDate,DiscountPercent,IsUsed,MaxDiscountValue")] Discounts discounts)
        {
            if (ModelState.IsValid)
            {
                db.Discounts.Add(discounts);
                db.SaveChanges();
                var user = db.Users.Find(discounts.UserID);
                if (user != null)
                {
                    try
                    {
                        if (user.IsMobileConfirmed)
                        {
                            var name = user.UserInfo.FirstOrDefault(u=>u.UserID==user.UserID).FullName.Split(' ')[0];
                            SendSMS.SendDiscountMessage(user.Mobile,name,discounts.DiscountCode,discounts.MaxDiscountValue.ToString(),discounts.ExpireDate.ToString("yyyy/MM/dd"));
                        }
                    }
                    catch
                    {
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.UserID = new SelectList(db.Users, "UserID", "Mobile", discounts.UserID);
                    return View(discounts);
                }
            }

            ViewBag.UserID = new SelectList(db.Users, "UserID", "Mobile", discounts.UserID);
            return View(discounts);
        }        

        // GET: Admin/Discounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discounts discounts = db.Discounts.Find(id);
            if (discounts == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Mobile", discounts.UserID);
            return View(discounts);
        }

        // POST: Admin/Discounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiscountID,UserID,DiscountCode,ExpireDate,DiscountPercent,MaxDiscountValue")] Discounts discounts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discounts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Mobile", discounts.UserID);
            return View(discounts);
        }

        // GET: Admin/Discounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discounts discounts = db.Discounts.Find(id);
            if (discounts == null)
            {
                return HttpNotFound();
            }
            return View(discounts);
        }

        // POST: Admin/Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discounts discounts = db.Discounts.Find(id);
            db.Discounts.Remove(discounts);
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

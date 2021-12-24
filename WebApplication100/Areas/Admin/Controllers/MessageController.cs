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
    public class MessageController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        // GET: Admin/Message
        public ActionResult Index()
        {
            var contactForm = db.ContactForm.Include(c => c.Users);
            return View(contactForm.ToList());
        }

        // GET: Admin/Message/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactForm contactForm = db.ContactForm.Find(id);
            if (contactForm == null)
            {
                return HttpNotFound();
            }
            return View(contactForm);
        }

        // GET: Admin/Message/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: Admin/Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserID,Date,Subject,Text,Name,Mobile,Email,IsFaq,Answer")] ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                contactForm.Date = DateTime.Now;
                contactForm.Subject = "سوال";
                contactForm.Name = User.Identity.Name;
                contactForm.Mobile = "09120624426";
                contactForm.IsFaq = true;
                contactForm.UserID = 1;
                db.ContactForm.Add(contactForm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contactForm);
        }

        // GET: Admin/Message/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactForm contactForm = db.ContactForm.Find(id);
            if (contactForm == null)
            {
                return HttpNotFound();
            }
            return View(contactForm);
        }

        // POST: Admin/Message/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserID,Date,Subject,Text,Name,Mobile,Email,IsFaq,Answer")] ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
                if (contactForm.Answer != null && contactForm.Answer!=""&& contactForm.Answer!=" ")
                {
                    SendEmail.Send(contactForm.Email,"a.janmohammadi@gmail.com","پاسخ به پیام "+ contactForm.Subject,contactForm.Answer);
                }
                db.Entry(contactForm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contactForm);
        }

        // GET: Admin/Message/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactForm contactForm = db.ContactForm.Find(id);
            if (contactForm == null)
            {
                return HttpNotFound();
            }
            return View(contactForm);
        }

        // POST: Admin/Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactForm contactForm = db.ContactForm.Find(id);
            db.ContactForm.Remove(contactForm);
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

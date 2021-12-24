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
    [Authorize(Roles="Admin")]
    public class CommentsController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        // GET: Admin/Comments
        public ActionResult Index()
        {
            var comments = db.Comments.Include(c => c.Blogs).Include(c => c.Products).OrderByDescending(c => c.CreateDate).Where(c => c.Name != "ادمین");
            return View(comments.ToList());
        }

        // GET: Admin/Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // GET: Admin/Comments/Create
        //public ActionResult Create()
        //{
        //    ViewBag.BlogID = new SelectList(db.Blogs, "BlogID", "Title");
        //    ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Title");
        //    return View();
        //}

        public ActionResult Create(int? id)
        {
            ViewBag.BlogID = new SelectList(db.Blogs, "BlogID", "Title");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Title");
            if (id != null)
            {
                ViewBag.ParentID = id;
                ViewBag.ParentComment = db.Comments.FirstOrDefault(c => c.CommentID == id).Comment;
            }
            return View();
        }

        // POST: Admin/Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CommentID,ProductID,Name,Comment,CreateDate,ParentID,IsShow,BlogID")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                comments.CreateDate = DateTime.Now;
                comments.Name = "ادمین";
                db.Comments.Add(comments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BlogID = new SelectList(db.Blogs, "BlogID", "Title", comments.BlogID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Title", comments.ProductID);
            return View(comments);
        }

        // GET: Admin/Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            ViewBag.BlogID = new SelectList(db.Blogs, "BlogID", "Title", comments.BlogID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Title", comments.ProductID);
            ViewBag.CommentID = new SelectList(db.Products, "CommentID", "Title", comments.CommentID);
            return View(comments);
        }

        // POST: Admin/Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CommentID,ProductID,Name,Comment,CreateDate,ParentID,IsShow,BlogID")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BlogID = new SelectList(db.Blogs, "BlogID", "Title", comments.BlogID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "Title", comments.ProductID);
            return View(comments);
        }

        // GET: Admin/Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comments comments = db.Comments.Find(id);
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View(comments);
        }

        // POST: Admin/Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comments comments = db.Comments.Find(id);
            db.Comments.Remove(comments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowInSite(int id,bool isShow)
        {
            Comments comments = db.Comments.Find(id);
            comments.IsShow = isShow;
            db.Entry(comments).State = EntityState.Modified;
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

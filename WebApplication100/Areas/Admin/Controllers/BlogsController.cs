using DataLayer;
using InsertShowImage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication100.Utilities;

namespace MyEshop.Areas.Admin.Controllers
{
    public class BlogsController : Controller
    {
        // GET: Admin/Blogs

        DataLayer.medab_DBEntities db = new DataLayer.medab_DBEntities();

        public ActionResult Index()
        {
            return View(db.Blogs.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BlogID,Title,ShortDescription,Text,Visit,ImageName,CreateDate,Tags,Author,SefUrl")] Blogs blog, HttpPostedFileBase imageName)
        {
            if (ModelState.IsValid)
            {
                blog.Visit = 0;
                blog.CreateDate = DateTime.Now;
                blog.Author = User.Identity.Name;

                if (imageName != null)
                {
                    blog.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageName.FileName);
                    imageName.SaveAs(Server.MapPath("/Images/BlogImages/" + blog.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/BlogImages/" + blog.ImageName),
                        Server.MapPath("/Images/BlogImages/Thumb/" + blog.ImageName));
                }
                if (blog.SefUrl == null || blog.SefUrl == "")
                {
                    blog.SefUrl = blog.Title.Replace(" ", "-");
                }
                db.Blogs.Add(blog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blog);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Blogs blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return HttpNotFound();
            }
            return View(blog);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BlogID,Title,ShortDescription,Text,Visit,ImageName,CreateDate,Tags,Author,SefUrl")] Blogs blog, HttpPostedFileBase imageName)
        {
            if (ModelState.IsValid)
            {
                if (imageName != null)
                {
                    if (blog.ImageName != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/BlogImages/" + blog.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/BlogImages/Thumb/" + blog.ImageName));
                    }

                    blog.ImageName = Guid.NewGuid() + Path.GetExtension(imageName.FileName);
                    imageName.SaveAs(Server.MapPath("/Images/BlogImages/" + blog.ImageName));
                    ImageResizer_330 img = new ImageResizer_330();
                    img.Resize(Server.MapPath("/Images/BlogImages/" + blog.ImageName),
                        Server.MapPath("/Images/BlogImages/Thumb/" + blog.ImageName));
                }
                if (blog.SefUrl == null || blog.SefUrl == "")
                {
                    blog.SefUrl = blog.Title.Replace(" ", "-");
                }

                db.Entry(blog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blog);
        }

        public ActionResult Delete(int id)
        {
            var blog = db.Blogs.Find(id);
            db.Blogs.Remove(blog);
            db.SaveChanges();
            return View("index", db.Blogs.ToList());
        }

    }
}
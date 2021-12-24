using DataLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Controllers
{
    public class BlogController : Controller
    {
        medab_DBEntities db = new medab_DBEntities();
        // GET: Blog
        [Route("Blog/All")]
        public ActionResult AllBlogs(int pageId = 1)
        {
            var blogs = db.Blogs;
            int take = 6;
            int skip = (pageId - 1) * take;
            var pageCount = blogs.Count() / take + 1;
            if (blogs.Count() % take == 0)
            {
                pageCount--;
            }
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = pageId;
            
            return View(blogs.OrderByDescending(b => b.CreateDate).Skip(skip).Take(take).ToList());
        }

        public ActionResult LastBlogs()
        {
            var lb = db.Blogs.OrderByDescending(b => b.CreateDate).Take(3);
            return PartialView(lb);
        }

        [Route("Blog/{id}/{SefUrl}")]
        public ActionResult ShowBlog(int id,string SefUrl)
        {
            var blog = db.Blogs.Find(id);
            if (blog != null)
            {
                blog.Visit++;

                try
                {
                    db.Entry(blog).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch { }

                ViewBag.Title = blog.Title;
                ViewBag.Description = blog.ShortDescription;
                ViewBag.Top = db.Blogs.OrderByDescending(b => b.Visit).Take(3).ToList();
                return View(blog);
            }
            else
            {
                return Redirect("/Error/Not-Found?aspxerrorpath=Blog/" + id + "/" + SefUrl);
                //return HttpNotFound();
            }
        }

        [Route("Blog/ShowBlog/{id}")]
        public ActionResult RedirectBlog(int id)
        {
            var blog = db.Blogs.Find(id);
            return RedirectPermanent("/Blog/"+id+"/"+blog.SefUrl);
        }

        [Route("Blog/AllBlogs")]
        public ActionResult RedirectBlog2()
        {
            return RedirectPermanent("/Blog/All");
        }


        [Route("Blog/Search")]
        public ActionResult Search(string q, int pageId = 1)
        {
            var blogs = db.Blogs;
            int take = 6;
            int skip = (pageId - 1) * take;
            var pageCount = blogs.Count() / take + 1;
            if (blogs.Count() % take == 0)
            {
                pageCount--;
            }
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = pageId;
            //var blogPage = db.PageData.Find(6);
            ViewBag.Search = q;
            return View(blogs.Where(p => p.Title.Contains(q) || p.ShortDescription.Contains(q) || p.Tags.Contains(q) || p.Text.Contains(q)).Distinct());
        }
    }
}
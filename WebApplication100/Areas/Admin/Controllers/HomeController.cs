using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LastProductsList()
        {
            var products = db.Products.OrderByDescending(p => p.CreateDate).Take(6).ToList();
            return PartialView(products);
        }

        public ActionResult Order()
        {
            return PartialView(db.Orders.ToList());
        }

        public ActionResult Media()
        {
            //var images = db.Products.Select(p=>p.ImageName).ToList();
            //ViewBag.Images = images;
            return View();
        }
        public ActionResult MediaPartial()
        {
            var images = db.Products.Select(p => p.ImageName).Distinct().ToList();
            ViewBag.Images = images;
            return PartialView();
        }
    }
}
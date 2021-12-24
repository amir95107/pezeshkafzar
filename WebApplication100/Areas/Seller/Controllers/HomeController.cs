using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Areas.Seller.Controllers
{
    public class HomeController : Controller
    {

        private medab_DBEntities db = new medab_DBEntities();
        // GET: Admin/Home

        public int SellerId()
        {
            string user = User.Identity.Name;
            int sellerId = db.Sellers.Single(s => s.Users.Mobile == user).SellerID;
            return sellerId;
        }
        public bool IsAccepted()
        {
            int sellerId = SellerId();
            bool isAccepted = false;
            if (db.Sellers.Find(sellerId).IsAcceptedByAdmin)
            {
                isAccepted = true;
            }
            return isAccepted;
        }
        public ActionResult Index()
        {
            if (!IsAccepted())
            {
                return RedirectToAction("Index","SellersAccount");
            }
            return View();
        }

        public ActionResult SideMenu()
        {
            bool isAccepted = false;
            int sellerId = SellerId();
            if (db.Sellers.Find(sellerId).IsAcceptedByAdmin)
            {
                isAccepted = true;
            }
            ViewBag.IsAccepted = isAccepted;
            return PartialView();
        }

        public ActionResult LastProductsList()
        {
            int sellerId = SellerId();
            var products = db.Products.Where(p => p.SellerID == sellerId).OrderByDescending(p => p.CreateDate).Take(6).ToList();
            return PartialView(products);
        }

        public ActionResult Order()
        {
            int sellerId = SellerId();
            return PartialView(db.OrderDetails.Where(o => o.Products.SellerID == sellerId && o.Orders.IsFinaly == true).ToList());
        }
    }
}
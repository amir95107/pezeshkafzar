using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls.WebParts;
using DataLayer;

namespace MyEshop.Areas.Seller.Controllers
{
    public class OrdersController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        public int SellerId()
        {
            string user = User.Identity.Name;
            int sellerId = db.Sellers.Single(s => s.Users.Mobile == user).SellerID;
            return sellerId;
        }
        // GET: Admin/Orders
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowDetails(int id)
        {
            ViewBag.isFinally = db.Orders.Find(id).IsFinaly;
            var orderDetails = db.OrderDetails.Where(o => o.OrderID == id).ToList();
            return PartialView(orderDetails);
        }

        public ActionResult OrderTable()
        {
            int sellerId = SellerId();
            var orders = db.OrderDetails.Where(o=>o.Products.SellerID == sellerId);
            return PartialView(orders.ToList());
        }

        

        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult ReportOrders(int? id)
        {
            var orders = db.OrderDetails.ToList();
            if (id != null)
            {

                orders = db.OrderDetails.Where(o => o.Products.SellerID == id).ToList();
            }
            
            return PartialView(orders);
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

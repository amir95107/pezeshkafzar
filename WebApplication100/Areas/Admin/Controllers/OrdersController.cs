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
using DataLayer.ViewModels;

namespace MyEshop.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            ViewBag.Sellers = db.Sellers.ToList();
            return View();
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "UserID", "Mobile");
            ViewBag.Products = db.Products.Where(p => p.Stock > 0 && p.IsActive && p.IsAcceptedByAdmin && p.PriceAfterDiscount > 0).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,UserID,Date,Payable,IsFinaly,PaymentWay,UseDiscountCode,IsSent,DeliveryID,DeliveryPrice")] Orders order)
        {
            List<ShowOrderViewModel> list = getListOrder();

            Random rnd = new Random();
            var newRnd = rnd.Next(10000, 100000);
            var userId = order.UserID;
            var userName = db.Users.Find(userId).Mobile;

            order.Date = DateTime.Now;
            order.Payable = list.Sum(l => l.PriceAfterDiscount * l.Count);
            order.IsFinaly = false;
            order.PaymentWay = 2;
            order.UseDiscountCode = false;
            order.IsFinaly = false;
            order.DeliveryID = 7;
            order.TraceCode = (userId + userName.Substring(9) + newRnd).ToString();

            db.Orders.Add(order);

            foreach (var item in list)
            {
                db.OrderDetails.Add(new OrderDetails()
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    Count = item.Count,
                    Price = item.PriceAfterDiscount,
                    Condition = 2,
                    TotalDiscount = item.Price - item.PriceAfterDiscount
                }
                );

                db.Products.Find(item.ProductID).Stock -= item.Count;
            }

            db.SaveChanges();
            Session["AdminShopCart"] = null;


            return RedirectToAction("Index");
        }


        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", orders.UserID);
            return View(orders);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserID,Date,Payable,IsFinaly,PaymentWay,UseDiscountCode")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName", orders.UserID);
            return View(orders);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return PartialView(orders);
        }

        // POST: Admin/Orders/Delete/5
        public ActionResult DeleteConfirmed(int id)
        {
            var od = db.OrderDetails.Where(o => o.OrderID == id);
            foreach (var item in od)
            {
                db.OrderDetails.Remove(item);
            }
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ShowDetails(int id)
        {
            ViewBag.isFinally = db.Orders.Find(id).IsFinaly;
            var orderDetails = db.OrderDetails.Where(o => o.OrderID == id).ToList();
            return PartialView(orderDetails);
        }

        public ActionResult ShowOredrDetails(int id)
        {
            ViewBag.isFinally = db.Orders.Find(id).IsFinaly;
            var orderDetails = db.OrderDetails.Where(o => o.OrderID == id).ToList();
            return PartialView(orderDetails);
        }

        public ActionResult OrderTable()
        {
            var orders = db.Orders;
            return PartialView(orders.OrderByDescending(o=>o.Date).ToList());
        }

        public ActionResult FinalizeOrder(int id)
        {
            Orders order = db.Orders.Find(id);
            order.IsFinaly = true;
            db.Entry(order).State = EntityState.Modified;
            var od = db.OrderDetails.Where(o => o.OrderID == id);
            foreach (var item in od)
            {
                db.Products.Find(item.ProductID).Stock -= item.Count;
                db.Entry(item).State = EntityState.Modified;
            }

            db.SaveChanges();
            return PartialView("OrderTable", db.Orders.ToList());
        }

        public ActionResult Reports()
        {
            ViewBag.Sellers = db.Sellers.ToList();
            return View();
        }

        public ActionResult ReportOrders(int? id)
        {
            var orders = db.Orders.ToList();
            if (id != null)
            {
                orders = db.OrderDetails.Where(o => o.Products.SellerID == id).Select(o => o.Orders).ToList();
            }

            return PartialView(orders);
        }

        public void DeleteOrders2(int id)
        {
            var od = db.OrderDetails.Where(o => o.OrderID == id);
            foreach (var item in od)
            {
                db.OrderDetails.Remove(item);
            }
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
        }

        public ActionResult DeleteExpiredOrders(int hour)
        {
            var now_2 = DateTime.Now.AddHours(hour);
            List<Orders> orders = db.Orders.Where(o => now_2 < o.Date && o.IsFinaly == false).ToList();
            if (orders.Any())
            {
                foreach (var order in orders)
                {
                    DeleteOrders2(order.OrderID);
                }
                db.SaveChanges();
            }
            return PartialView("OrderTable", db.Orders.ToList());
        }


        public ActionResult SetOrderDetailsCondition(int id, int num, int orderId)
        {
            var od = db.OrderDetails.Find(id);
            od.Condition = num;
            db.Entry(od).State = EntityState.Modified;
            db.SaveChanges();

            SetOrderIsSent(orderId);

            return PartialView("OrderTable", db.Orders.ToList());
        }

        public ActionResult SetOrderIsSent(int id)
        {
            var order = db.Orders.Find(id);
            int sum = 0;
            foreach (var item in order.OrderDetails)
            {
                if (item.Condition == 1)
                {
                    sum++;
                }
            }
            if (sum == order.OrderDetails.Count())
            {
                order.IsSent = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView("OrderTable", db.Orders.ToList());
        }

        public ActionResult LoggedCarts(string range)
        {
            List<Users> users = db.Users.ToList();
            ViewBag.Users = users;
            var logged = db.LoggedCart;
            var time = DateTime.Now;
            if (range == "day")
            {
                time = DateTime.Now.AddDays(-1);
            }
            else if (range == "week")
            {
                time = DateTime.Now.AddDays(-7);
            }
            else if (range == "month")
            {
                time = DateTime.Now.AddMonths(-1);
            }
            else
            {
                time = DateTime.Now.AddYears(-100);
            }
            if (range == null) range = "";
            ViewBag.Range = range;

            return View(logged.Where(l => l.Date > time && !l.Url.Contains("localhost") && l.UserName != "admin").ToList());
        }

        public void removeLoggs()
        {
            var logs = db.LoggedCart.ToList();
            foreach (var log in logs)
            {
                db.LoggedCart.Remove(log);
            }
            db.SaveChanges();
        }

        public List<ShowOrderViewModel> getListOrder()
        {
            List<ShowOrderViewModel> list = new List<ShowOrderViewModel>();

            if (Session["AdminShopCart"] != null)
            {
                List<ShopCartItem> listShop = Session["AdminShopCart"] as List<ShopCartItem>;
                foreach (var item in listShop)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.Title,
                        p.Price,
                        p.PriceAfterDiscount,
                        p.SefUrl
                    }).Single();
                    list.Add(new ShowOrderViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        Price = product.Price,
                        PriceAfterDiscount = product.PriceAfterDiscount,
                        Discount = item.Count * product.Price - item.Count * product.PriceAfterDiscount,
                        ImageName = product.ImageName,
                        Title = product.Title,
                        Sum = item.Count * product.Price,
                        SefUrl = product.SefUrl
                    });
                }
            }
            return list;
        }

        public ActionResult AdminOrderList()
        {
            List<Products> products = db.Products.Where(p => p.Stock > 0 && p.IsActive && p.IsAcceptedByAdmin && p.PriceAfterDiscount > 0).ToList();
            ViewBag.Products = products;
            List<ShopCartItem> listShop = Session["AdminShopCart"] as List<ShopCartItem>;
            return PartialView(listShop);
        }

        public void CommandOrder(int id, int count)
        {
            List<ShopCartItem> listShop = Session["AdminShopCart"] as List<ShopCartItem>;
            int index = listShop.FindIndex(p => p.ProductID == id);
            if (count == 0)
            {
                listShop.RemoveAt(index);
            }
            else
            {
                listShop[index].Count = count;
            }
            Session["AdminShopCart"] = listShop;
        }


        [HttpPost]
        public bool UserInformation(int userId, string fullName, string telephone)
        {
            var user = db.Users.Find(userId);
            if (user != null)
            {
                UserInfo userInfo1 = new UserInfo()
                {
                    UserID = userId,
                    FullName = fullName,
                    Telephone = telephone,
                };
                db.UserInfo.Add(userInfo1);
                user.IsUserInfoCompleted = true;
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public void AddAddress(int userId,string address)
        {
            db.Addresses.Add(new Addresses() { 
                UserID=userId,
                City="تهران",
                State="تهران",
                Address=address,
                IsDefault=true,
                PostalCode="1234"
            });
            db.SaveChanges();
        }

        public bool IsUserInfoComplete(int id) {
            var user = db.Users.Find(id);
            if (user.IsUserInfoCompleted)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasAddress(int id)
        {
            var hasAddress = db.Addresses.Any(a=>a.UserID==id);
            return hasAddress;
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

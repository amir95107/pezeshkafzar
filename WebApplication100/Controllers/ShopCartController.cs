using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.UI;
using System.Security.Permissions;
using DataLayer;
using System.Web.Script.Serialization;
using Antlr.Runtime;
using System.Configuration;
using System.Data.Entity;

namespace MyEshop.Controllers
{
    public class ShopCartController : Controller
    {
        medab_DBEntities db = new medab_DBEntities();
        // GET: ShopCart

        [Authorize]
        public int GetUserIdWithUserName()
        {
            var userName = User.Identity.Name;
            var user = db.Users.SingleOrDefault(u => u.Mobile == userName);
            return user.UserID;
        }

        public ActionResult ShowCart()
        {
            List<ShopCartItemViewModel> list = new List<ShopCartItemViewModel>();

            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> listShop = (List<ShopCartItem>)Session["ShopCart"];

                foreach (var item in listShop)
                {
                    var product = db.Products.Where(p => p.ProductID == item.ProductID).Select(p => new
                    {
                        p.ImageName,
                        p.PriceAfterDiscount,
                        p.Title,
                        p.SefUrl
                    }).Single();
                    list.Add(new ShopCartItemViewModel()
                    {
                        Count = item.Count,
                        ProductID = item.ProductID,
                        Title = product.Title,
                        ImageName = product.ImageName,
                        PriceAfterDiscount = product.PriceAfterDiscount,
                        SefUrl = product.SefUrl
                    });
                }
            }

            return PartialView(list);
        }

        public ActionResult Index()
        {

            return View();
        }

        public List<ShowOrderViewModel> getListOrder()
        {
            List<ShowOrderViewModel> list = new List<ShowOrderViewModel>();

            if (Session["ShopCart"] != null)
            {
                List<ShopCartItem> listShop = Session["ShopCart"] as List<ShopCartItem>;
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

        public ActionResult OrderingLevel()
        {
            bool shopCart = false, shipping = false, userInfo = false, factor = false, payment = false;
            if (Session["ShopCart"] != null)
            {
                shopCart = true;
                if (User.Identity.IsAuthenticated)
                {
                    int userId = GetUserIdWithUserName();
                    if (db.Users.Find(userId).IsUserInfoCompleted)
                    {
                        userInfo = true;
                        if (Session["Shipping"] != null)
                        {
                            shipping = true;
                            if (Request.Url.AbsolutePath == "/ShopCart/Factor")
                            {
                                factor = true;
                            }
                            if (Request.Url.AbsolutePath.Contains("Pay"))
                            {
                                factor = true;
                                payment = true;
                            }
                        }
                    }
                }

            }
            bool[] list = new bool[5] { shopCart, userInfo, shipping, factor, payment };
            string[] _list = new string[5] { "سبد خرید", "تکمیل اطلاعات", "شیوه ارسال", "فاکتور", "پرداخت" };
            string[] url = new string[5] { "/ShopCart", "/Account/UserInformation", "/ShopCart/Delivery", "/ShopCart/Factor", "#" };
            ViewBag.Level = list;
            ViewBag.ListName = _list;
            ViewBag.Url = url;
            return PartialView();
        }

        public ActionResult Order()
        {
            if (Request.QueryString["noShippingWaySelected"] != null)
            {
                ViewBag.NoShippingWaySelected = "لطفا یکی از روشهای ارسال را انتخاب کنید";
            }
            ViewBag.shipping = Session["Shipping"];
            ViewBag.DiscountPercent = Session["Discount"];
            List<DeliveryWays> dList = db.DeliveryWays.ToList();
            ViewBag.DList = dList;
            return PartialView(getListOrder());
        }

        public ActionResult CommandOrder(int id, int count)
        {
            List<DataLayer.ViewModels.ShopCartItem> listShop = Session["ShopCart"] as List<DataLayer.ViewModels.ShopCartItem>;
            int index = listShop.FindIndex(p => p.ProductID == id);
            if (count == 0)
            {
                listShop.RemoveAt(index);
            }
            else
            {
                listShop[index].Count = count;
            }
            Session["ShopCart"] = listShop;

            return PartialView("Order", getListOrder());
        }

        [Authorize]
        public ActionResult Factor()
        {
            if (Session["ShopCart"] == null)
            {
                return RedirectToAction("Index");
            }
            DeliveryWays Shipping = Session["Shipping"] as DeliveryWays;


            //if (discount == null) discount = 0;
            if (Shipping != null)
            {
                var discount = Session["Discount"] as Discounts;
                int shipping = int.Parse(Shipping.Price.ToString());
                string user = User.Identity.Name;
                var isComplete = db.Users.Single(u => u.Mobile == user).IsUserInfoCompleted;
                ViewBag.shipping = Shipping;
                ViewBag.DiscountPercent = (discount == null) ? 0 : discount.DiscountPercent;
                if (isComplete)
                {
                    var userId = GetUserIdWithUserName();
                    var address = db.Addresses.Where(a => a.UserID == userId);
                    if (address.Any(a => a.IsDefault))
                    {

                        var listDetails = getListOrder();
                        int payable = listDetails.Sum(l => l.Sum) - listDetails.Sum(l => l.Discount);
                        int decreaseFromPayable = (discount == null) ? 0 : Math.Min(discount.DiscountPercent * payable / 100, discount.MaxDiscountValue);
                        int payableAfterDiscountCode = payable - decreaseFromPayable;
                        bool isUsed = false;
                        if (Session["UseDiscount"] != null && bool.Parse(Session["UseDiscount"].ToString()) == true)
                        {
                            isUsed = true;
                        }

                        Orders order = new Orders()
                        {
                            UserID = userId,
                            Date = DateTime.Now,
                            Payable = payableAfterDiscountCode + shipping,
                            IsFinaly = false,
                            PaymentWay = 2,
                            UseDiscountCode = isUsed,
                            TraceCode = DateTime.Now.ToString("hhMMss") + User.Identity.Name.Substring(9) + userId.ToString(),
                            IsSent = false,
                            DeliveryID = db.DeliveryWays.FirstOrDefault(d => d.Price == shipping).DeliveryID,
                            DeliveryPrice = shipping
                        };
                        db.Orders.Add(order);
                        foreach (var item in listDetails)
                        {
                            db.OrderDetails.Add(new OrderDetails()
                            {
                                Count = item.Count,
                                ProductID = item.ProductID,
                                Price = item.PriceAfterDiscount,
                                OrderID = order.OrderID,
                                TotalDiscount = (item.Price - item.PriceAfterDiscount) * item.Count,
                                Condition = 2
                            });
                        }


                        db.SaveChanges();
                        ViewBag.OrderId = order.OrderID;
                        ViewBag.DefaultAddress = address.FirstOrDefault(a => a.IsDefault).Address.ToString();

                        string body = $"شخصی با شماره {order.Users.Mobile} یک سفارش به مبلغ  {order.Payable} تومن، ثبت کرده.";
                        try
                        {
                            if (order.Users.Mobile != "09120624426")
                            {
                                SendEmail.Send("a.janmohammadi@gmail.com", "", "سفارش جدید", body);
                            }
                        }
                        catch { }

                        return View(getListOrder());
                    }
                    else
                    {
                        return Redirect("/ShopCart/Delivery");
                    }
                }
                else
                {
                    return RedirectToAction("UserInformation", "Account", new { area = "" });
                }
            }
            else
            {

                return RedirectToAction("Delivery", db.Addresses.ToList());
            }
        }

        [Authorize]
        public ActionResult Delivery()
        {
            if (Session["ShopCart"] == null || Session["ShopCart"].ToString() == "")
            {
                return RedirectToAction("Index");
            }
            else
            {
                var userId = GetUserIdWithUserName();
                if (db.Users.Find(userId).IsUserInfoCompleted)
                {
                    var addresses = db.Addresses.Where(a => a.UserID == userId).ToList();

                    if (Session["Shipping"] == null || Session["Shipping"].ToString() == "" || !addresses.Any(a => a.IsDefault))
                    {
                        ViewBag.EmptyShipping = "لطفا یکی از شیوه های ارسال را انتخاب کنید";
                    }
                    var deliveryWays = db.DeliveryWays.OrderBy(d => d.Price).ToList();
                    var pay = Payable();
                    int payable = pay[0];
                    bool moreThanMil = false;
                    if (payable > 1000000)
                    {
                        moreThanMil = true;
                    }
                    ViewBag.MoreThanMillion = moreThanMil;
                    ViewBag.EmptyShipping = "لطفا یکی از شیوه های ارسال را انتخاب کنید";
                    ViewBag.DeliveryWays = deliveryWays;
                    return View(addresses);
                    //else
                    //{
                    //    return RedirectToAction("Factor");
                    //}
                }
                else
                {
                    return Redirect("/Account/UserInformation?ReturnUrl=/ShopCart/Delivery");
                }
            }
        }

        public string ShowDeliveryDescription(int id)
        {
            string desc = db.DeliveryWays.Find(id).Description;
            return desc;
        }

        public int[] Payable()
        {
            List<ShowOrderViewModel> list = getListOrder();
            int discount = 0;
            if (Session["Discount"] != null)
            {
                discount = int.Parse(discount.ToString());
            }
            DeliveryWays ship = Session["Shipping"] as DeliveryWays;
            var totalDiscount = list.Sum(l => l.Price - l.PriceAfterDiscount) + discount;

            var sum = (int)(((100 - discount) / 100) * list.Sum(l => l.PriceAfterDiscount) + (ship != null ? ship.Price : 0));
            int[] arr = new int[] { sum, discount };
            return arr;
        }


        public ActionResult PayWithCardNo(int id)
        {
            Orders order = db.Orders.Find(id);
            int[] arr = Payable();
            if (!order.IsFinaly)
            {

                order.PaymentWay = 2;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                ViewBag.IsFinally = false;
                string body = $"شخصی با شماره {order.Users.Mobile} یک سفارش به مبلغ  {order.Payable} تومن، به روش کارت به کارت، ثبت کرده.";
                SendEmail.Send("a.janmohammadi@gmail.com", "", "سفارش جدید", body);
            }
            else
            {
                ViewBag.IsFinally = true;
            }
            ViewBag.Data = arr;
            return View();
        }

        [Authorize]
        [HttpPost]
        public dynamic DiscountCodeChecker(string dCode)
        {
            var user = db.Users.Single(u => u.Mobile == User.Identity.Name);
            bool isUsed = false;

            if (db.Discounts.Any(d => d.UserID == user.UserID && d.DiscountCode == dCode && d.IsUsed == false && d.ExpireDate > DateTime.Now))
            {
                Discounts discounts = db.Discounts.FirstOrDefault(d => d.UserID == user.UserID && d.DiscountCode == dCode && d.IsUsed == false && d.ExpireDate > DateTime.Now);
                isUsed = true;
                Session["UseDiscount"] = isUsed;
                Session["Discount"] = discounts;
                ViewBag.DiscountPercent = Session["Discount"];

                return PartialView("Order", getListOrder());
            }
            else
            {
                ViewBag.DiscountIsNotCorrect = "وجود ندارد!";
            }
            return "";

        }

        public ActionResult Shipping(int id)
        {
            var dw = db.DeliveryWays.Find(id);
            var shCost = dw.Price;


            Session["Shipping"] = dw;
            ViewBag.Shipping = Session["Shipping"];
            List<DeliveryWays> dList = db.DeliveryWays.ToList();
            ViewBag.DList = dList;
            return PartialView("Order", getListOrder());
        }

        [Authorize]
        public void DeleteOrders(int id)
        {
            int userId = GetUserIdWithUserName();
            var order = db.Orders.Find(id);
            if (order != null && order.UserID == userId)
            {
                var od = db.OrderDetails.Where(o => o.OrderID == id);
                foreach (var item in od)
                {
                    db.OrderDetails.Remove(item);
                }
                db.Orders.Remove(order);
                db.SaveChanges();
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteAccountOrders(int id)
        {
            DeleteOrders(id);
            int userId = GetUserIdWithUserName();
            return PartialView("AccountOrders", db.Orders.Where(o => o.UserID == userId).ToList());
        }

        [Authorize]
        public ActionResult AccountOrders()
        {
            int userId = GetUserIdWithUserName();
            return PartialView(db.Orders.Where(o => o.UserID == userId).ToList());
        }

        [HttpPost]
        public void LogAddtoCart(int productId, string url)
        {
            LoggedCart shopCart = new LoggedCart()
            {
                ProductID = productId,
                UserName = User.Identity.Name,
                Date = DateTime.Now,
                Url = url
            };

            try
            {
                if (shopCart.UserName != null && shopCart.UserName != "admin")
                {
                    db.LoggedCart.Add(shopCart);
                    db.SaveChanges();
                }
            }
            catch { }

        }
    }
}
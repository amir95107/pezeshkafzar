using DataLayer;
using MyEshop;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication100.Controllers
{
    [Authorize]
    public class PayController : Controller
    {
        DataLayer.medab_DBEntities db = new DataLayer.medab_DBEntities();
        public ActionResult Index(int? id)
        {
            ViewBag.Title = "صفحه اصلی";

            return View(db.Orders.Find(id));
        }

        public int GetUserIdWithUserName()
        {
            var userName = User.Identity.Name;
            var id = db.Users.Single(u => u.Mobile == userName).UserID;
            return id;
        }

        void FinalizeOrder(int id)
        {
            Orders order = db.Orders.Find(id);
            order.IsFinaly = true;
            order.PaymentWay = 1;
            bool isUsed = false;
            if (Session["UseDiscount"] != null)
            {
                isUsed = bool.Parse(Session["UseDiscount"].ToString());
            }
            order.UseDiscountCode = isUsed;
            db.Entry(order).State = EntityState.Modified;
            var od = db.OrderDetails.Where(o => o.OrderID == id);
            foreach (var item in od)
            {
                db.Products.Find(item.ProductID).Stock -= item.Count;
                db.Entry(item).State = EntityState.Modified;
            }
            var userInfo = order.Users.UserInfo.FirstOrDefault(u => u.UserID == order.UserID);
            var name = userInfo.FullName.Split(' ')[0];
            var sms = SendSMS.SendSuccessOrderMessage(order.Users.Mobile, name, order.TraceCode);
            if (!sms.Contains("ارسال موفق بود"))
            {
                if (userInfo.IsEmailConfirmed)
                {
                    string body = PartialToStringClass.RenderPartialView("ManageEmails", "SuccessOrderEmail", order);
                    SendEmail.Send(userInfo.Email, "", "ثبت موفق سفارش در پزشک افزار", body);
                }
            }
            db.SaveChanges();
        }

        public ActionResult Pay(int? orderId)
        {
            db.Orders.Find(orderId).PaymentWay = 1;
            db.SaveChanges();
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();

            string MerchantID = "49c906f6-b307-49be-9ebd-00ba583b6de9";
            string CallbackURL = Request.Url.Scheme + "://" + Request.Url.Authority + "/Pay/Verify/" + orderId;

            long Amount = db.Orders.Find(orderId).Payable;

            string Description = "جهت خرید از فروشگاه تجهیزات پزشکی پزشک افزار";

            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantID, Amount, CallbackURL, Description);


            //zarinpal.EnableSandboxMode();
            zarinpal.DisableSandboxMode();
            var res = zarinpal.InvokePaymentRequest(pr);
            if (res.Status == 100)
            {
                Response.Redirect(res.PaymentURL);
            }

            return View();
        }

        [AllowAnonymous, Route("Pay/Amount/{amount}")]
        public ActionResult PayAmount(int amount)
        {
            ZarinPal.ZarinPal zarinpal = ZarinPal.ZarinPal.Get();

            string MerchantID = "49c906f6-b307-49be-9ebd-00ba583b6de9";
            string CallbackURL = Request.Url.Scheme + "://" + Request.Url.Authority;


            string Description = "جهت خرید از فروشگاه تجهیزات پزشکی پزشک افزار";

            ZarinPal.PaymentRequest pr = new ZarinPal.PaymentRequest(MerchantID, amount, CallbackURL, Description);


            //zarinpal.EnableSandboxMode();
            zarinpal.DisableSandboxMode();
            var res = zarinpal.InvokePaymentRequest(pr);
            if (res.Status == 100)
            {
                Response.Redirect(res.PaymentURL);
            }

            return View();
        }


        [Route("Pay/Verify/{orderId}")]
        public ActionResult Verify(string Authority, string Status, int orderId)
        {
            if (Status != "OK")
            {
                ViewBag.msg = "پرداخت ناموفق.";
                return View();
            }

            var zarinpal = ZarinPal.ZarinPal.Get();

            string MerchantID = "49c906f6-b307-49be-9ebd-00ba583b6de9";

            var order = db.Orders.Find(orderId);
            long Amount = order.Payable;

            var pv = new ZarinPal.PaymentVerification(MerchantID, Amount, Authority);
            var verificationResponse = zarinpal.InvokePaymentVerification(pv); // For use WithExtra method InvokePaymentVerificationWithExtra()
            if (verificationResponse.Status == 100)
            {
                FinalizeOrder(orderId);
                ViewBag.msg = "پرداخت با موفقیت انجام شد." + "\n کد پیگیری :" + verificationResponse.RefID;
                ViewBag.Trace = order.TraceCode;
                Session.Clear();
            }
            else
            {
                ViewBag.msg = "پرداخت ناموفق." + verificationResponse.Status;
            }
            return View();
        }

        [Route("OrderTracking/{traceCode}")]
        public ActionResult OrderTracking(string traceCode)
        {
            var order = db.Orders.SingleOrDefault(o => o.TraceCode == traceCode);
            if (order != null)
            {
                var user = User.Identity.Name;
                if (order.Users.UserName != user)
                {
                    return Redirect("/PageNotFound");
                }
                else
                {
                    return View(order);
                }
            }
            else
            {
                return Redirect("/PageNotFound");
            }
        }

        public ActionResult OrderedList(string traceCode)
        {
            var order = db.Orders.SingleOrDefault(o => o.TraceCode == traceCode);
            return PartialView(order);
        }



    }
}
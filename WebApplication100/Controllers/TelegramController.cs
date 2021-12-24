using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Telegram.Bot.Types;
using Telegram.Bot.Args;
using DataLayer;
using Telegram.Bot.Types.ReplyMarkups;
using MyEshop;
using System.Configuration;

namespace WebApplication100.Controllers
{
    public class TelegramController : Controller
    {
        medab_DBEntities db = new medab_DBEntities();
        // GET: Telegram
        private static string token = "1156965526:AAFGa_2IMMcJew5JpvL7VcLhzoElo6RenMY";
        private Thread botThread;
        private Telegram.Bot.TelegramBotClient bot;
        private ReplyKeyboardMarkup mainKeyBoardMarkup;



        public ActionResult GetNumber(int id)
        {
            var product = db.Products.Find(id);
            return PartialView(product);
        }


        [HttpPost]
        public bool Start(int id, string num)
        {
            var product = db.Products.Find(id);
            //botThread = new Thread(new ThreadStart(() =>
            //{
            //    bot = new Telegram.Bot.TelegramBotClient(token);//var chatId = 104950188; pouya , 266809220; amirhossein
            //    long[] chatId = { 266809220, 104950188 };
            //    foreach (var item in chatId)
            //    {
            //        bot.SendTextMessageAsync(item, "اطلاعات مربوط به محصول " + product.Title + " با کدمحصول " + product.ProductID + " رو بفرست برای شماره " + num);
            //    }

            //}));
            //botThread.Start();
            string href = $"<a href='tel:{num}'>{num}</a>";
            string img = $"<a href='{ConfigurationManager.AppSettings["MyDomain"]}/ShowProduct/{product.ProductID}/{product.SefUrl}'><img src='{ConfigurationManager.AppSettings["MyDomain"]}/images/productimages/thumb/{product.ImageName}' /></a>";
            string body = "اطلاعات محصول " + product.Title + " برای مشتری با شماره " + href + " بفرست.<br/>" + img + "<br/>برای مشاهده محصول در سایت روی تصویر کلیک کن.";
            bool res = false;
            try
            {
                SendEmail.Send("meysam.khorasani81@gmail.com", "a.janmohammadi@gmail.com", "ارسال اطلاعات محصول برای مشتری", body);
                //string message = "عملیات با موفقیت انجام شد. لطفا منتظر تماس کارشناسان ما باشید.";
                res = true;
            }
            catch
            {
                //string message = "عملیات با شکست مواجه شد. لطفا بعدا تلاش کنید.";
                res = false;
            }


            //storing the number
            if (!db.Lead_Clients.Any(m => m.Mobile == num))
            {
                Lead_Clients lead = new Lead_Clients()
                {
                    Mobile = num,
                    CreateDate = DateTime.Now
                };
                try
                {
                    db.Lead_Clients.Add(lead);
                    db.SaveChanges();
                }
                catch { }
            }

            return res;
        }
    }
}
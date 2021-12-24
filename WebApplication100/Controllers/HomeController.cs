using DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication100.Models;
using WebApplication100.Utilities;

namespace MyEshop.Controllers
{

    public class HomeController : Controller
    {
        medab_DBEntities db = new medab_DBEntities();
        public int GetUserIdWithUserName()
        {
            var userName = User.Identity.Name;
            var id = db.Users.Single(u => u.Mobile == userName).UserID;
            return id;
        }

        // GET: Home
        public ActionResult Index()
        {
            List<Brands> bList = db.Brands.ToList();
            ViewBag.Brands = bList;
            return View();

        }

        [Route("AboutUs")]
        public ActionResult AboutUs()
        {
            Page data = db.Page.Find(2);
            ViewBag.PageTitle = data.PageTitle;
            ViewBag.Title = data.HeadTitle;
            ViewBag.Description = data.MetaDescription;
            ViewBag.Content = data.PageContent;
            return View();
        }

        [Route("ContactUs")]
        public ActionResult ContactUs()
        {
            Page data = db.Page.Find(1);
            ViewBag.PageTitle = data.PageTitle;
            ViewBag.Title = data.HeadTitle;
            ViewBag.Description = data.MetaDescription;
            ViewBag.Content = data.PageContent;
            return View();
        }

        [HttpPost]
        [Route("SendMessage")]
        public ActionResult SendMessage(ContactForm contact, FormCollection form)
        {
            

            if (ModelState.IsValid)
            {
                string urlToPost = "https://www.google.com/recaptcha/api/siteverify";
                string secretKey = "6LfwqHQaAAAAAMM2R1tFwQdmak_N9stfEGYKofVu"; // change this
                string gRecaptchaResponse = form["g-recaptcha-response"];

                var postData = "secret=" + secretKey + "&response=" + gRecaptchaResponse;

                // send post data
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlToPost);
                request.Method = "POST";
                request.ContentLength = postData.Length;
                request.ContentType = "application/x-www-form-urlencoded";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(postData);
                }

                // receive the response now
                string result = string.Empty;
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }
                }

                // validate the response from Google reCaptcha
                var captChaesponse = JsonConvert.DeserializeObject<reCaptchaResponse>(result);
                if (!captChaesponse.Success)
                {
                    ViewBag.Message = "لطفا عبارت امنیتی را تایید کنید.";
                    return View("ContactUs");
                }


                contact.Date = DateTime.Now;
                if (User.Identity.IsAuthenticated)
                {
                    contact.UserID = GetUserIdWithUserName();
                }
                db.ContactForm.Add(contact);
                db.SaveChanges();
                TempData["Result"] = "ok";
                TempData["Message"] = "پیام با موفقیت دریافت شد";
                SendEmail.Send("a.janmohammadi@gmail.com","","پیام جدید با موضوع: "+ contact.Subject,"نام: "+contact.Subject+"\n متن پیام: "+contact.Text);
                return RedirectToAction("ContactUs");
            }
            else
            {
                TempData["Result"] = "error";
                TempData["Message"] = "یک یا چند فیلد به اشتباه پر شده است";
                return View(contact);
            }
        }

        [Route("Faq")]
        public ActionResult Faq()
        {
            Page data = db.Page.Find(3);
            ViewBag.PageTitle = data.PageTitle;
            ViewBag.Title = data.HeadTitle;
            ViewBag.Description = data.MetaDescription;
            ViewBag.Content = data.PageContent;
            return View(db.ContactForm.Where(f=>f.IsFaq).ToList());
        }

        [Route("terms-and-conditions")]
        public ActionResult PrivacyAndPolicy()
        {
            Page data = db.Page.Find(4);
            ViewBag.PageTitle = data.PageTitle;
            ViewBag.Title = data.HeadTitle;
            ViewBag.Description = data.MetaDescription;
            ViewBag.Content = data.PageContent;
            return View();
        }

        public ActionResult Header()
        {
            var roleId = 1;
            bool isUserInformationComplete = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.FirstOrDefault(u => u.Mobile == User.Identity.Name);
                if(user == null)
                {
                    return Redirect("/ServerError");
                }
                roleId = user.Roles.RoleID;
                isUserInformationComplete = user.IsUserInfoCompleted;
            }
            ViewBag.IsUserInformationComplete = isUserInformationComplete;
            ViewBag.Role = roleId;
            return PartialView();
        }

        public ActionResult Categories()
        {
            return PartialView();
        }

        public ActionResult Slider()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            return PartialView(db.Slider.Where(s => s.IsActive == true && s.StartDate <= dt && s.EndDate >= dt).OrderByDescending(s=>s.StartDate));
        }

        public ActionResult VisitSite()
        {
            DateTime dt = DateTime.Now.Date;
            DateTime yesterday = dt.AddDays(-1);
            DataLayer.ViewModels.VisitSiteViewModel visit = new DataLayer.ViewModels.VisitSiteViewModel();
            visit.VisitSum = db.SiteVisit.Count();
            visit.VisitToday = db.SiteVisit.Count(v => v.Date == dt);
            visit.VisitYesterdaye = db.SiteVisit.Count(v => v.Date == yesterday);
            return PartialView(visit);
        }

        public ActionResult HumanBody()
        {
            return PartialView();
        }


        public void Lead(string num)
        {
            Lead_Clients lead = new Lead_Clients();
            lead.CreateDate = DateTime.Now;
            lead.Mobile = num;
            db.Lead_Clients.Add(lead);
            db.SaveChanges();
        }

        public ActionResult Footer()
        {
            return PartialView();
        }

        [Route("PageNotFound")]
        public ActionResult PageNotFound()
        {
            return View();
        }

        [Route("ServerError")]
        public ActionResult ServerError()
        {
            return View();
        }

        public ActionResult SomeProductGroups()
        {
            return PartialView(db.Product_Groups.ToList());
        }

        public ActionResult BottomMenu()
        {
            return PartialView();
        }
    }
}


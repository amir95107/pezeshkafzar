using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModels;
using System.Web.Security;
using System.Data.Entity;
using System.Web.UI.WebControls.WebParts;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using WebApplication100.Models;
using System.Net.Http;
using WebApplication100.Utilities;
using System.Threading.Tasks;

namespace MyEshop.Controllers
{
    public class AccountController : Controller
    {
        medab_DBEntities db = new medab_DBEntities();

        private int GetUserIdWithUserName()
        {
            var userName = User.Identity.Name;
            var id = db.Users.Single(u => u.Mobile == userName).UserID;
            return id;
        }

        [Authorize]
        public ActionResult Index()
        {
            Users user = db.Users.FirstOrDefault(u => u.Mobile == User.Identity.Name);
            return View(user);
        }

        public ActionResult GetListOrder()
        {
            int userId = GetUserIdWithUserName();
            if (db.Orders.Any(o => o.IsFinaly && !o.IsSent && o.Users.UserID == userId))
            {
                List<string> traceode = db.Orders.Where(o => o.IsFinaly && !o.IsSent && o.Users.UserID == userId).Select(o => o.TraceCode).ToList();
                //ViewBag.Order = traceode;
                return Json(traceode, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        // GET: Account
        [Route("Register")]
        public ActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }



        [HttpPost]
        [Route("Register")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel register, FormCollection form, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                #region googleRecaptcha
                //google recaptcha
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
                    return View();
                }
                //google recaptcha
                #endregion
                if (register.Mobile.Length != 11 || register.Mobile.Substring(0, 2) != "09")
                {
                    TempData["RegisterError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
                    return View(register);
                }
                var users = db.Users.ToList();
                Random rnd = new Random();
                int num = rnd.Next(100, 100000);
                if (!users.Any(u => u.Mobile == register.Mobile.Trim()))
                {
                    Users user = new Users()
                    {
                        Mobile = register.Mobile.Trim(),
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(register.Password, "MD5"),
                        ActiveCode = Guid.NewGuid().ToString(),
                        IsActive = false,
                        RegisterDate = DateTime.Now,
                        RoleID = 1,
                        IsUserInfoCompleted = false,
                        UserName = "User-" + num,
                        Email = "user@pezeshkafzar.com",
                    };
                    db.Users.Add(user);
                    db.SaveChanges();



                    //send verivication code
                    var verifyCode = SendSMS.SendVerificationCode(register.Mobile);
                    //string[] verifyCode = new string[] { "123456", "ارسال موفق بود" };

                    if (verifyCode[1].Contains("ارسال موفق بود"))
                    {
                        Session["VerifyCode"] = verifyCode[0];
                        Session.Timeout = 2;
                        return View("SuccessVerifyRegister", user);
                    }
                    TempData["RegisterError"] = "خطایی در ارسال پیامک رخ داده. لطفا بعدا تلاش کنید.";
                    return View(register);

                }
                else
                {
                    TempData["RegisterError"] = "شما قبلا ثبت نام کرده اید.";
                    return View(register);
                }


            }
            TempData["RegisterError"] = "یکی از فیلدها به اشتباه پر شده است.";
            return View(register);
        }

        [Route("RegisterSeller")]
        public ActionResult RegisterSeller()
        {
            return View();
        }

        

        [Authorize]
        [HttpPost]
        public bool ActivateEmail()
        {
            var user = db.Users.FirstOrDefault(u => u.Mobile == User.Identity.Name);
            var userInfo = user.UserInfo.FirstOrDefault(u => u.UserID == user.UserID);
            if (userInfo.Email == null)
            {
                return false;
            }
            if (!userInfo.IsEmailConfirmed)
            {
                try
                {
                    string body = PartialToStringClass.RenderPartialView("ManageEmails", "ActiviationEmail", userInfo);
                    SendEmail.Send(userInfo.Email, "", "ایمیل فعالسازی", body);
                    return true;
                }
                catch { }
            }
            return false;
        }

        [HttpPost]
        [Route("RegisterSeller")]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterSeller(RegisterSellerViewModel registerSeller)
        {
            if (ModelState.IsValid)
            {
                if (!db.Users.Any(u => u.Mobile == registerSeller.Mobile.Trim()))
                {
                    Users user = new Users()
                    {
                        Mobile = registerSeller.Mobile.Trim(),
                        Password = FormsAuthentication.HashPasswordForStoringInConfigFile(registerSeller.Password, "MD5"),
                        ActiveCode = Guid.NewGuid().ToString(),
                        IsActive = false,
                        RegisterDate = DateTime.Now,
                        RoleID = 3,
                        IsMobileConfirmed=false,
                        IsUserInfoCompleted=false
                    };
                    db.Users.Add(user);

                    Sellers seller = new Sellers()
                    {
                        StoreName = registerSeller.StoreName,
                        IsAcceptedByAdmin = false,
                    };
                    db.Sellers.Add(seller);
                    db.SaveChanges();

                    //Send Active Email to seller
                    var verifyCode = SendSMS.SendSellerVerificationCode(user.Mobile);
                    //string[] verifyCode = new string[] { "123456", "ارسال موفق بود" };

                    if (verifyCode[1].Contains("ارسال موفق بود"))
                    {
                        Session["VerifyCode"] = verifyCode[0];
                        Session.Timeout = 2;
                        return View("SuccessVerifyRegisterSeller", user);
                    }
                    //End Send Active Email

                    TempData["RegisterError"] = "خطایی در ارسال پیامک رخ داده. لطفا بعدا تلاش کنید.";
                    return View(registerSeller);
                }
                else
                {
                    TempData["RegisterError"] = "این موبایل قبلا در سیستم ثبت شده است.";
                    return View(registerSeller);
                }
            }
            TempData["RegisterError"] = "یکی از فیلدها به اشتباه پر شده است.";
            return View(registerSeller);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel login, FormCollection form, string ReturnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                //google recaptcha
                #region googleRecaptcha
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
                    return View();
                }
                #endregion
                //google recaptcha

                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(login.Password, "MD5");
                var user = db.Users.SingleOrDefault(u => u.Email == login.Email && u.Password == hashPassword);
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, login.RememberMe);
                        
                        if (ReturnUrl == "" || ReturnUrl == null) ReturnUrl = "/";
                        if (user.RoleID == 2)
                        {
                            string body = user.UserName + "در تاریخ " + DateTime.Now + " وارد پنل شد!";
                            //SendEmail.Send("support@pezeshkafzar.com", "ورود به پنل ادمین", body);
                        }
                        if (Session["ShopCart"] != null)
                        {
                            return RedirectToAction("Index", "ShopCart");
                        }
                        else
                        {
                            return Redirect(ReturnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نشده است");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری با اطلاعات وارد شده یافت نشد");
                }
            }

            return View(login);
        }

        [Route("Login")]
        public ActionResult UserLogin()
        {
            if(User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Login")]
        public ActionResult UserLogin(UserLoginWithMobileViewModel user, string ReturnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                if (user.Mobile.Length != 11 || user.Mobile.Substring(0, 2) != "09")
                {
                    TempData["LoginError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
                    return View(user);
                }
                string hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "MD5");
                Users users = db.Users.FirstOrDefault(u => u.Mobile == user.Mobile && u.Password == hashPassword);
                users.LoginAtempts++;
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                if (users == null)
                {
                    TempData["LoginError"] = "شماره موبایل و یا رمز عبور به اشتباه وارد شده است.";
                    return View(user);
                }
                else
                {
                    if (users.IsMobileConfirmed)
                    {
                        FormsAuthentication.SetAuthCookie(user.Mobile, user.RememberMe);
                        
                        if (ReturnUrl == "" || ReturnUrl == null) ReturnUrl = "/";

                        if (Session["ShopCart"] != null)
                        {
                            return RedirectToAction("Index", "ShopCart");
                        }
                        else
                        {
                            return Redirect(ReturnUrl);
                        }
                    }
                    else
                    {
                        TempData["NoVerifyError"] = "شماره موبایل شما هنوز تایید نشده است.";
                        return View(user);
                    }
                }
            }
            ModelState.AddModelError("Mobile", "موبایل وارد شده در فرمت درستی نیست.");
            return View(user);
        }

        [AllowAnonymous]
        public ActionResult UserLoginWithCode()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLoginWithCode(string mobile, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (mobile.Length != 11 || mobile.Substring(0, 2) != "09")
                {
                    TempData["LoginError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
                    return View();
                }
                Users user = db.Users.FirstOrDefault(u => u.Mobile == mobile);
                if (user == null)
                {
                    TempData["LoginError"] = "شما هنوز ثبت نام نکرده اید.";
                    return View(user);
                }
                else
                {
                    if ((bool)user.IsMobileConfirmed)
                    {

                        string name = user.UserName;
                        if (name == null)
                        {
                            name = "کاربر";
                        }
                        string[] verify = SendSMS.SendLoginVerificationCode(mobile, name);
                        //string[] verifyCode = new string[] { "123456", "ارسال موفق بود" };

                        if (verify[1].Contains("ارسال موفق بود"))
                        {
                            Session["VerifyCode"] = verify[0];
                            Session.Timeout = 2;
                            return View("SuccessVerifyCode", user);
                        }
                        else
                        {
                            TempData["LoginError"] = "خطایی در ارسال پیامک رخ داده. لطفا از روشی دیگر برای ورود استفاده کنید و یا بعدا تلاش کنید..";
                            return View(user);
                        }
                    }
                    else
                    {
                        TempData["NoVerifyError"] = "شماره موبایل شما هنوز تایید نشده است.";
                        return View(user);
                    }
                }
            }
            else
            {
                return View(mobile);
            }
        }

        public ActionResult ResendRegisterVerificationCode(string mobile)
        {
            var user = db.Users.FirstOrDefault(u => u.Mobile == mobile);
            if (mobile.Length != 11 || mobile.Substring(0, 2) != "09")
            {
                TempData["LoginError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
            }
            else
            {
                if (user != null && (bool)!user.IsMobileConfirmed)
                {
                    var userName = user.UserName;
                    if (userName == null)
                    {
                        userName = "کاربر";
                    }
                    var verify = SendSMS.SendLoginVerificationCode(mobile, userName);
                    //string[] verify = new string[] { "123456", "ارسال موفق بود" };
                    if (verify[1].Contains("ارسال موفق بود"))
                    {
                        Session["VerifyCode"] = verify[0];
                        Session.Timeout = 2;
                    }
                }
            }
            return View("SuccessVerifyRegister", user);
        }

        public ActionResult ResendLoginVerificationCode(string mobile)
        {
            var user = db.Users.FirstOrDefault(u => u.Mobile == mobile);
            if (mobile.Length != 11 || mobile.Substring(0, 2) != "09")
            {
                TempData["LoginError"] = "فرمت شماره موبایل اشتباه وارد شده است .";
            }
            else
            {

                if (user != null && (bool)user.IsMobileConfirmed)
                {
                    var userName = user.UserName;
                    if (userName == null)
                    {
                        userName = "کاربر";
                    }
                    var verify = SendSMS.SendLoginVerificationCode(mobile, userName);
                    //string[] verify = new string[] { "123456", "ارسال موفق بود" };
                    if (verify[1].Contains("ارسال موفق بود"))
                    {
                        Session["VerifyCode"] = verify[0];
                        Session.Timeout = 2;
                    }
                }
            }

            return View("SuccessVerifyCode", user);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyCode(string code, string Mobile, string referPage)
        {
            var refer = referPage.ToLower().Contains("register");
            var goTo = refer ? "SuccessVerifyRegister" : "SuccessVerifyCode";
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.Mobile == Mobile);
                var theCode = Session["VerifyCode"];
                if (theCode == null)
                {
                    TempData["VerifyError"] = "کد تایید وارد شده صحیح نمیباشد.";
                    return View("ResendLoginVerificationCode", Mobile);
                }
                if (theCode.ToString() == code)
                {
                    Session["VerifyCode"] = null;
                    if (referPage.ToLower() == "register")
                    {
                        user.IsMobileConfirmed = true;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["SuccessRegister"] = "ثبت نام شما با موفقیت انجام شد";
                        return Redirect("/Login?ref=register");
                    }
                    else if (referPage.ToLower() == "seller")
                    {
                        user.IsMobileConfirmed = true;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        TempData["SuccessRegister"] = "ثبت نام شما با موفقیت انجام شد";
                        return Redirect("/Login?ref=RegisterSeller");
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(Mobile, true);
                        if (Session["ShopCart"] != null)
                        {
                            return RedirectToAction("Index", "ShopCart");
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                }
                else
                {
                    TempData["VerifyError"] = "کد تایید وارد شده صحیح نمیباشد.";
                    return View(goTo, user);
                }
            }
            else
            {
                TempData["VerifyError"] = "خطایی رخ داده.لطفا بعدا تلاش کنید.";
                return RedirectToAction("Register");
            }
        }

        public ActionResult ActiveUser(string id)
        {
            var user = db.UserInfo.SingleOrDefault(u => u.ActiveCode == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.IsEmailConfirmed)
            {
                View(user);
            }

            user.IsEmailConfirmed = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            db.SaveChanges();
            ViewBag.UserName = user.FullName.Split(' ')[0];
            return RedirectToAction("Index",user.Users);
        }

        public ActionResult ActiveSeller(string id)
        {
            var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.IsActive = true;
            user.ActiveCode = Guid.NewGuid().ToString();
            db.SaveChanges();
            ViewBag.Name = user.UserName;
            return View();
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        [Route("ForgotPassword")]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        [Route("ForgotPassword")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Email == forgot.Email);
                if (user != null)
                {
                    if (user.IsActive)
                    {
                        string bodyEmail =
                            PartialToStringClass.RenderPartialView("ManageEmails", "RecoveryPassword", user);
                        SendEmail.Send(user.Email, "", "بازیابی کلمه عبور", bodyEmail);
                        return View("SuccesForgotPassword", user);
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "حساب کاربری شما فعال نیست");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "کاربری یافت نشد");
                }
            }
            return View();
        }

        public ActionResult RecoveryPassword(string id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecoveryPassword(string id, RecoveryPasswordViewModel recovery)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.ActiveCode == id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(recovery.Password, "MD5");
                user.ActiveCode = Guid.NewGuid().ToString();
                db.SaveChanges();
                return Redirect("/Login?recovery=true");
            }
            return View();
        }

        [Authorize]
        public ActionResult UserInformation()
        {
            string user = User.Identity.Name;
            var isComplete = db.Users.Single(u => u.Mobile == user).IsUserInfoCompleted;
            if (isComplete)
            {
                return RedirectToAction("EditUserInfo");
            }
            if (!isComplete)
            {
                var rd = "/Account/UserInformation?previousUrl=ShopCart";
                ViewBag.Redirect = rd;
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserInformation(UserInfoViewModel userInfo, string ReturnUrl)
        {
            int uid = db.Users.Single(u => u.Mobile == User.Identity.Name).UserID;
            UserInfo userInfo1 = new UserInfo()
            {
                UserID = uid,
                FullName = userInfo.FullName,
                Email = userInfo.Email,
                Telephone = userInfo.Telephone,
                ActiveCode=Guid.NewGuid().ToString(),
                IsEmailConfirmed=false
            };
            db.UserInfo.Add(userInfo1);
            db.Users.Find(uid).IsUserInfoCompleted = true;
            db.SaveChanges();
            var rq = Request.QueryString["previousUrl"];
            if (ReturnUrl != null)
            {
                return Redirect(ReturnUrl);
            }
            return Redirect("/");

        }

        [Authorize]
        public ActionResult EditUserInfo()
        {
            string userName = User.Identity.Name;
            var isComplete = db.Users.Single(u => u.Mobile == userName).IsUserInfoCompleted;
            if (!isComplete)
            {
                return RedirectToAction("UserInformation");
            }
            UserInfo ui = db.UserInfo.FirstOrDefault(u => u.Users.Mobile == userName);
            return View(ui);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInfo(UserInfo ui,string returnUrl)
        {
            string userName = User.Identity.Name;
            var userInfo = db.UserInfo.Find(ui.UIID);
            userInfo.Email = ui.Email;
            userInfo.IsEmailConfirmed = ui.IsEmailConfirmed;
            userInfo.Telephone = ui.Telephone;
            userInfo.FullName = ui.FullName;
            userInfo.UserID = ui.UserID;
            if (userInfo.Email!=null && userInfo.Email.ToLower().Trim() != ui.Email.ToLower().Trim())
            {
                userInfo.IsEmailConfirmed = false;
            }
            userInfo.ActiveCode = Guid.NewGuid().ToString();
            db.Entry(userInfo).State = EntityState.Modified;
            db.SaveChanges();
            TempData["SuccessEditUSerInfo"] = "اطلاعات با موفقیت بروزرسانی شد.";
            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return View(db.UserInfo.FirstOrDefault(u => u.Users.Mobile == userName));
        }

        [Authorize]
        public ActionResult MyOrders()
        {
            var userId = GetUserIdWithUserName();
            List<Orders> orders = db.Orders.Where(o => o.UserID == userId).ToList();
            return View(orders);
        }

        [Authorize]
        public ActionResult ShowOrderDetails(int id)
        {
            ViewBag.isFinally = db.Orders.Find(id).IsFinaly;
            var orderDetails = db.OrderDetails.Where(o => o.OrderID == id).ToList();
            return PartialView(orderDetails);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel change)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Single(u => u.Mobile == User.Identity.Name);
                string oldPasswordHash =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(change.OldPassword, "MD5");
                if (user.Password == oldPasswordHash)
                {
                    string hashNewPasword =
                        FormsAuthentication.HashPasswordForStoringInConfigFile(change.Password, "MD5");
                    user.Password = hashNewPasword;
                    db.SaveChanges();
                    ViewBag.Success = true;
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "کلمه عبور فعلی درست نمی باشد");
                }
            }
            return View();
        }

        public string SessionChecker(string name)
        {
            var session = Session[name];
            if (session != null)
            {
                return session.ToString();
            }
            else
            {
                return "";
            }
        }

        [HttpPost]
        [Route("SessionTimeouter")]
        public bool SessionTimeouter(string name, int? time)
        {
            bool isNull = false;
            if (Session[name] != null)
            {
                if (time != null)
                {
                    Task.Delay(TimeSpan.FromMinutes((int)time));
                }
                Session[name] = null;
                return !isNull;
            }
            return isNull;
        }
        public void SetEmailInDb()
        {
            List<Users> users = db.Users.ToList();
            List<UserInfo> userInfos = db.UserInfo.ToList();

            foreach (var item in userInfos) {
                item.Email = users.FirstOrDefault(u => u.UserID == item.UserID).Email;
                db.Entry(item).State = EntityState.Modified;
            }
            db.SaveChanges();
        }
    }
}
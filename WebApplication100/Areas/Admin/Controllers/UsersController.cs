using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer;
using Microsoft.Ajax.Utilities;

namespace MyEshop.Areas.Admin.Controllers
{
    //[Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        // GET: Admin/Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.Roles);
            return View("GetUsers", users.ToList());
        }

        // GET: Admin/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Admin/Users/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleTitle");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserID,RoleID,UserName,Email,Password,ActiveCode,IsActive,RegisterDate,Mobile,IsUserInfoCompleted,IsMobileConfirmed")] Users user)
        {
            if (ModelState.IsValid)
            {
                var users = db.Users.ToList();
                if (users.Any(u => u.Mobile == user.Mobile))
                {
                    ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleTitle", user.RoleID);
                    ModelState.AddModelError("Mobile", "موبایل تکراری است.");
                    return View(user);
                }
                else
                {
                    user.RegisterDate = DateTime.Now;
                    user.ActiveCode = Guid.NewGuid().ToString();
                    user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(user.Password, "MD5");
                    user.IsMobileConfirmed = false;
                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("GetUsers", db.Users.ToList());
                }

            }

            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleTitle", user.RoleID);
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleTitle", users.RoleID);
            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,RoleID,UserName,Email,Password,ActiveCode,IsActive,RegisterDate,Mobile,IsUserInfoCompleted,IsMobileConfirmed")] Users users)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Mobile == users.Mobile))
                {
                    ViewBag.RoleID = new SelectList(db.Roles, "RoleID", "RoleTitle", users.RoleID);
                    ModelState.AddModelError("Mobile", "این شماره قبلا در سیستم ثبت شده است.");
                    return View(users);
                }
                else
                {
                    db.Entry(users).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("GetUsers");
                }
            }
            
            return View(users);
        }

        // GET: Admin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetUsers()
        {
            ViewBag.Roles = db.Roles.ToList();
            return View();
        }

        public ActionResult UsersTable(int? id)
        {
            var users = db.Users.ToList();

            if (id > 0)
            {
                users = db.Users.Where(u => u.Roles.RoleID == id).ToList();
            }
            return PartialView(users);
        }

        public ActionResult EditUsers(int id)
        {
            var users = db.Users.Single(u => u.UserID == id);

            return PartialView(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsers([Bind(Include = "UserID,RoleID,Email,Password,ActiveCOde,IsActive,RegisterDate,IsUserInfoCompleted")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
            }

            return PartialView(users);
        }



        public ActionResult GetUserInformation(int id)
        {
            Users user = db.Users.Find(id);
            var userInfo = user.UserInfo;
            string address = "";
            if (user.Addresses.Any())
            {
                address = user.Addresses.FirstOrDefault(a => a.UserID == id).Address;
            }
            int orderCount = user.Orders.Where(o => o.UserID == id && o.IsFinaly).Count();
            ViewBag.Address = address;
            ViewBag.OrderCount = orderCount;
            return PartialView(userInfo.FirstOrDefault());
        }

        public ActionResult GetAllCustomers()
        {
            var customers = db.Orders.Select(o => o.Users).ToList();
            return View(customers);
        }

        public ActionResult EditUserInformation(int id)
        {
            var userInformation = db.UserInfo.Single(ui => ui.UserID == id);
            return PartialView(userInformation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserInformation(UserInfo userInfo)
        {

            if (ModelState.IsValid)
            {
                db.Entry(userInfo).State = EntityState.Modified;
                db.SaveChanges();
                return PartialView("GetUserInformation", db.UserInfo.Where(ui => ui.UserID == userInfo.UserID).ToList());
            }
            return PartialView(db.UserInfo.Single(ui => ui.UserID == userInfo.UserID));
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

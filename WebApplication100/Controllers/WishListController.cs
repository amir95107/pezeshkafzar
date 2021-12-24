using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Controllers
{
    [Authorize]
    public class WishListController : Controller
    {
        DataLayer.medab_DBEntities db = new DataLayer.medab_DBEntities();
        // GET: Compare

        public ActionResult Index()
        {
            var user = db.Users.FirstOrDefault(u => u.Mobile == User.Identity.Name);
            var likedProducts = db.LikeProduct.Where(p => p.UserID == user.UserID).Select(u => u.Products).ToList();
            return View(likedProducts);
        }

        [Authorize]
        public ActionResult AddToWish(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.FirstOrDefault(u => u.Mobile == User.Identity.Name);


                if (!db.LikeProduct.Any(p => p.ProductID == id && p.UserID == user.UserID))
                {
                    LikeProduct likeProduct = new LikeProduct()
                    {
                        UserID = user.UserID,
                        ProductID = id,
                        Date = DateTime.Now
                    };
                    db.LikeProduct.Add(likeProduct);
                    db.Products.Find(id).LikeCount++;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.ItWasLiked = "این محصول در لیست علاقه مندی ها موجود است!";
                }

                var list = db.LikeProduct.Where(p => p.UserID == user.UserID).Select(u => u.Products).ToList();

                return PartialView("WishList", list);
            }
            else
            {
                return Redirect("/Login");
            }

        }

        [Authorize]
        public ActionResult WishList()
        {
            var user = db.Users.SingleOrDefault(u => u.Mobile == User.Identity.Name);
            var likedProducts = db.LikeProduct.Where(p => p.UserID == user.UserID).Select(u => u.Products).ToList();
            ViewBag.LikedProductsCount = db.LikeProduct.Where(p => p.UserID == user.UserID).Count();
            return PartialView(likedProducts);
            //return PartialView(list);
        }

        public ActionResult LikedProducts()
        {
            var user = db.Users.SingleOrDefault(u => u.Mobile == User.Identity.Name);
            if (user == null)
            {
                user = db.Users.Single(u => u.Mobile == User.Identity.Name);
            }
            return PartialView(db.LikeProduct.Where(p => p.UserID == user.UserID).Select(u => u.Products).ToList());
        }

        public ActionResult DeleteFromWishList(int id)
        {
            var user = db.Users.Single(u => u.Mobile == User.Identity.Name);
            var product = db.LikeProduct.Single(p => p.ProductID == id && p.UserID == user.UserID);
            db.LikeProduct.Remove(product);
            db.Products.Find(id).LikeCount--;
            db.SaveChanges();
            var likedProducts = db.LikeProduct.Where(p => p.UserID == user.UserID).Select(u => u.Products).ToList();
            return PartialView("LikedProducts", likedProducts);
        }
    }
}
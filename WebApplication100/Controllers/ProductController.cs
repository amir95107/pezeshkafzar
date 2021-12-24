using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using DataLayer.ViewModels;

namespace MyEshop.Controllers
{
    public class ProductController : Controller
    {
        medab_DBEntities db = new medab_DBEntities();
        public ActionResult ShowGroups()
        {
            return PartialView(db.Product_Groups.Where(p => p.Product_Selected_Groups.Any()).ToList());
        }


        public ActionResult LastProduct()
        {
            var products = db.Products.Where(p => p.IsAcceptedByAdmin && p.IsActive && p.Stock>0 && p.PriceAfterDiscount>0).OrderByDescending(p => p.LastUpdated).Take(12);
            return PartialView(products);
        }

        public ActionResult SpecialOffer()
        {
            var products = db.Products.Where(p => p.Price != p.PriceAfterDiscount && p.PriceAfterDiscount > 0 && p.IsActive && p.IsAcceptedByAdmin).OrderByDescending(p => (p.Price - p.PriceAfterDiscount) * 100 / p.Price).Take(6);
            return PartialView(products);
        }

        [Route("AllOffers")]
        public ActionResult AllOffers(int pageId = 1)
        {
            var products = db.Products.Where(p => p.Price != p.PriceAfterDiscount && p.PriceAfterDiscount > 0 && p.IsActive == true && p.IsAcceptedByAdmin);
            int take = 16;
            int skip = take * (pageId - 1);
            var pageCount = products.Count() / take + 1;
            if (products.Count() % take == 0)
            {
                pageCount--;
            }
            ViewBag.PageCount = pageCount;
            ViewBag.CurrentPage = pageId;
            var product = products.OrderByDescending(p => (p.Price - p.PriceAfterDiscount) * 100 / p.Price).Skip(skip).Take(take).ToList();
            return View(product);
        }


        public ActionResult LastSeenProducts()
        {
            List<HttpCookie> list = new List<HttpCookie>();
            for (int i = Request.Cookies.Count - 1; i >= 0; i--)
            {
                if (list.Where(p => p.Name == Request.Cookies[i].Name).Any() == false)
                    list.Add(Request.Cookies[i]);
            }
            List<Products> lastSeen = new List<Products>();
            foreach (var item in list.Where(l => l.Name.StartsWith("SeenProduct-")))
            {
                var product = db.Products.Find(int.Parse(item.Value));
                if (product != null)
                {
                    lastSeen.Add(product);
                }
            }

            return PartialView(lastSeen.Take(5));
        }




        [Route("ShowProduct/{id}/{sefUrl}")]
        public ActionResult ShowProduct(int id, string sefUrl)
        {
            var product = db.Products.Find(id);
            if(product != null)
            {
                if (product.IsAcceptedByAdmin)
                {
                    if (product.SefUrl != sefUrl)
                    {
                        return RedirectPermanent($"/ShowProduct/{id}/{product.SefUrl}");
                    }
                    ViewBag.LastUpdate = TimeHelper.Detail((DateTime)product.LastUpdated);
                    ViewBag.Galleries = db.Product_Galleries.Where(p => p.ProductID == id).ToList();
                    //ViewBag.ProductFeatures = product.Product_Features.DistinctBy(f=>f.FeatureID).Select(f=>new ShowProductFeatureViewModel()
                    //{
                    //    FeatureTitle = f.Features.FeatureTitle,
                    //    Values = db.Product_Features.Where(fe=>fe.FeatureID==f.FeatureID).Select(fe=>fe.Value).ToList()
                    //}).ToList();
                    ViewBag.Size = db.Product_Features.Where(p => p.ProductID == id && p.FeatureID == 2).Select(p => p.Value).ToList();
                    if (product == null)
                    {
                        return HttpNotFound();
                    }
                    if (User.Identity.IsAuthenticated)
                    {
                        var userid = db.Users.Single(u => u.Mobile == User.Identity.Name).UserID;
                        ViewBag.IsLiked = db.LikeProduct.Any(p => p.ProductID == id && p.Users.UserID == userid);
                    }
                    HttpCookie cookie = new HttpCookie("SeenProduct-" + id.ToString())
                    {
                        Expires = DateTime.Now.AddMonths(1),
                        HttpOnly = true,
                        Value = id.ToString()
                    };
                    Response.Cookies.Add(cookie);
                    product.Visit++;
                    db.Entry(product).State = EntityState.Modified;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch { }

                    return View(product);
                }
                else
                {
                    return Redirect($"/PageNotFound?aspxerrorpath={Request.Url.LocalPath}");
                }
            }
            else
            {
                return Redirect($"/PageNotFound?aspxerrorpath={Request.Url.LocalPath}");
            }
        }

        [Route("ShowProduct/{id}")]
        public ActionResult RedirectToNew(int id)
        {
            Products product = db.Products.Find(id);

            if (product != null)
            {
                string sef = product.SefUrl;
                string url = "/ShowProduct/" + id + "/" + sef;
                return RedirectPermanent(url);
            }
            else
            {
                return RedirectPermanent("/Archive");
            }
        }

        [Route("ShowProduct/{id}/contactus")]
        public ActionResult RedirectToNew2(int id)
        {
            Products product = db.Products.Find(id);
            if (product != null)
            {
                string sef = product.SefUrl;
                string url = "/ShowProduct/" + id + "/" + sef;
                return RedirectPermanent(url);
            }
            else
            {
                return RedirectPermanent("/Archive");
            }
        }

        [Route("{str}/contactus")]
        public ActionResult RedirectToNew3(string str)
        {
            return RedirectPermanent("/ContactUs");
        }

        public ActionResult ShowComments(int id)
        {
            return PartialView(db.Comments.Where(c => c.ProductID == id));
        }

        [Route("Product/CreateComment/{id}")]
        public ActionResult CreateComment(int id)
        {
            return PartialView(new Comments()
            {
                ProductID = id
            });
        }

        [Authorize]
        [HttpPost]
        [Route("Product/CreateComment/{id}")]
        public ActionResult CreateComment(Comments productComment,int id)
        {
            if (ModelState.IsValid)
            {
                productComment.ProductID = id;
                productComment.CreateDate = DateTime.Now;
                productComment.IsShow = false;
                db.Comments.Add(productComment);
                db.SaveChanges();
                SendEmail.Send("a.janmohammadi@gmail.com","","کامنت برای محصول "+productComment.Products.Title,"یک نظر برای محصول "+ productComment.Products.Title + " از طرف "+productComment.Name+" اومده.");
                return PartialView("ShowComments", db.Comments.Where(c => c.ProductID == productComment.ProductID));
            }
            return PartialView(productComment);
        }

        [Route("Archive")]
        public ActionResult ArchiveProduct(int pageId = 1, string title = "", int minPrice = 0, int maxPrice = 0, List<int> selectedGroups = null, int brandId = 0)
        {
            ViewBag.Groups = db.Product_Groups.ToList();
            ViewBag.productTitle = title;
            
            ViewBag.pageId = pageId;
            ViewBag.NoProducts = "محصولی با مشخصات وارد شده موجود نیست!";
            ViewBag.selectGroup = selectedGroups;
            List<Products> list = new List<Products>();
            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (int group in selectedGroups)
                {
                    list.AddRange(db.Product_Selected_Groups.Where(g => g.GroupID == group).Select(g => g.Products).Where(p => p.IsAcceptedByAdmin == true && p.IsActive == true).ToList());
                }

            }
            else
            {
                list.AddRange(db.Products.Where(p => p.IsAcceptedByAdmin == true && p.IsActive).ToList());
            }

            if (list.Any())
            {
                ViewBag.minPrice = list.Select(p => p.Price).Min();
                ViewBag.maxPrice = list.Select(p => p.Price).Max();
            }
            else
            {
                var products = db.Products;
                ViewBag.minPrice = products.Min(p=>p.Price);
                ViewBag.maxPrice = products.Max(p=>p.Price);
            }

            if (title != "")
            {
                list = list.Where(p => p.Title.Contains(title)).ToList();
            }
            if (minPrice > 0)
            {
                list = list.Where(p => p.Price >= minPrice).ToList();
            }
            if (maxPrice > 0)
            {
                list = list.Where(p => p.Price <= maxPrice).ToList();
            }

            if (brandId > 0)
            {
                var _list = db.ProductBrand.Where(pb => pb.BrandID == brandId).Select(p => p.Products).Where(p => p.IsAcceptedByAdmin == true && p.IsActive == true).ToList();
                if (_list != null && _list.Any() && _list.Count() > 0)
                {
                    list = _list;
                }
                else
                {
                    ViewBag.NoBrand = "این برند هیچ محصولی ندارد!";
                }
            }


            list = list.Distinct().ToList();
            
            int take = 12;
            int skip = (pageId - 1) * take;
            ViewBag.Take = take;
            if (list != null)
            {
                ViewBag.ProductsCount = list.Count();
                ViewBag.PageCount = list.Count() / take + 1;
            }
            return View(list.OrderByDescending(p => p.CreateDate).Skip(skip).Take(take).ToList());
        }

        public ActionResult RelatedProducts(int id)
        {
            var gid = db.Product_Selected_Groups.Where(p => p.ProductID == id).Select(p => p.GroupID).Min();
            var products = db.Product_Selected_Groups.Where(p => p.GroupID == gid && p.ProductID != id).Select(p => p.Products).Where(p => p.IsActive == true && p.IsAcceptedByAdmin).Take(4);
            return PartialView(products);
        }


        public ActionResult ShowBrand()
        {
            var brands = db.Brands.Where(b => b.ProductBrand.Any()).Take(4).ToList();
            return PartialView(brands);
        }

        [Route("ShowAllBrands")]
        public ActionResult ShowAllBrands()
        {
            var brands = db.Brands.OrderByDescending(b => b.ProductBrand.Count()).ToList();
            return View(brands);
        }

        [Route("Product/QuickView/{id}")]
        public ActionResult QuickView(int id)
        {
            var product = db.Products.Find(id);
            return PartialView(product);
        }

        public ActionResult SpecialProducts()
        {
            return PartialView(db.SpecialProducts.Where(sp=>sp.ExpireDate>DateTime.Now&&sp.CreateDate<DateTime.Now&&sp.IsActive));
        }

        public ActionResult BestSellings()
        {
            var products = db.Products
                .Where(p => p.IsAcceptedByAdmin && p.IsActive && p.Stock > 0 && p.IsInBestselling)
                .OrderByDescending(p => p.CreateDate).Take(5);
            return PartialView(products);
        }

        public ActionResult BestSellingsInBlog()
        {
            var products = db.Products.AsNoTracking()
                .Where(p => p.IsAcceptedByAdmin && p.IsActive && p.Stock > 0 && p.IsInBestselling)
                .Select(p=> new BestSellingsInBlog {ProductID=p.ProductID, Title=p.Title,SefUrl=p.SefUrl,Image=p.ImageName,Price=p.Price,PriceAfterDiscount=p.PriceAfterDiscount, Date=p.CreateDate })
                .OrderByDescending(p => p.Date).Take(5);
            return PartialView(products);
        }

        [Route("Categories")]
        public ActionResult Categories()
        {
            return View();
        }

        public ActionResult CategoriesPartial(int? groupId, int? parentId)
        {
            var cats = db.Product_Groups.Where(g => g.ParentID == null);
            var products = db.Product_Selected_Groups.Where(p => p.GroupID == parentId).Select(p => p.Products).Where(p => p.IsActive && p.IsAcceptedByAdmin).OrderByDescending(p => p.CreateDate).Take(12).ToList();
            if (groupId != null && parentId == null)
            {
                cats = db.Product_Groups.Where(g => g.ParentID == null && g.GroupID == groupId);

            }
            else if (groupId == null && parentId != null)
            {
                cats = db.Product_Groups.Where(g => g.ParentID == parentId);
            }
            else if (groupId != null && parentId != null)
            {
                cats = db.Product_Groups.Where(g => g.ParentID == parentId && g.GroupID == groupId);
            }
            ViewBag.Products = products;
            ViewBag.ParentId = parentId;
            ViewBag.GroupId = groupId;
            return PartialView(cats.ToList());
        }
    }
}
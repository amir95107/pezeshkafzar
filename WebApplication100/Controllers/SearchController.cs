using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyEshop.Controllers
{
    public class SearchController : Controller
    {
        medab_DBEntities db = new medab_DBEntities();
        public ActionResult Index(string q, int pageId = 1)
        {
            if (q == null)
            {
                return RedirectToAction("MobileSearch");
            }
            List<Products> list = new List<Products>();

            if (Request.QueryString["ref"] == "tag")
            {
                list.AddRange(db.Product_Tags.Where(t => t.Tag == q).Select(t => t.Products).Where(p => p.IsActive && p.IsAcceptedByAdmin).ToList());
            }
            List<Products> products = db.Products.Where(p => p.IsActive && p.IsAcceptedByAdmin).ToList();
            list.AddRange(products.Where(p => p.Title.Contains(q) || p.ShortDescription.Contains(q) || p.Text.Contains(q) || p.ProductID.ToString() == q).ToList());
            var searched = q.Split(' ').Where(qq => qq.Length >= 4);
            if (!q.Split(' ').Any(s => s.Length >= 4))
            {
                searched = q.Split(' ');
            }
            foreach (var item in searched)
            {
                list.AddRange(products.Where(p => p.Title.Contains(item) || p.ShortDescription.Contains(item) || p.Text.Contains(item) || p.ProductID.ToString() == item).OrderByDescending(p => p.Visit).ToList());
            }
            
            ViewBag.search = q;
            int take = 12;
            int skip = (pageId - 1) * take;
            ViewBag.pageId = pageId;
            ViewBag.Take = take;
            if (list != null)
            {
                ViewBag.ProductsCount = list.Count();
                ViewBag.PageCount = list.Count() / take + 1;
            }
            return View(list.Distinct().Skip(skip).Take(take).ToList());
        }

        public ActionResult SearchSuggestion(string q)
        {
            List<Products> list = new List<Products>();
            List<Products> products = db.Products.Where(p => p.IsActive && p.IsAcceptedByAdmin).ToList();
            list.AddRange(products.Where(p => p.Title.Contains(q) || p.ShortDescription.Contains(q) || p.Text.Contains(q) || p.ProductID.ToString() == q).ToList());
            var searched = q.Split(' ').Where(qq => qq.Length >= 4);
            if (!q.Split(' ').Any(s => s.Length >= 4))
            {
                searched = q.Split(' ');
            }
            foreach (var item in searched)
            {
                list.AddRange(products.Where(p => p.Title.Contains(item) || p.ShortDescription.Contains(item) || p.Text.Contains(item)).ToList().OrderByDescending(p => p.Visit));
            }
            if (q == null || q == "" || q == " ")
            {
                list.Clear();
            }
            ViewBag.search = q;
            list = list.Distinct().ToList();
            ViewBag.Count = list.Count;
            return PartialView(list);
        }

        private int ProductBrand(int id)
        {
            var product = db.Products.Find(id);
            var brandId = product.ProductBrand.Single(pb => pb.ProductID == product.ProductID).BrandID;
            return brandId;
        }

        public ActionResult MobileSearch()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MobileSearchSuggestion(string q)
        {
            List<Products> list = new List<Products>();
            List<Products> products = db.Products.Where(p => p.IsActive && p.IsAcceptedByAdmin).ToList();
            list.AddRange(products.Where(p => p.Title.Contains(q) || p.ShortDescription.Contains(q) || p.Text.Contains(q) || p.ProductID.ToString() == q).ToList());
            var searched = q.Split(' ').Where(qq => qq.Length >= 4);
            if (!q.Split(' ').Any(s => s.Length >= 4))
            {
                searched = q.Split(' ');
            }
            foreach (var item in searched)
            {
                list.AddRange(products.Where(p => p.Title.Contains(item) || p.ShortDescription.Contains(item) || p.Text.Contains(item)).OrderByDescending(o => o.Visit));
            }

            if (q == null || q == "" || q == " ")
            {
                list.Clear();
            }
            ViewBag.search = q;
            ViewBag.Count = list.Count;
            return PartialView(list.Distinct().Where(p => p.IsActive));
        }

        public ActionResult LastSearchedProducts()
        {
            List<HttpCookie> list = new List<HttpCookie>();
            for (int i = Request.Cookies.Count - 1; i >= 0; i--)
            {
                if (list.Where(p => p.Name == Request.Cookies[i].Name).Any() == false)
                    list.Add(Request.Cookies[i]);
            }
            List<Products> lastSearched = new List<Products>();
            foreach (var item in list.Where(l => l.Name.StartsWith("SearchedProduct-")))
            {
                var product = db.Products.Find(int.Parse(item.Value));
                if (product != null)
                {
                    lastSearched.Add(product);
                }
            }

            return PartialView(lastSearched.Take(4));
        }

        public void SetLastSearchedCookie(int id)
        {
            HttpCookie cookie = new HttpCookie("SearchedProduct-" + id.ToString())
            {
                Expires = DateTime.Now.AddMonths(1),
                HttpOnly = true,
                Value = id.ToString()
            };
            Response.Cookies.Add(cookie);
        }
    }
}
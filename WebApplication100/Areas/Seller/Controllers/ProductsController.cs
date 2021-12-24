using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer;
using InsertShowImage;
using KooyWebApp_MVC.Classes;

namespace MyEshop.Areas.Seller.Controllers
{
    public class ProductsController : Controller
    {
        private medab_DBEntities db = new medab_DBEntities();

        public int SellerId()
        {
            string user = User.Identity.Name;
            int sellerId = db.Sellers.Single(s => s.Users.Mobile == user).SellerID;
            return sellerId;
        }

        public bool IsAccepted()
        {
            int sellerId = SellerId();
            bool isAccepted = false;
            if (db.Sellers.Find(sellerId).IsAcceptedByAdmin)
            {
                isAccepted = true;
            }
            return isAccepted;
        }
        // GET: Admin/Products
        public ActionResult Index()
        {
            //bool isAccepted = IsAccepted();
            //if (!isAccepted)
            //{
            //    return Redirect("/Seller");
            //}
            int sellerId = SellerId();
            return View(db.Products.Where(p=>p.SellerID == sellerId).ToList());
        }

        
        
        
        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            bool isAccepted = IsAccepted();
            if (!isAccepted)
            {
                return Redirect("/Seller");
            }
            ViewBag.Groups = db.Product_Groups.ToList();
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Title,ShortDescription,Text,Price,ImageName,CreateDate,SellerID,PriceAfterDiscount,LikeCount,Stock,Point,IsAcceptedByAdmin")] Products products, List<int> selectedGroups, HttpPostedFileBase imageProduct, string tags)
        {
            if (ModelState.IsValid)
            {
                if (selectedGroups == null)
                {
                    ViewBag.ErrorSelectedGroup = true;
                    ViewBag.Groups = db.Product_Groups.ToList();
                    return View(products);
                }
                products.ImageName = "images.jpg";
                if (imageProduct != null && imageProduct.IsImage())
                {
                    products.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageProduct.FileName);
                    imageProduct.SaveAs(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + products.ImageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + products.ImageName));
                }
                products.CreateDate = DateTime.Now;
                products.LikeCount = 0;
                products.Point = 0;
                products.SellerID = SellerId();
                products.IsAcceptedByAdmin = false;

                db.Products.Add(products);

                foreach (int selectedGroup in selectedGroups)
                {
                    db.Product_Selected_Groups.Add(new Product_Selected_Groups()
                    {
                        ProductID = products.ProductID,
                        GroupID = selectedGroup
                    });
                }

                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tag = tags.Split(',');
                    foreach (string t in tag)
                    {
                        db.Product_Tags.Add(new Product_Tags()
                        {
                            ProductID = products.ProductID,
                            Tag = t.Trim()
                        });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Groups = db.Product_Groups.ToList();
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }

            int sellerId = SellerId();

            if(db.Products.Where(p=>p.SellerID == sellerId).Any(p=>p.ProductID == products.ProductID))
            {
                ViewBag.SelectedGroups = products.Product_Selected_Groups.ToList();
                ViewBag.Groups = db.Product_Groups.ToList();
                ViewBag.Tags = string.Join(",", products.Product_Tags.Select(t => t.Tag).ToList());
                return View(products);
            }

            ViewBag.YouCant = "شما اجازه ویرایش این محصول را ندارید!!";
            return RedirectToAction("Index", db.Products.Where(p => p.SellerID == SellerId()).ToList());
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,Title,ShortDescription,Text,Price,ImageName,CreateDate,UserName,PriceAfterDiscount,LikeCount,Stock,Point,IsAcceptedByAdmin")] Products products, List<int> selectedGroups, HttpPostedFileBase imageProduct, string tags)
        {
            if (ModelState.IsValid)
            {
                if (imageProduct != null && imageProduct.IsImage())
                {
                    if (products.ImageName != "images.jpg")
                    {
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                        System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + products.ImageName));
                    }

                    products.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imageProduct.FileName);
                    imageProduct.SaveAs(Server.MapPath("/Images/ProductImages/" + products.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + products.ImageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + products.ImageName));
                }
                db.Entry(products).State = EntityState.Modified;


                db.Product_Tags.Where(t => t.ProductID == products.ProductID).ToList().ForEach(t => db.Product_Tags.Remove(t));
                if (!string.IsNullOrEmpty(tags))
                {
                    string[] tag = tags.Split(',');
                    foreach (string t in tag)
                    {
                        db.Product_Tags.Add(new Product_Tags()
                        {
                            ProductID = products.ProductID,
                            Tag = t.Trim()
                        });
                    }
                }


                db.Product_Selected_Groups.Where(g => g.ProductID == products.ProductID).ToList().ForEach(g => db.Product_Selected_Groups.Remove(g));
                if (selectedGroups != null && selectedGroups.Any())
                {
                    foreach (int selectedGroup in selectedGroups)
                    {
                        db.Product_Selected_Groups.Add(new Product_Selected_Groups()
                        {
                            ProductID = products.ProductID,
                            GroupID = selectedGroup
                        });
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SelectedGroups = selectedGroups;
            ViewBag.Groups = db.Product_Groups.ToList();
            ViewBag.Tags = tags;
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            if (db.Products.Where(p => p.SellerID == SellerId()).Any(p => p.ProductID == products.ProductID))
            {
                return View(products);
            }
            ViewBag.YouCant = "شما اجازه حذف این محصول را ندارید!!";
            return RedirectToAction("Index", db.Products.Where(p => p.SellerID == SellerId()).ToList());
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        #region Gallery

        public ActionResult Gallery(int id)
        {
            ViewBag.Galleries = db.Product_Galleries.Where(p => p.ProductID == id).ToList();
            return View(new Product_Galleries()
            {
                ProductID = id
            });
        }

        [HttpPost]
        public ActionResult Gallery(Product_Galleries galleries, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    galleries.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/ProductImages/" + galleries.ImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + galleries.ImageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + galleries.ImageName));
                    db.Product_Galleries.Add(galleries);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Gallery", new { id = galleries.ProductID });
        }

        public ActionResult DeleteGallery(int id)
        {
            var gallery = db.Product_Galleries.Find(id);

            System.IO.File.Delete(Server.MapPath("/Images/ProductImages/" + gallery.ImageName));
            System.IO.File.Delete(Server.MapPath("/Images/ProductImages/Thumb/" + gallery.ImageName));

            db.Product_Galleries.Remove(gallery);
            db.SaveChanges();
            return RedirectToAction("Gallery", new { id = gallery.ProductID });
        }

        #endregion


        #region  Featurs

        public ActionResult ProductFeaturs(int id)
        {
            ViewBag.Product = db.Products.Find(id);
            ViewBag.Features = db.Product_Features.Where(f => f.ProductID == id).ToList();
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureTitle");
            return View(new Product_Features()
            {
                ProductID = id
            });
        }

        [HttpPost]
        public ActionResult ProductFeaturs(Product_Features feature)
        {
            if (ModelState.IsValid)
            {
                db.Product_Features.Add(feature);
                db.SaveChanges();
            }

            return RedirectToAction("ProductFeaturs", new { id = feature.ProductID });
        }

        public void DeleteFeature(int id)
        {
            var feature = db.Product_Features.Find(id);
            db.Product_Features.Remove(feature);
            db.SaveChanges();
        }

        public ActionResult Brands()
        {
            ViewBag.Brands = db.Brands.ToList();
            return View(new Brands());
        }

        [HttpPost]
        public ActionResult CreateBrands(Brands brand, HttpPostedFileBase imgUp)
        {
            if (true)
            {
                if (imgUp != null && imgUp.IsImage())
                {
                    brand.BrandImageName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/Images/ProductImages/" + brand.BrandImageName));
                    ImageResizer img = new ImageResizer();
                    img.Resize(Server.MapPath("/Images/ProductImages/" + brand.BrandImageName),
                        Server.MapPath("/Images/ProductImages/Thumb/" + brand.BrandImageName));
                    db.Brands.Add(brand);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Brands", new Brands());
        }

        public ActionResult DeleteBrand(int id)
        {
            var brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
            var brands = db.ProductBrand.Where(pb => pb.BrandID == id);
            foreach(var item in brands)
            {
                db.ProductBrand.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Brands", new Brands());
        }

        public ActionResult ProductBrand(int id)
        {
            var product = db.Products.Single(p => p.ProductID == id);
            ViewBag.Brands = db.Brands.ToList();
            return View(product);
        }

        [HttpPost]
        public ActionResult SetProductBrand(ProductBrand brand)
        {
            if (ModelState.IsValid)
            {
                db.ProductBrand.Add(brand);
                db.SaveChanges();
            }

            return RedirectToAction("ProductBrand", new { id = brand.ProductID });
        }

        public ActionResult EditProductBrand(int id)
        {
            ViewBag.Brands = db.Brands.ToList();
            var product = db.Products.Single(p => p.ProductID == id);
            return PartialView(product);
        }

        [HttpPost]
        public ActionResult EditProductBrand(ProductBrand brand)
        {

            if (ModelState.IsValid)
            {

                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("ProductBrand", new { id = brand.ProductID });
        }

        #endregion

        //public ActionResult SpecialProducts()
        //{
        //    return View(db.SpecialProducts);
        //}

        //public ActionResult CreateSpecialProduct()
        //{
        //    ViewBag.Products = db.Products.Where(p=>p.Stock > 0).ToList();

        //    return View();
        //}

        //[HttpPost]
        //public ActionResult CreateSpecialProduct(SpecialProducts sp)
        //{
        //    db.SpecialProducts.Add(sp);
        //    db.SaveChanges();
        //    return RedirectToAction("SpecialProducts", db.SpecialProducts.ToList());
        //}

        //public ActionResult DeleteSepcialProduct(int id)
        //{
        //    var sp = db.SpecialProducts.Find(id);
        //    db.SpecialProducts.Remove(sp);
        //    db.SaveChanges();
        //    return RedirectToAction("SpecialProducts", db.SpecialProducts);
        //}

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

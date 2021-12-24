using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using DataLayer;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using RouteAttribute = System.Web.Mvc.RouteAttribute;
using AcceptVerbsAttribute = System.Web.Mvc.AcceptVerbsAttribute;

namespace WebApplication100.Areas.Admin.Controllers
{
    public class AdminShopController : ApiController
    {
        // GET: api/Shop
        public int Get()
        {
            List<DataLayer.ViewModels.ShopCartItem> list = new List<DataLayer.ViewModels.ShopCartItem>();
            var sessions = HttpContext.Current.Session;
            if (sessions["AdminShopCart"] != null)
            {
                list = sessions["AdminShopCart"] as List<DataLayer.ViewModels.ShopCartItem>;
            }
            return list.Sum(l => l.Count);
        }

        // GET: api/Shop/5
        public int Get(int id)
        {
            List<DataLayer.ViewModels.ShopCartItem> list = new List<DataLayer.ViewModels.ShopCartItem>();
            var sessions = HttpContext.Current.Session;
            if (sessions["AdminShopCart"] != null)
            {
                list = sessions["AdminShopCart"] as List<DataLayer.ViewModels.ShopCartItem>;
            }
            if (list.Any(p => p.ProductID == id))
            {
                int index = list.FindIndex(p => p.ProductID == id);
                list[index].Count++;
            }
            else
            {
                list.Add(new DataLayer.ViewModels.ShopCartItem()
                {
                    ProductID = id,
                    Count = 1
                });
            }

            sessions["AdminShopCart"] = list;

            return Get();
        }


    }
}

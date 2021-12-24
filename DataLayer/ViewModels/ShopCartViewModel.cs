using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ShopCartItem
    {
        public int ProductID { get; set; }
        public int Count { get; set; }
    }

    public class ShopCartItemViewModel
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }
        public int PriceAfterDiscount { get; set; }
        public string SefUrl { get; set; }

        public int Sum { get; set; }
    }

    public class ShowOrderViewModel
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int PriceAfterDiscount { get; set; }
        public int Discount { get; set; }
        public int Sum { get; set; }
        public string SefUrl { get; set; }

    }

    public class DiscountViewModel
    {
        public int Percent { get; set; }
        public string code { get; set; }
    }

    public class PaymentViewModel
    {
        public string merchant_id { get; set; }
        public int amount { get; set; }
        public string description { get; set; }
        public string callback_url { get; set; }
    }

    public class SendingWaysViewModel
    {
        public int Title { get; set; }
        public int Price { get; set; }
    }
}

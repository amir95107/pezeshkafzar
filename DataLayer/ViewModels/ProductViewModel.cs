using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ShowProductFeatureViewModel
    {
        public string FeatureTitle { get; set; }
        public List<string> Values { get; set; }
    }

    public class LastSeen
    {
        public Products product { get; set; }
    }

    public class BestSellingsInBlog
    {
        public int ProductID { get; set; }
        public string Title { get; set; }
        public string SefUrl { get; set; }
        public string Image { get; set; }
        public int PriceAfterDiscount { get; set; }
        public int Price { get; set; }
        public DateTime Date { get; set; }

    }

}

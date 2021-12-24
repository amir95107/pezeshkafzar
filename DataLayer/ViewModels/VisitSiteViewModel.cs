using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class VisitSiteViewModel
    {
        public int VisitToday { get; set; }
        public int VisitYesterdaye { get; set; }
        public int VisitSum { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DataLayer.MetaDataClasses
{
    public class AddressMetaData
    {
        [AllowHtml]
        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "طول جغرافیایی")]
        public Nullable<double> Long { get; set; }

        [Display(Name = "عرض جغرافیایی")]
        public Nullable<double> Lat { get; set; }

        [Display(Name = "آدرس پیشفرض")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public bool IsDefault { get; set; }

        [Display(Name = "استان")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string State { get; set; }

        [Display(Name = "شهر")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string City { get; set; }

        [Display(Name = "کد پستی")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string PostalCode { get; set; }

        public virtual Users Users { get; set; }
    }
}

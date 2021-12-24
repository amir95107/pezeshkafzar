using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DataLayer
{
    public class PageMetaData
    {
        [Key]
        public int PageID { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50)]
        public string PageTitle { get; set; }

        [AllowHtml]
        [Display(Name = "محتوا")]
        public string PageContent { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100)]
        public string HeadTitle { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100)]
        public string Url { get; set; }
    }
    [MetadataType(typeof(PageMetaData))]
    public partial class Page
    {

    }
}
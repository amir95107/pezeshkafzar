//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    internal class UserInfoMetaData
    {
        [Display(Name = "نام کامل")]
        [Required(ErrorMessage = "لطفا {0} وارد کنید")]
        public string FullName { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "تلفن ثابت")]
        public string Telephone { get; set; }

    }
    [MetadataType(typeof(UserInfoMetaData))]
    public partial class UserInfo
    {

    }
}
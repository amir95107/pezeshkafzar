//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            this.LikeProduct = new HashSet<LikeProduct>();
            this.OrderDetails = new HashSet<OrderDetails>();
            this.Product_Features = new HashSet<Product_Features>();
            this.Product_Galleries = new HashSet<Product_Galleries>();
            this.Product_Selected_Groups = new HashSet<Product_Selected_Groups>();
            this.Product_Tags = new HashSet<Product_Tags>();
            this.ProductBrand = new HashSet<ProductBrand>();
            this.LoggedCart = new HashSet<LoggedCart>();
            this.SpecialProducts = new HashSet<SpecialProducts>();
            this.Comments = new HashSet<Comments>();
        }
    
        public int ProductID { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public int Price { get; set; }
        public int PriceAfterDiscount { get; set; }
        public string ImageName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int LikeCount { get; set; }
        public int Stock { get; set; }
        public Nullable<double> Point { get; set; }
        public Nullable<int> SellerID { get; set; }
        public bool IsAcceptedByAdmin { get; set; }
        public bool IsActive { get; set; }
        public string SefUrl { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public Nullable<int> Visit { get; set; }
        public string Garanty { get; set; }
        public Nullable<int> MinCount { get; set; }
        public bool IsInBestselling { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LikeProduct> LikeProduct { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Features> Product_Features { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Galleries> Product_Galleries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Selected_Groups> Product_Selected_Groups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product_Tags> Product_Tags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductBrand> ProductBrand { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoggedCart> LoggedCart { get; set; }
        public virtual Products Products1 { get; set; }
        public virtual Products Products2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialProducts> SpecialProducts { get; set; }
        public virtual Sellers Sellers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comments> Comments { get; set; }
    }
}

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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class medab_DBEntities : DbContext
    {
        public medab_DBEntities()
            : base("name=medab_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Addresses> Addresses { get; set; }
        public virtual DbSet<Blogs> Blogs { get; set; }
        public virtual DbSet<Brands> Brands { get; set; }
        public virtual DbSet<ContactForm> ContactForm { get; set; }
        public virtual DbSet<DeliveryWays> DeliveryWays { get; set; }
        public virtual DbSet<Discounts> Discounts { get; set; }
        public virtual DbSet<Features> Features { get; set; }
        public virtual DbSet<Lead_Clients> Lead_Clients { get; set; }
        public virtual DbSet<LikeProduct> LikeProduct { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Product_Features> Product_Features { get; set; }
        public virtual DbSet<Product_Galleries> Product_Galleries { get; set; }
        public virtual DbSet<Product_Groups> Product_Groups { get; set; }
        public virtual DbSet<Product_Selected_Groups> Product_Selected_Groups { get; set; }
        public virtual DbSet<Product_Tags> Product_Tags { get; set; }
        public virtual DbSet<ProductBrand> ProductBrand { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SiteVisit> SiteVisit { get; set; }
        public virtual DbSet<Slider> Slider { get; set; }
        public virtual DbSet<SpecialProducts> SpecialProducts { get; set; }
        public virtual DbSet<LoggedCart> LoggedCart { get; set; }
        public virtual DbSet<Page> Page { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Sellers> Sellers { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
    }
}

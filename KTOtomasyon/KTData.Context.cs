﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KTOtomasyon
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class KTOtomasyonEntities : DbContext
    {
        public KTOtomasyonEntities()
            : base("name=KTOtomasyonEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Operations> Operations { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<vCustomers> vCustomers { get; set; }
        public virtual DbSet<vOrders> vOrders { get; set; }
        public virtual DbSet<vTotalOrder> vTotalOrder { get; set; }
        public virtual DbSet<Mails> Mails { get; set; }
        public virtual DbSet<vTodayTotalOrder> vTodayTotalOrder { get; set; }
        public virtual DbSet<vLastTotalOrder> vLastTotalOrder { get; set; }
    
        public virtual ObjectResult<AYLIKSIPARISRAPOR_Result> AYLIKSIPARISRAPOR()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<AYLIKSIPARISRAPOR_Result>("AYLIKSIPARISRAPOR");
        }
    
        public virtual ObjectResult<Nullable<decimal>> ILKPROS(Nullable<System.DateTime> firstodate, Nullable<System.DateTime> lastodate)
        {
            var firstodateParameter = firstodate.HasValue ?
                new ObjectParameter("firstodate", firstodate) :
                new ObjectParameter("firstodate", typeof(System.DateTime));
    
            var lastodateParameter = lastodate.HasValue ?
                new ObjectParameter("lastodate", lastodate) :
                new ObjectParameter("lastodate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<decimal>>("ILKPROS", firstodateParameter, lastodateParameter);
        }
    }
}

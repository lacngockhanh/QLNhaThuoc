//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TH.Agular.UserManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shop()
        {
            this.Services = new HashSet<Service>();
            this.ServiceTypes = new HashSet<ServiceType>();
            this.Users = new HashSet<User>();
            this.Customers = new HashSet<Customer>();
            this.Receipts = new HashSet<Receipt>();
        }
    
        public string ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhone { get; set; }
        public Nullable<int> OrderIndex { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> DateCreate { get; set; }
        public bool IsDelete { get; set; }
        public Nullable<System.DateTime> DateDelete { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Service> Services { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceType> ServiceTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Customer> Customers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}

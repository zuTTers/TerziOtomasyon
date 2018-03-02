using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KTOtomasyon.Models
{
    public class DisplayOrderDetail
    {

        public IList<Orders> Orders { get; set; } 
        public int TotalPrice { get; set; }
        public string CreatedUserName { get; set; }

        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
    }

    public class OrderWithDetail
    {
        public int Order_Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public System.DateTime OrderDate { get; set; }
        public int CreatedUser { get; set; }
        public string CreatedUserText { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        public int OrderDetail_Id { get; set; }
        public Nullable<int> Order_Id { get; set; }
        public Nullable<int> Operation_Id { get; set; }
        public string OperationText { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }         
    }
}
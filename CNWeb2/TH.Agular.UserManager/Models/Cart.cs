using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TH.Agular.UserManager.Models
{
    public class Cart
    {
        public string CustomerId;
        public string CustomerName;
        public string Address;
        public string Email;
        public string Phone;
        public string Description;
        public decimal TotalMoney;
        public List<CartItem> ListCartItem = new List<CartItem>();
    }
    public class CartItem
    {
        public string ServiceId;
        public int OrderIndex;
        public string Name;
        public string ShopId;
        public string ShopName;        
        public string Unit;
        public decimal Quantity;
        public decimal Price;
    }
}
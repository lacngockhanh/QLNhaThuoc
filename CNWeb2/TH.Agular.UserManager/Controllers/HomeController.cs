using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;

namespace TH.Agular.UserManager.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Shops()
        {
            return View();
        }
        public ActionResult ShopDetail(string id)
        {
            return View();
        }

        public ActionResult AddToCart(string ShopId, string ServiceId, string ReturnPage, string Search, string OrderBy, decimal Quantity)
        {
            if (Quantity <= 0)
            {
                return Content("<script>alert('Số lượng phải lớn hơn 0, vui lòng kiểm tra lại'); history.back();</script>");
            }
            UserManagerEntities db = new UserManagerEntities();
            Service service = db.Services.Find(ServiceId);             
            Cart cart;
            if (Session["Cart"] != null)
            {
                cart = Session["Cart"] as Cart;
            } else
            {
                cart = new Cart();                 
            }
            
            foreach (var i in cart.ListCartItem)
            {
                if (i.ServiceId == ServiceId)
                {
                    return Content("<script>alert('Sản phẩm đã thêm rồi, vui lòng kiểm tra lại'); history.back();</script>");
                }
            }

            CartItem item = new CartItem();
            item.ServiceId = service.ServiceId;
            item.Name = service.Name;
            item.ShopName = service.Shop.Name;
            item.ShopId = service.ShopId;
            item.Unit = service.Unit;
            item.Quantity = Quantity;
            item.Price = service.Price;
            int count = cart.ListCartItem.Count + 1;
            item.OrderIndex = 1;            
            cart.ListCartItem.Add(item);                               
            Session["Cart"] = cart;
            if (ReturnPage == "search")
            {
                return RedirectToAction("Index", new { search = Search, orderby = OrderBy });
            } else return RedirectToAction("ShopDetail", new { id = ShopId });
        }


        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult DestroyCart()
        {
            Session.Remove("Cart");
            return RedirectToAction("Shops", "Home");
        }
        public ActionResult Order()
        {
            return View();
        }

        public ActionResult UpdateQuantity(string ServiceId, decimal Quantity)
        {
            
            if (Quantity <= 0)
            {
                return Content("<script>alert('Số lượng phải lớn hơn 0, vui lòng kiểm tra lại'); history.back();</script>");
            }
            UserManagerEntities db = new UserManagerEntities();
            Cart cart;
            if (Session["Cart"] != null)
            {
                cart = Session["Cart"] as Cart;
                CartItem item = cart.ListCartItem.Where(x => x.ServiceId == ServiceId).FirstOrDefault();
                item.Quantity = Quantity;
                Session["Cart"] = cart;
                return RedirectToAction("Cart", "Home");
            }
            else
            {
                return Content("<script>alert('Phiên làm việc hết hạn, vui lòng thêm giỏ hàng lại'); history.back();</script>");
            }
        }
        public ActionResult RemoveItem(string ServiceId)
        {
            UserManagerEntities db = new UserManagerEntities();
            Cart cart;
            if (Session["Cart"] != null)
            {
                cart = Session["Cart"] as Cart;
                CartItem item = cart.ListCartItem.Where(x => x.ServiceId == ServiceId).FirstOrDefault();
                cart.ListCartItem.Remove(item);
                Session["Cart"] = cart;
                return RedirectToAction("Cart", "Home");
            }
            else
            {
                return Content("<script>alert('Phiên làm việc hết hạn, vui lòng thêm giỏ hàng lại'); history.back();</script>");
            }
        }

        public ActionResult FinishOrder(string FullName, string Address, string Phone, string Email, string Description)
        {
            UserManagerEntities db = new UserManagerEntities();
            Cart cart = new Cart();
            if (Session["Cart"] != null)
            {
                try
                {
                    cart = Session["Cart"] as Cart;
                    var listDistinctShopId = cart.ListCartItem.Select(x => x.ShopId).Distinct();
                    foreach (var shopId in listDistinctShopId)
                    {
                        Customer customer = new Customer();
                        customer.CustomerId = Guid.NewGuid().ToString().ToLower();
                        customer.ShopId = shopId;
                        customer.Phone = Phone;
                        customer.FullName = FullName;
                        customer.Address = Address;
                        customer.Email = Email;
                        customer.DateCreate = DateTime.Now;
                        customer.IsDelete = false;
                        db.Customers.Add(customer);
                        db.SaveChanges();
                        Receipt receipt = new Receipt();
                        receipt.CustomerId = customer.CustomerId;
                        receipt.DateCreate = DateTime.Now;
                        receipt.Description = Description;
                        receipt.GUID = Guid.NewGuid().ToString().ToLower();
                        receipt.DateCreate = DateTime.Now;
                        receipt.DateUpdate = DateTime.Now;
                        receipt.ReceiptStatusId = 1;
                        receipt.ReceiptTypeId = 1;
                        receipt.ShopId = shopId;
                        db.Receipts.Add(receipt);
                        db.SaveChanges();
                        receipt.ReceiptNumber = "HD" + DateTime.Now.ToString("yyMMdd") + "-" + receipt.ReceiptId;

                        var listCartItems = cart.ListCartItem.OrderBy(x => x.ShopId).Where(x => x.ShopId == shopId).ToList();
                        int count = 0;
                        decimal totalMoney = 0;
                        foreach (var item in listCartItems)
                        {
                            count++;
                            totalMoney += item.Quantity * item.Price;
                            ReceiptService rs = new ReceiptService();
                            rs.DateCreate = DateTime.Now;
                            rs.ItemName = item.Name;
                            rs.OrderIndex = count;
                            rs.Price = item.Price;
                            rs.Quantity = item.Quantity;
                            rs.ReceiptId = receipt.ReceiptId;
                            rs.ServiceId = item.ServiceId;
                            rs.Unit = item.Unit;
                            db.ReceiptServices.Add(rs);
                        }
                        receipt.TotalMoney = totalMoney;
                        db.SaveChanges();
                    }
                    Session.Remove("cart");
                    return Content("ok");
                }
                catch (Exception ex) {
                    return Content(ex.Message);
                }
                
            }
            else
            {
                return Content("false");
            }
            
            

            
        }
    }
}
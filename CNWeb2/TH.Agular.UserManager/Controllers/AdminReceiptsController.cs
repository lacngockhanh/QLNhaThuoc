using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;

namespace TH.Agular.UserManager.Controllers
{
    public class AdminReceiptsController : Controller
    {
        // GET: AdminReceipts
        public ActionResult Index(string from, string to)
        {
            
            return View();
        }
        public ActionResult New(string type, string shop)
        {
            type = type.ToLower();
            if (type == "hd" || type == "pn" || type == "pt" || type == "pc")
            {
                int typeId = 1;
                if (type == "hd") typeId = 1;
                if (type == "pn") typeId = 2;
                if (type == "pt") typeId = 3;
                if (type == "pc") typeId = 4;
                UserManagerEntities db = new UserManagerEntities();
                Receipt r = new Receipt();
                r.GUID = Guid.NewGuid().ToString().ToLower();
                r.DateCreate = DateTime.Now;
                r.DateUpdate = DateTime.Now;
                r.ReceiptStatusId = 5;
                r.ReceiptTypeId = typeId;
                r.ShopId = shop;
                db.Receipts.Add(r);
                db.SaveChanges();
                r.ReceiptNumber = type.ToUpper() + DateTime.Now.ToString("yyMMdd") + "-" + r.ReceiptId;
                db.SaveChanges();
                return RedirectToAction("Detail", new { id = r.ReceiptId, guid = r.GUID} );
            } else
            {
                return Content("<script>alert('Đường dẫn không hợp lệ');</script>");
            }
            
        }
        public ActionResult Detail(int id, string guid)
        {
            UserManagerEntities db = new UserManagerEntities();
            Receipt r = db.Receipts.Find(id);
            if (guid == null || guid == "" || r.GUID != guid)
            { 
                return Content("<script>alert('Đường dẫn không hợp lệ');</script>");
            }
            return View(r);
        }

        [HttpPost]
        public JsonResult LoadListReceiptServices(int ReceiptId)
        {
            
            UserManagerEntities db = new UserManagerEntities();
            var listReceiptServices = db.ReceiptServices.Select(x => new { x.ItemName, x.OrderIndex, x.Quantity, x.Price, x.Unit, x.ReceiptServiceId, x.ReceiptId }).Where(x => x.ReceiptId == ReceiptId).ToList();
            return Json(listReceiptServices);
        }
        [HttpPost]
        public ActionResult CheckAvailableReceiptServices(int ReceiptId, string ServiceId)
        {
            UserManagerEntities db = new UserManagerEntities();
            var list = db.ReceiptServices.Where(x => x.ReceiptId == ReceiptId && x.ServiceId == ServiceId).ToList();
            if (list.Any())
            {
                return Content("true");
            } else
            {
                return Content("false");
            }
        }
        [HttpPost]
        public ActionResult ReceiptAddItem(int ReceiptId, string ServiceId)
        {            
            UserManagerEntities db = new UserManagerEntities();
            Service s = db.Services.Find(ServiceId);
            ReceiptService rs = new ReceiptService();
            rs.ReceiptId = ReceiptId;
            rs.ServiceId = ServiceId;
            rs.ItemName = s.Name;
            rs.Price = s.Price;
            rs.Unit = s.Unit;
            rs.Quantity = 1;            
            rs.OrderIndex = 1;
            rs.DateCreate = DateTime.Now;
            db.ReceiptServices.Add(rs);
            db.SaveChanges();
            Receipt r = db.Receipts.Find(ReceiptId);
            r.TotalMoney = r.ReceiptServices.Sum(x => (x.Quantity * x.Price));
            db.SaveChanges();
            return Content("true");        
        }
        [HttpPost]
        public ActionResult RemoveItem(int ReceiptServiceId)
        {
            UserManagerEntities db = new UserManagerEntities();
            ReceiptService rs = db.ReceiptServices.Find(ReceiptServiceId);
            int receiptId = rs.ReceiptId;
            db.ReceiptServices.Remove(rs);
            db.SaveChanges();
            Receipt r = db.Receipts.Find(receiptId);
            r.TotalMoney = r.ReceiptServices.Sum(x => (x.Quantity * x.Price));
            db.SaveChanges();
            return Content("true");
        }
        [HttpPost]
        public ActionResult UpdateQuantity(int ReceiptServiceId, int Quantity)
        {
            UserManagerEntities db = new UserManagerEntities();
            ReceiptService rs = db.ReceiptServices.Find(ReceiptServiceId);
            rs.Quantity = Quantity;
            int receiptId = rs.ReceiptId;
            db.SaveChanges();
            Receipt r = db.Receipts.Find(receiptId);
            r.TotalMoney = r.ReceiptServices.Sum(x => (x.Quantity * x.Price));
            db.SaveChanges();
            return Content("true");
        }
        public ActionResult UpdateStatus(int id, int status, string guid, string from, string to)
        {
            UserManagerEntities db = new UserManagerEntities();
            Receipt r = db.Receipts.Find(id);
            if (guid == null || guid == "" || r.GUID != guid)
            {
                return Content("<script>alert('Đường dẫn không hợp lệ');</script>");
            }
            r.ReceiptStatusId = status;
            db.SaveChanges();
            return RedirectToAction("Index", "AdminReceipts", new { @from = from, @to = to});
        }
        [HttpPost]
        public ActionResult UpdateCustomerId(int ReceiptId, string CustomerId)
        {
            UserManagerEntities db = new UserManagerEntities();
            Receipt r = db.Receipts.Find(ReceiptId);
            r.CustomerId = CustomerId;
            db.SaveChanges();
            return Content("true");
        }
        [HttpPost]
        public JsonResult GetReceiptCustomer(int ReceiptId)
        {
            
                UserManagerEntities db = new UserManagerEntities();
                var receipt = db.Receipts.Find(ReceiptId);
                if (receipt.Customer != null)
                {
                    var customer = receipt.Customer;
                    var c = new 
                    {
                        CustomerId = customer.CustomerId,
                        Address = customer.Address,
                        FullName = customer.FullName,
                        Email = customer.Email,
                        Phone = customer.Phone
                    };
                    return Json(c);
                } else
                {
                    var c = new
                    {
                        CustomerId = "",
                        Address = "",
                        FullName = "",
                        Email = "",
                        Phone = ""
                    };
                    return Json(c);
                }
                
            
                   
        }

        [HttpPost]
        public JsonResult GetReceipt(int ReceiptId)
        {
            UserManagerEntities db = new UserManagerEntities();
            var r = db.Receipts.Where(x => x.ReceiptId == ReceiptId).Select(x => new { x.ReceiptId, x.ReceiptNumber, x.ReceiptStatusId, x.ReceiptTypeId, x.Description, x.DateCreate, x.UserId, UserFullName = x.User.FullName}).FirstOrDefault();
            return Json(r);
        }
        [HttpPost]
        public ActionResult InsertCustomer(
                                            string FullName,
                                            string Address,
                                            string Phone,
                                            string Email
                                         )
        {
            UserManagerEntities db = new UserManagerEntities();
            Customer customer = new Customer();
            customer.CustomerId = Guid.NewGuid().ToString().ToLower();
            customer.FullName = FullName;
            customer.Address = Address;
            customer.Phone = Phone;
            customer.Email = Email;
            customer.DateCreate = DateTime.Now;
            customer.IsDelete = false;
            customer.DateDelete = null;
            db.Customers.Add(customer);
            db.SaveChanges();
            return Content(customer.CustomerId);
        }
        [HttpPost]
        public ActionResult UpdateCustomer(string CustomerId,
                                            string FullName,
                                            string Address,
                                            string Phone,
                                            string Email)
        {
            UserManagerEntities db = new UserManagerEntities();
            Customer customer = db.Customers.Find(CustomerId);
            customer.FullName = FullName;
            customer.Address = Address;
            customer.Phone = Phone;
            customer.Email = Email;
            db.SaveChanges();
            return Content(customer.CustomerId);
        }
        [HttpPost]
        public ActionResult UpdateReceipt(int ReceiptId, string Description, string UserId)
        {
            UserManagerEntities db = new UserManagerEntities();
            Receipt r = db.Receipts.Find(ReceiptId);
            r.Description = Description;
            r.DateUpdate = DateTime.Now;
            r.UserId = UserId;
            r.ReceiptStatusId = 1;
            db.SaveChanges();
            return Content("true");
        }
    }
}
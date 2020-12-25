using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;

namespace TH.Agular.UserManager.Controllers
{
    public class AdminCustomersController : Controller
    {
        // GET: AdminCustomers

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _pDetail()
        {
            return View();
        }
        public ActionResult _pList()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetListCustomers()
        {
            UserManagerEntities db = new UserManagerEntities();
            CurrentUser currentUser = new CurrentUser();
            if (Session["CurrentUser"] != null)
            {
                currentUser = Session["CurrentUser"] as CurrentUser;
            }

            var listCustomers = db.Customers.Where(x => x.IsDelete != true).Select(x => new { x.CustomerId, x.FullName, x.Address, x.Phone, x.DateCreate, x.ShopId }).OrderByDescending(x => x.DateCreate).ToList();
            if (currentUser.Permits.Where(x => x.RoleId == 2).ToList().Any())
            {
                listCustomers = listCustomers.Where(x => x.ShopId == currentUser.ShopId).ToList();
            }            
            return Json(listCustomers);
        }
        [HttpPost]
        public JsonResult CheckAvailableCustomerName(string FullName, string mode)
        {
            UserManagerEntities db = new UserManagerEntities();
            if (mode == "Insert")
            {
                var list = db.Customers.Where(x => x.FullName == FullName && x.IsDelete != true).ToList();
                if (list.Any()) return Json(true); else return Json(false);
            }
            else
            {
                return Json(false);
            }

        }
        [HttpPost]
        public JsonResult GetCustomer(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            var customer = db.Customers.Where(x => x.CustomerId == id).Select(x => new { x.CustomerId, x.Address, x.Email, x.FullName, x.Phone}).First();
            return Json(customer);
        }
        [HttpPost]
        public JsonResult InsertCustomer(
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
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateCustomer(string CustomerId,                                             
                                            string FullName,
                                            string Address,
                                            string Phone, string Email)
        {
            UserManagerEntities db = new UserManagerEntities();
            Customer customer = db.Customers.Find(CustomerId);
            customer.FullName = FullName;
            customer.Address = Address;
            customer.Phone = Phone;
            customer.Email = Email;
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult DeleteCustomer(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            Customer customer = db.Customers.Find(id);
            customer.IsDelete = true;
            db.SaveChanges();
            return Json(true);
        }
    }
}
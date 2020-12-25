using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;

namespace TH.Agular.UserManager.Controllers
{
    public class AdminServicesController : Controller
    {
        // GET: AdminServices
        
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
        public JsonResult GetListServices()
        {
            UserManagerEntities db = new UserManagerEntities();
            TH.Agular.UserManager.Models.CurrentUser currentUser = new TH.Agular.UserManager.Models.CurrentUser();
            if (Session["CurrentUser"] == null)
            {
                Response.Redirect(Url.Action("Login", "AdminHome"));
            }
            else
            {
                currentUser = Session["CurrentUser"] as TH.Agular.UserManager.Models.CurrentUser;
                if (!currentUser.Permits.Any())
                {
                    Response.Redirect(Url.Action("UserNotPermit", "AdminHome"));
                }
            }
            var listServices = db.Services.Where(x => x.IsDelete != true && x.ShopId == currentUser.ShopId).OrderBy(x => x.OrderIndex).Select(x => new { x.ServiceId, x.OrderIndex, x.Name, x.Alias, x.ImportPrice, x.Price, x.IsActive, x.ShopId }).ToList();
            return Json(listServices);
        }
        [HttpPost]
        public JsonResult CheckAvailableServiceName(string ServiceName, string mode)
        {
            UserManagerEntities db = new UserManagerEntities();
            if (mode == "Insert")
            {
                var list = db.Services.Where(x => x.Name == ServiceName && x.IsDelete != true).ToList();
                if (list.Any()) return Json(true); else return Json(false);
            }
            else
            {
                return Json(false);
            }

        }
        [HttpPost]
        public JsonResult GetService(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            var service = db.Services.Select(x => new { x.ServiceId, x.Unit, x.Alias, x.DefaultQuantity, x.Description, x.ImportPrice, x.Price, x.IsActive, x.Name, x.OrderIndex, x.ServiceTypeId}).FirstOrDefault(x => x.ServiceId == id);
            return Json(service);
        }
        
        [HttpPost]
        public JsonResult InsertService(string ServiceTypeId, int OrderIndex, 
                                            string Name, 
                                            string Alias, 
                                            string Unit,
                                            decimal ImportPrice,
                                            decimal Price,
                                            decimal DefaultQuantity,
                                            string Description,
                                            bool? IsActive)
        {
            UserManagerEntities db = new UserManagerEntities();
            Service service = new Service();
            service.ServiceId = Guid.NewGuid().ToString().ToLower();
            service.ServiceTypeId = ServiceTypeId;
            ServiceType type = db.ServiceTypes.Find(ServiceTypeId);
            service.ShopId = type.ShopId;
            service.OrderIndex = OrderIndex;
            service.Name = Name;
            service.Unit = Unit;
            service.Alias = Alias;
            service.ImportPrice = ImportPrice;
            service.Price = Price;
            service.DefaultQuantity = DefaultQuantity;
            service.Description = Description;
            service.IsActive = IsActive ?? false;
            service.DateCreate = DateTime.Now;
            service.IsDelete = false;
            service.DateDelete = null;
            db.Services.Add(service);
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateService(string ServiceId, string ServiceTypeId, int OrderIndex,
                                            string Name,
                                            string Alias,
                                            string Unit,
                                            decimal ImportPrice,
                                            decimal Price,
                                            decimal DefaultQuantity,
                                            string Description,
                                            bool IsActive)
        {
            UserManagerEntities db = new UserManagerEntities();
            Service service = db.Services.Find(ServiceId);
            service.ServiceTypeId = ServiceTypeId;
            ServiceType type = db.ServiceTypes.Find(ServiceTypeId);
            service.ShopId = type.ShopId;
            service.OrderIndex = OrderIndex;
            service.Name = Name;
            service.Unit = Unit;
            service.Alias = Alias;
            service.ImportPrice = ImportPrice;
            service.Price = Price;
            service.DefaultQuantity = DefaultQuantity;
            service.Description = Description;
            service.IsActive = IsActive;
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult DeleteService(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            Service service = db.Services.Find(id);
            service.IsDelete = true;
            db.SaveChanges();
            return Json(true);
        }
    }
}
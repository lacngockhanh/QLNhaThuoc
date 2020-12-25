using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;


namespace TH.Agular.UserManager.Controllers
{
    public class AdminServiceTypesController : Controller
    {
        // GET: AdminServiceTypes
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
        public JsonResult GetListServiceTypes()
        {
            CurrentUser currentUser = new CurrentUser();
            if (Session["CurrentUser"] != null)
            {
                currentUser = Session["CurrentUser"] as CurrentUser;
            }
            UserManagerEntities db = new UserManagerEntities();
            var listServiceTypes = db.ServiceTypes.Where(x => x.IsDelete != true).OrderBy(x => x.Shop.OrderIndex).ThenBy(x => x.OrderIndex).Select(x => new {x.ServiceTypeId, x.Name, x.OrderIndex, x.IsActive, ShopName = x.Shop.Name, ShopId = x.ShopId }).ToList();
            if (currentUser.Permits.Where(x => x.RoleId == 2).ToList().Any())
            {
                listServiceTypes = listServiceTypes.Where(x => x.ShopId == currentUser.ShopId).ToList();
            }
            return Json(listServiceTypes);
        }
        [HttpPost]
        public JsonResult CheckAvailableServiceTypeName(string ServiceTypeName, string mode)
        {
            UserManagerEntities db = new UserManagerEntities();
            if (mode == "Insert")
            {
                var list = db.ServiceTypes.Where(x => x.Name == ServiceTypeName && x.IsDelete != true).ToList();
                if (list.Any()) return Json(true); else return Json(false);
            }
            else
            {
                return Json(false);
            }

        }
        [HttpPost]
        public JsonResult GetServiceType(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            //ServiceType servicetype = db.ServiceTypes.Find(id);
            var serviceType = db.ServiceTypes.Where(x => x.ServiceTypeId == id).Select(x => new { x.IsActive, x.Name, x.OrderIndex, x.ServiceTypeId, x.ShopId}).FirstOrDefault();
            return Json(serviceType);
        }
        [HttpPost]
        public JsonResult InsertServiceType(string Name, int OrderIndex, bool? IsActive, string ShopId)
        {
            UserManagerEntities db = new UserManagerEntities();
            ServiceType servicetype = new ServiceType();
            servicetype.ServiceTypeId = Guid.NewGuid().ToString().ToLower();
            servicetype.Name = Name;
            servicetype.OrderIndex = OrderIndex;
            servicetype.ShopId = ShopId;
            servicetype.IsActive = IsActive ?? false;
            servicetype.DateCreate = DateTime.Now;
            servicetype.IsDelete = false;
            servicetype.DateDelete = null;
            db.ServiceTypes.Add(servicetype);
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateServiceType(string ServiceTypeId, string Name, int OrderIndex, bool? IsActive, string ShopId)
        {
            UserManagerEntities db = new UserManagerEntities();
            ServiceType servicetype = db.ServiceTypes.Find(ServiceTypeId);
            servicetype.Name = Name;
            servicetype.OrderIndex = OrderIndex;
            servicetype.ShopId = ShopId;
            servicetype.IsActive = IsActive;
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult DeleteServiceType(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            ServiceType servicetype = db.ServiceTypes.Find(id);
            servicetype.IsDelete = true;
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult GetListShops()
        {
            UserManagerEntities db = new UserManagerEntities();
            CurrentUser currentUser = new CurrentUser();
            if (Session["CurrentUser"] != null)
            {
                currentUser = Session["CurrentUser"] as CurrentUser;
            }
            var listShops = db.Shops.Where(x => x.IsDelete != true).OrderBy(x => x.OrderIndex).Select(x => new { x.ShopId, x.OrderIndex, ShopName = x.Name, x.Alias, x.Address, x.Phone, x.IsActive }).ToList();
            if (currentUser.Permits.Where(x => x.RoleId == 2).Any())
            {
                listShops = listShops.Where(x => x.ShopId == currentUser.ShopId).ToList();
            }
            
            return Json(listShops);
        }
    }
}
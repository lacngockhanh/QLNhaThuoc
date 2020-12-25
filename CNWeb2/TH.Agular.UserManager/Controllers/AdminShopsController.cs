using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;

namespace TH.Agular.UserManager.Controllers
{
    public class AdminShopsController : Controller
    {
        // GET: AdminShops

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
        public JsonResult GetListShops()
        {
            UserManagerEntities db = new UserManagerEntities();

            var listShops = db.Shops.Where(x => x.IsDelete != true).OrderBy(x => x.OrderIndex).Select(x => new { x.ShopId, x.OrderIndex, x.Name, x.Alias, x.Address, x.Phone, x.IsActive }).ToList();
            return Json(listShops);
        }
        [HttpPost]
        public JsonResult CheckAvailableShopName(string ShopName, string mode)
        {
            UserManagerEntities db = new UserManagerEntities();
            if (mode == "Insert")
            {
                var list = db.Shops.Where(x => x.Name == ShopName && x.IsDelete != true).ToList();
                if (list.Any()) return Json(true); else return Json(false);
            }
            else
            {
                return Json(false);
            }

        }
        [HttpPost]
        public JsonResult GetShop(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            var shop = db.Shops.Select(x => new { x.ShopId,
                                                    x.Name,
                                                    x.Description,
                                                    x.Alias,
                                                    x.Image,
                                                    x.Phone,
                                                    x.Address,
                                                    x.OwnerName,
                                                    x.OwnerPhone,
                                                    x.IsActive,
                                                    x.OrderIndex}).FirstOrDefault(x => x.ShopId == id);
            return Json(shop);
        }

        [HttpPost]
        public JsonResult InsertShop(
                                            string Name,
                                            string Description,
                                            string Alias,
                                            string Image,
                                            string Phone,
                                            string Address,
                                            string OwnerName,
                                            string OwnerPhone,
                                            int OrderIndex,
                                            bool? IsActive)
        {
            UserManagerEntities db = new UserManagerEntities();
            Shop shop = new Shop();
            shop.ShopId = Guid.NewGuid().ToString().ToLower();
            shop.Name = Name;
            shop.Description = Description;
            shop.Alias = Alias;
            shop.Image = Image;
            shop.Phone = Phone;
            shop.Address = Address;
            shop.OwnerName = OwnerName;
            shop.OwnerPhone = OwnerPhone;
            shop.OrderIndex = OrderIndex;
            shop.IsActive = IsActive ?? false;              
            shop.DateCreate = DateTime.Now;
            shop.IsDelete = false;
            shop.DateDelete = null;
            db.Shops.Add(shop);
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateShop(string ShopId, 
                                            string Name,
                                            string Description,
                                            string Alias,
                                            string Image,
                                            string Phone,
                                            string Address,
                                            string OwnerName,
                                            string OwnerPhone,
                                            int OrderIndex,
                                            bool? IsActive)
        {
            UserManagerEntities db = new UserManagerEntities();
            Shop shop = db.Shops.Find(ShopId);
            shop.Name = Name;
            shop.Description = Description;
            shop.Alias = Alias;
            shop.Image = Image;
            shop.Phone = Phone;
            shop.Address = Address;
            shop.OwnerName = OwnerName;
            shop.OwnerPhone = OwnerPhone;            
            shop.OrderIndex = OrderIndex;
            shop.IsActive = IsActive ?? false;
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult DeleteShop(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            Shop shop = db.Shops.Find(id);
            shop.IsDelete = true;
            db.SaveChanges();
            return Json(true);
        }
    }
}
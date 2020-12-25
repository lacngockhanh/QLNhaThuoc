using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;

namespace TH.Agular.UserManager.Controllers
{
    public class AdminUsersController : Controller
    {
        // GET: AdminUsers
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
        public JsonResult GetListUsers()
        {
            UserManagerEntities db = new UserManagerEntities();
            CurrentUser currentUser = new CurrentUser();
            if (Session["CurrentUser"] != null)
            {
                currentUser = Session["CurrentUser"] as CurrentUser;
            }
            var listUsers = db.Users.Select(x => new { x.UserId, x.UserName, x.FullName, x.DateCreate, x.IsActive, x.IsDelete, x.ShopId, ShopName = x.Shop.Name }).Where(x => x.IsDelete != true).OrderByDescending(x => x.DateCreate).ToList();
            if (currentUser.Permits.Where(x => x.RoleId == 2).Any())
            {
                listUsers = listUsers.Where(x => x.ShopId == currentUser.ShopId).ToList();
            }
            
            return Json(listUsers);            
        }
        [HttpPost]
        public JsonResult CheckAvailableUserName(string UserName, string mode)
        {
            UserManagerEntities db = new UserManagerEntities();
            if (mode == "Insert")
            {
                var list = db.Users.Where(x => x.UserName == UserName && x.IsDelete != true).ToList();
                if (list.Any()) return Json(true); else return Json(false);
            } else
            {
                return Json(false);
            }
            
        }   
        [HttpPost]
        public JsonResult GetUser(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            var user = db.Users.Select(x => new {x.UserId, x.UserName, x.FullName, x.Email, x.Phone, x.Description, x.IsActive, x.DateCreate, x.IsDelete, x.ShopId, ShopName = x.Shop.Name }).Where(x => x.IsDelete != true && x.UserId == id).OrderByDescending(x => x.DateCreate).FirstOrDefault();

            return Json(user);
        }
        [HttpPost]
        public JsonResult InsertUser(string FullName, string UserName, string Password, string Email, string Phone, bool? IsActive, string ShopId)
        {
            UserManagerEntities db = new UserManagerEntities();
            User user = new User();
            user.UserId = Guid.NewGuid().ToString().ToLower();
            user.FullName = FullName;
            user.UserName = UserName;
            user.Password = THLib.ToMD5(Password);
            user.Email = Email;
            user.Phone = Phone;
            user.IsActive = IsActive??false;
            user.DateCreate = DateTime.Now;
            user.IsDelete = false;
            user.DateDelete = null;
            user.ShopId = ShopId;
            db.Users.Add(user);
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult UpdateUser(string UserId, string FullName, string UserName, string Password, string Email, string Phone, bool? IsActive, string ShopId)
        {
            UserManagerEntities db = new UserManagerEntities();
            User user = db.Users.Find(UserId);
            user.FullName = FullName;
            user.UserName = UserName;
            if (Password != "")
            {
                user.Password = THLib.ToMD5(Password);
            }
            user.Email = Email;
            user.Phone = Phone;
            user.ShopId = ShopId;
            user.IsActive = IsActive;
            db.SaveChanges();
            return Json(true);
        }
        [HttpPost]
        public JsonResult DeleteUser(string id)
        {
            UserManagerEntities db = new UserManagerEntities();
            User user = db.Users.Find(id);
            user.IsDelete = true;
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
            var listShops = db.Shops.Where(x => x.IsDelete != true).OrderBy(x => x.OrderIndex).Select(x => new { x.ShopId, x.OrderIndex, x.Name, x.Alias, x.Address, x.Phone, x.IsActive }).ToList();
            if (currentUser.Permits.Where(x => x.RoleId == 2).Any())
            {
                listShops = listShops.Where(x => x.ShopId == currentUser.ShopId).ToList();
            }
            
            return Json(listShops);
        }

    }
}
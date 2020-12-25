using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;

namespace TH.Agular.UserManager.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: AdminHome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ActionLogin(string UserName, string Password)
        {
            UserManagerEntities db = new UserManagerEntities();
            string userName = UserName.ToLower();
            string passwordMd5 = Password.ToMD5();
            var list = db.Users.Where(x => x.UserName == userName && x.Password == passwordMd5).ToList();
            if (!list.Any())
            {
                return RedirectToAction("Login", "AdminHome", new { msg = "false" });
            }
            else
            {

                var user = list.FirstOrDefault();
                CurrentUser currentUser = new CurrentUser();
                currentUser.UserId = user.UserId;
                currentUser.UserName = user.UserName;
                currentUser.FullName = user.FullName;
                currentUser.ShopName = user.Shop.Name;
                currentUser.ShopId = user.ShopId;
                currentUser.Permits = user.Permits as ICollection<Permit>;
                Session["CurrentUser"] = currentUser;
                return RedirectToAction("Index", "AdminHome");
            }

        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "AdminHome");
        }
        public ActionResult UserNotPermit()
        {
            return View();
        }
    }
}
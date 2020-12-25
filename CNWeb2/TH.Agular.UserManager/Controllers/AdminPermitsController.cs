using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TH.Agular.UserManager.Models;

namespace TH.Agular.PermitManager.Controllers
{
    public class AdminPermitsController : Controller
    {
        // GET: AdminPermits
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
        public JsonResult GetListPermits()
        {
            UserManagerEntities db = new UserManagerEntities();
            List<Permit> listPermits = new List<Permit>();
            listPermits = db.Permits.ToList();
            return Json(listPermits);
        }

        [HttpPost]
        public ActionResult CheckPermitAvailable(string UserId, int RoleId)
        {
            UserManagerEntities db = new UserManagerEntities();
            bool isAvailable = db.Permits.Where(x => x.UserId == UserId && x.RoleId == RoleId).ToList().Any();
            if (isAvailable)
            {
                return Content("available");
            } else return Content("ok");
        }
        [HttpPost]
        public ActionResult InsertPermit(string UserId, int RoleId)
        {
            UserManagerEntities db = new UserManagerEntities();
            Permit permit = new Permit();                
            permit.UserId = UserId;
            permit.RoleId = RoleId;
            db.Permits.Add(permit);
            db.SaveChanges();
            return Content("ok");
        }
        
        [HttpPost]
        public ActionResult RemovePermit(int id)
        {
            UserManagerEntities db = new UserManagerEntities();
            Permit permit = db.Permits.Find(id);
            db.Permits.Remove(permit);
            db.SaveChanges();
            return Content("ok");
        }
    }
}
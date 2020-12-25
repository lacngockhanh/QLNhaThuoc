using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TH.Agular.UserManager.Models
{
    public class CurrentUser
    {
        public string UserId;
        public string FullName;
        public string UserName;
        public string ShopId;
        public string ShopName;
        public ICollection<Permit> Permits;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZeroHunger.DTO
{
    public class NGOlogin
    {
        public class adminAccess : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                return httpContext.Session["user"] != null && httpContext.Session["type"].ToString().Equals("admin");
            }
        }

    }
}
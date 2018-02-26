using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistriFest.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if(filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                RouteValueDictionary rvd = new RouteValueDictionary(new { controller = "Error", action = "NotAllowed" });
                filterContext.Result = new RedirectToRouteResult(rvd);
                
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

    }
}
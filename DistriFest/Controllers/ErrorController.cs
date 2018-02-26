using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistriFest.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotAllowed()
        {
            ViewBag.errorMessage = "You have insufficient privilages to acces this page";

            return View();
        }
    }
}
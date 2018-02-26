using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistriFest.Models;
using DistriFest.Exceptions;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DistriFest.Controllers
{
    [Models.Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            return View(claims);
        }


        public ActionResult Ordering()
        {
            //shows only orders for bar signed, or all of the orders for any of the bars for everyone else
            var identity = (ClaimsIdentity)User.Identity;
            ProductOrderViewModel productlist = new ProductOrderViewModel(Convert.ToInt16(identity.Claims.Last().Value));
            if (TempData["ProcessResult"] != null)
            {
                ViewBag.ErrorMessage = TempData["ProcessResult"];
            }
            return View(productlist);

        }

        public ActionResult ShoppingCart()
        {
            var identity = (ClaimsIdentity)User.Identity;
            ShoppingCartViewModel scvm = new ShoppingCartViewModel(Convert.ToInt16(identity.Claims.Last().Value));
            if (TempData["ProcessResult"] != null)
            {
                ViewBag.ErrorMessage = TempData["ProcessResult"];
            }
            return View(scvm);
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin"), HandleError]
        public ActionResult ManageProducts()
        {
            var identity = (ClaimsIdentity)User.Identity;
            ProductOrderViewModel productlist = new ProductOrderViewModel(Convert.ToInt16(identity.Claims.Last().Value));
            if (TempData["ProcessResult"] != null)
            {
                ViewBag.ErrorMessage = TempData["ProcessResult"];
            }
            return View(productlist);
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin"), HandleError]
        public ActionResult Reporting()
        {
            ReportingViewModel report = new ReportingViewModel();
            return View(report);
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin, DC"), HandleError]
        public ActionResult StockControl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderProduct(OrderViewModel OVM)
        {
            if (OVM.AmountOrdered > 0)
            {
                Order.RegisterOrder(OVM.OrderID, OVM.ProdID, OVM.AmountOrdered);
            }

            return RedirectToAction("Ordering");
        }

        [HttpPost]
        public ActionResult ProcessOrder()
        {
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                Order.ProcessOrder(Convert.ToInt16(identity.Claims.Last().Value));
                TempData["ProcessResult"] = "Bestelling succesvol verwerkt";
            }
            catch
            {
                TempData["ProcessResult"] = "Er ging iets fout tijdens het verwerken van je bestelling!";
            }
            
            return RedirectToAction("ShoppingCart");
        }

        [HttpPost]
        public ActionResult DeleteProductFromOrder(Product prod)
        {
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                Order.RemoveProduct(Convert.ToInt16(identity.Claims.Last().Value), prod.ID);
                TempData["ProcessResult"] = "Product succesvol verwijderd";
            }
            catch
            {
                TempData["ProcessResult"] = "Er ging iets mis tijdens het verwijderen van het product, probeer het opnieuw.";
            }

            if ((string)TempData["Return"] == "Ordering")
            {
                return RedirectToAction("Ordering");
            }
            return RedirectToAction("ShoppingCart");
        }

        [HttpPost]
        public ActionResult ChangeProductInOrder(OrderViewModel ovm)
        {
            if (ovm.AmountOrdered == 0)
            {
                DeleteProductFromOrder(new Product { ID = ovm.ProdID});
                return RedirectToAction("ShoppingCart");
            }
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                Order.EditOrderedAmount(Convert.ToInt16(identity.Claims.Last().Value), ovm.ProdID, ovm.AmountOrdered);
                TempData["ProcessResult"] = "Product succesvol aangepast";
            }
            catch
            {
                TempData["ProcessResult"] = "Er ging iets mis tijdens het wijzigen, probeer het opnieuw.";
            }

            return RedirectToAction("ShoppingCart");
        }



    }
}
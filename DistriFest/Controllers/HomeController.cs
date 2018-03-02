using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DFModels = DistriFest.Models;
using DistriFest.Exceptions;
using System.Security.Claims;
using System.Security.Cryptography;
using Models;
using Repositories;
using Interfaces;


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
            IProductRepository prodRepo = new ProductRepository();
            List<OrderLine> productList = new List<OrderLine>();
            foreach(Product prod in prodRepo.GetAllProducts())
            {
                productList.Add(new OrderLine(prod,0));
            }

            //shows only orders for bar signed, or all of the orders for any of the bars for everyone else
            //var identity = (ClaimsIdentity)User.Identity;
            //ProductOrderViewModel productlist = new ProductOrderViewModel(Convert.ToInt16(identity.Claims.Last().Value));
            if (TempData["ProcessResult"] != null)
            {
                ViewBag.ErrorMessage = TempData["ProcessResult"];
            }
            return View(productList);

        }

        public ActionResult ShoppingCart()
        {
            var identity = (ClaimsIdentity)User.Identity;
            DFModels.ShoppingCartViewModel scvm = new DFModels.ShoppingCartViewModel(Convert.ToInt16(identity.Claims.Last().Value));
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
            DFModels.ProductOrderViewModel productlist = new DFModels.ProductOrderViewModel(Convert.ToInt16(identity.Claims.Last().Value));
            if (TempData["ProcessResult"] != null)
            {
                ViewBag.ErrorMessage = TempData["ProcessResult"];
            }
            return View(productlist);
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin"), HandleError]
        public ActionResult Reporting()
        {
            DFModels.ReportingViewModel report = new DFModels.ReportingViewModel();
            return View(report);
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin, DC"), HandleError]
        public ActionResult StockControl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderProduct(OrderLine OL)
        {
            if (OL.Amount > 0)
            {
                var identity = (ClaimsIdentity)User.Identity;
                IOrderRepository OrderRepo = new OrderRepository();
                IOrderLineRepository OrderLineRepo = new OrderLineRepository();
                Order Order = OrderRepo.CheckForOpenOrder(Convert.ToInt16(identity.Claims.Last().Value));
                OrderLineRepo.AddOrderLineToOrder(OL, Order.ID);
            }

            return RedirectToAction("Ordering");
        }

        [HttpPost]
        public ActionResult ProcessOrder()
        {
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                IOrderRepository OrderRepo = new OrderRepository();
                Order Order = OrderRepo.CheckForOpenOrder(Convert.ToInt16(identity.Claims.Last().Value));
                if (Order.Products.Count > 0)
                {
                    OrderRepo.ProcessOrder(Order);
                    TempData["ProcessResult"] = "Bestelling succesvol verwerkt";
                }
                else
                {
                    TempData["ProcessResult"] = "Geen Producten in bestelling";
                }                
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
                DFModels.Order.RemoveProduct(Convert.ToInt16(identity.Claims.Last().Value), prod.ID);
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
        public ActionResult ChangeProductInOrder(DFModels.OrderViewModel ovm)
        {
            if (ovm.AmountOrdered == 0)
            {
                DeleteProductFromOrder(new Product { ID = ovm.ProdID});
                return RedirectToAction("ShoppingCart");
            }
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                DFModels.Order.EditOrderedAmount(Convert.ToInt16(identity.Claims.Last().Value), ovm.ProdID, ovm.AmountOrdered);
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
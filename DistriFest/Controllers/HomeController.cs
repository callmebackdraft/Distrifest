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
            List<Product> productList = prodRepo.GetAllProducts();
            if (TempData["ProcessResult"] != null)
            {
                ViewBag.ErrorMessage = TempData["ProcessResult"];
            }
            return View(productList);
        }

        public ActionResult ShoppingCart()
        {
            var identity = (ClaimsIdentity)User.Identity;
            DFModels.ViewModels.ShoppingCartViewModel scvm = new DFModels.ViewModels.ShoppingCartViewModel(Convert.ToInt16(identity.Claims.Last().Value));
            if (TempData["ProcessResult"] != null)
            {
                ViewBag.ErrorMessage = TempData["ProcessResult"];
            }
            return View(scvm);
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin"), HandleError]
        public ActionResult ManageProducts()
        {
            IProductRepository prodRepo = new ProductRepository();
            List<Product> productList = prodRepo.GetAllProducts();
            return View(productList);
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin"), HandleError]
        public ActionResult Reporting()
        {
            IReportRepository ReportRepo = new ReportRepository();
            return View(ReportRepo.GetAllReportCharts());
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin, DC"), HandleError]
        public ActionResult StockControl()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OrderProduct(int Amount, int ProdID)
        {
            if (Amount > 0)
            {
                var identity = (ClaimsIdentity)User.Identity;
                IOrderRepository OrderRepo = new OrderRepository();
                IOrderLineRepository OrderLineRepo = new OrderLineRepository();
                IProductRepository ProductRepository = new ProductRepository();
                OrderLine OL = new OrderLine(ProductRepository.GetProductByID(ProdID), Amount);
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
        public ActionResult DeleteProductFromOrder(DFModels.ViewModels.OLChangeViewModel ocm)
        {
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                IOrderLineRepository OrderLineRepo = new OrderLineRepository();
                OrderLineRepo.RemoveOrderLineFromOrder(ocm.ProdID,ocm.OrderID);
                
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
        public ActionResult ChangeProductInOrder(DFModels.ViewModels.OLChangeViewModel ocm)
        {
            if (ocm.Amount == 0)
            {
                DeleteProductFromOrder(ocm);
                return RedirectToAction("ShoppingCart");
            }
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                IOrderLineRepository OrderLineRepo = new OrderLineRepository();
                OrderLineRepo.EditOrderedAmount(ocm.OrderID, ocm.ProdID, ocm.Amount);
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
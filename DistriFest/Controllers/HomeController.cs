using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DistriFest.Models;
using DistriFest.Exceptions;
using System.Security.Claims;
using System.Security.Cryptography;
using DistriFest.Models.ViewModels;
using Rotativa;
using Models;
using Repositories;
using Interfaces;
using System.IO;

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
            Models.ViewModels.ShoppingCartViewModel scvm = new Models.ViewModels.ShoppingCartViewModel(Convert.ToInt16(identity.Claims.Last().Value));
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

        [Models.Authorize(Roles = "Admin, SuperAdmin, DC, StockOnly"), HandleError]
        public ActionResult StockControl()
        {
            return View(new ProductRepository().GetAllProducts());
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin, DC"), HandleError]
        public ActionResult DCOverview()
        {
            IOrderRepository OrderRepo = new OrderRepository();
            if (TempData["ProcessResult"] != null)
            {
                ViewBag.ErrorMessage = TempData["ProcessResult"];
            }
            return View(OrderRepo.GetAllRelevantOrders());
        }

        [Models.Authorize(Roles = "Bar, SuperAdmin"), HandleError]
        public ActionResult BarOrderView()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return View(new OrderRepository().GetAllOrders(Convert.ToInt16(identity.Claims.Last().Value)));
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
                List<OrderLine> results = Order.Products.FindAll(x => x.Product.ID == ProdID);
                if (results.Count >= 1)
                {
                    OrderLineRepo.EditOrderedAmount(Order.ID,ProdID,Amount + results[0].Amount);
                }
                else
                {
                    OrderLineRepo.AddOrderLineToOrder(OL, Order.ID);
                }
            }
            return RedirectToAction("Ordering");
        }

        private ActionResult GetPDFPackingSlip(Order _order)
        {
            try
            {
                var report = new PartialViewAsPdf("../PartialViews/PartialPackingSlip", _order)
                {
                    FileName = "Bestelling:" + _order.ID,
                };
                return report;
            }
            catch (Exception e)
            {
                throw new Exception("An Error Occured while creating PDF please try again: " + e.Message);
            }
        }

        [HttpPost]
        public ActionResult FurtherOrderStatus(int _orderID, OrderStatus.OrderStatusesEnum _orderStatuses, string _returnURL)
        {
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                IOrderRepository OrderRepo = new OrderRepository();
                Order Order = OrderRepo.GetOrderByID(_orderID);
                if (Order.Products.Count > 0)
                {
                    OrderRepo.FurtherOrderStatus(Order, _orderStatuses);
                    if (_orderStatuses == OrderStatus.OrderStatusesEnum.WaitingForDC)
                    {
                        var body = System.IO.File.ReadAllText(Server.MapPath(@"~\Content\EmailTemplate.cshtml"));
                        Mail mail = new Mail("dc@notacorrect.nl", "Nieuwe Bestelling: " + Order.ID + " van Bar: " + Order.CustomerID, string.Format(body, Order.CustomerID, DateTime.Now.ToString("dd/MM/yyyy")));
                        MemoryStream strm = new MemoryStream(((ViewAsPdf)GetPDFPackingSlip(Order)).BuildFile(ControllerContext));
                        System.Net.Mail.Attachment pdfatt = new System.Net.Mail.Attachment(strm, "Bestelling: " + Order.ID + ".pdf", "application/pdf");
                        mail.AddAttachment(pdfatt);
                        mail.SendMail();
                        TempData["ProcessResult"] = string.Format("Bestelling: {0} succesvol doorgezet naar Distributiecentrum", _orderID);
                    }
                    else if (_orderStatuses == OrderStatus.OrderStatusesEnum.Processing)
                    {
                        TempData["ProcessResult"] = string.Format("Bestelling: {0} in behandeling genomen", _orderID);
                    }
                    else if (_orderStatuses == OrderStatus.OrderStatusesEnum.Delivered)
                    {
                        TempData["ProcessResult"] = string.Format("Bestelling: {0} succesvol verwerkt", _orderID);
                    }
                    else if (_orderStatuses == OrderStatus.OrderStatusesEnum.Rejected)
                    {
                        TempData["ProcessResult"] = string.Format("Bestelling: {0} succesvol Geweigerd", _orderID);
                    }
                }
                else
                {
                    TempData["ProcessResult"] = "Geen Producten in bestelling";
                }                
            }
            catch
            {
                TempData["ProcessResult"] = "Er ging iets fout tijdens het verwerken van de bestelling!";
            }
            
            return Redirect(_returnURL);
        }

        [HttpPost]
        public ActionResult DeleteProductFromOrder(Models.ViewModels.OLChangeViewModel ocm)
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
        public ActionResult ChangeProductInOrder(Models.ViewModels.OLChangeViewModel ocm)
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

        [HttpPost]
        public ActionResult ProcessDelivery(DeliveryViewModel _delivery)
        {
            IDeliveryLineRepository DelLineRepo = new DeliveryLineRepository();
            IDeliveryRepository DelRepo = new DeliveryRepository();
            Delivery delivery = _delivery.ConvertToDelivery();
            DelLineRepo.SaveAllDeliveryLines(delivery);
            DelRepo.UpdateDelivery(delivery);
            return RedirectToAction("StockControl");
        }

        [HttpPost]
        public ActionResult ProcessDCOrder(OrderViewModel _order)
        {
            IOrderRepository OrderRepo = new OrderRepository();
            IOrderLineRepository OrderLineRepo = new OrderLineRepository();
            Order order = _order.ConvertToOrder();
            OrderRepo.UpdateOrder(order);
            OrderLineRepo.SaveAllOrderLines(order);
            FurtherOrderStatus(order.ID, _order.SelectedStatus, "/Home/StockControl");


            return RedirectToAction("StockControl");
        }

        public ActionResult EditProduct()
        {
            return RedirectToAction("ManageProducts");
        }

        public ActionResult Delivery()
        {
            return View(new DeliveryViewModel(new DeliveryRepository().GetNewDelivery()));
        }
        
        public ActionResult AddProductToDB()
        {
            return View();
        }

        public ActionResult ShowOrder(int _orderID)
        {
            return View(new OrderRepository().GetOrderByID(_orderID));
        }

        public ActionResult PartialPackingSlip(int _orderID)
        {
            PartialViewAsPdf result = new PartialViewAsPdf("../PartialViews/PartialPackingSlip", new OrderRepository().GetOrderByID(_orderID));
            result.FileName = "Bestelling - " + _orderID + ".pdf";
            return result;
        }

        public ActionResult PartialEmptyOrderList()
        {
            return new PartialViewAsPdf("../PartialViews/PartialEmptyOrderList", new ProductRepository().GetAllProducts());
        }

        public ActionResult DCOrder()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IOrderRepository OrderRepo = new OrderRepository();
            return View(new OrderViewModel(OrderRepo.CheckForOpenOrder(Convert.ToInt16(identity.Claims.Last().Value))));
        }
    }
}
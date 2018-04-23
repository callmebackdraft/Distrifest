﻿using System;
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
using extmodels = Models;
using Models;
using Repositories;
using Interfaces;
using System.IO;
using DataHandling;
using ClosedXML.Excel;
using Microsoft.AspNet.SignalR;
using DistriFest.Models.SignalR;
using Newtonsoft.Json;

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

        [Models.Authorize(Roles = "Admin, SuperAdmin, DCCoord"), HandleError]
        public ActionResult ManageProducts()
        {
            IProductRepository prodRepo = new ProductRepository();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            foreach(Product _prod in prodRepo.GetAllProducts())
            {
                productList.Add(new ProductViewModel(_prod));
            }
            return View(productList);
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin"), HandleError]
        public ActionResult Reporting()
        {
            IReportRepository ReportRepo = new ReportRepository();
            return View(ReportRepo.GetAllReportCharts());
        }

        [Models.Authorize(Roles = "Admin, SuperAdmin, DC, StockOnly, DCCoord"), HandleError]
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
        public void FurtherOrderStatus(int _orderID, OrderStatus.OrderStatusesEnum _orderStatus, string _returnURL)
        {
            string notificationMsg = "";
            var identity = (ClaimsIdentity)User.Identity;
            try
            {
                IOrderRepository OrderRepo = new OrderRepository();
                Order Order = OrderRepo.GetOrderByID(_orderID);
                if (Order.Products.Count > 0)
                {
                    if (Order.Statuses.FindIndex(x => x.RegisteredStatus == _orderStatus) == -1)
                    {
                        foreach (string str in OrderRepo.FurtherOrderStatus(Order, _orderStatus))
                        {
                            notificationMsg += str;
                        }
                    }
                    if (_orderStatus == OrderStatus.OrderStatusesEnum.WaitingForDC)
                    {
                        UpdateDCOverviewTroughSignalR();
                        var body = System.IO.File.ReadAllText(Server.MapPath(@"~\Content\EmailTemplate.cshtml"));
                        Mail mail = new Mail("dc@notacorrect.nl", "Nieuwe Bestelling: " + Order.ID + " van Bar: " + Order.CustomerID, string.Format(body, Order.CustomerID, DateTime.Now.ToString("dd/MM/yyyy")));
                        MemoryStream strm = new MemoryStream(((ViewAsPdf)GetPDFPackingSlip(Order)).BuildFile(ControllerContext));
                        System.Net.Mail.Attachment pdfatt = new System.Net.Mail.Attachment(strm, "Bestelling: " + Order.ID + ".pdf", "application/pdf");
                        SendMessageTroughSignalR(Order.CustomerID, notificationMsg);
                        SendMessageTroughSignalR(8, new UserRepository().GetUserByID(Order.CustomerID).Name + " Heeft een nieuwe bestelling doorgevoerd");
                        mail.AddAttachment(pdfatt);
                        mail.SendMail();
                    }
                    else if (_orderStatus == OrderStatus.OrderStatusesEnum.Processing)
                    {
                        SendMessageTroughSignalR(8, string.Format("Bestelling: {0} in behandeling genomen", _orderID));
                    }
                    else if (_orderStatus == OrderStatus.OrderStatusesEnum.Delivered)
                    {
                        SendMessageTroughSignalR(8, string.Format("Bestelling: {0} succesvol verwerkt", _orderID));
                    }
                    else if (_orderStatus == OrderStatus.OrderStatusesEnum.Rejected)
                    {
                        SendMessageTroughSignalR(Order.CustomerID, string.Format("Distributie Centrum heeft uw bestelling: {0} geweigerd. Melding van DC: {1}", _orderID, _returnURL));
                        SendMessageTroughSignalR(8, string.Format("Bestelling: {0} succesvol Geweigerd", _orderID));
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
        }

        [HttpPost]
        public ActionResult DeleteProductFromOrder(Models.ViewModels.OLChangeViewModel ocm, string _returnURL)
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
            return Redirect(_returnURL);
        }

        [HttpPost]
        public ActionResult ChangeProductInOrder(Models.ViewModels.OLChangeViewModel ocm)
        {
            if (ocm.Amount == 0)
            {
                DeleteProductFromOrder(ocm, "/Home/ShoppingCart");
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

        public ActionResult EditProduct(ProductViewModel _product)
        {
            IProductRepository ProdRepo = new ProductRepository();
            ProdRepo.EditProduct(_product.ConvertToProduct());
            return RedirectToAction("ManageProducts");
        }

        public ActionResult Delivery()
        {
            return View(new DeliveryViewModel(new DeliveryRepository().GetNewDelivery()));
        }
        
        public ActionResult AddProductToDB(ProductViewModel _product)
        {
            IProductRepository ProdRepo = new ProductRepository();
            int x = ProdRepo.SaveNewProduct(_product.ConvertToProduct());
            return RedirectToAction("ManageProducts");
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

        [HttpPost]
        public FileResult GetExcelReport()
        {
            //IUserContext Userctx = new UserSQLQuery();
            //IOrderContext Orderctx = new OrderSQLQuery();
            //IOrderLineContext OrderLinectx = new OrderLineSQLQuery();
            //IOrderStatusContext OrderStatusctx = new OrderStatusSQLQuery();
            //IProductContext Productctx = new ProductSQLQuery();
            //IDeliveryContext Deliveryctx = new DeliverySQLQuery();
            //IDeliveryLineContext DeliveryLinectx = new DeliveryLineContext();
            IReportContext Reportctx = new ReportSQLQuery();
            using (XLWorkbook report = new XLWorkbook())
            {
                //report.Worksheets.Add(Userctx.GetAllUsers(), "Gebruikers");
                //report.Worksheets.Add(Orderctx.GetAllOrders(), "Bestellingen");
                //report.Worksheets.Add(OrderLinectx.GetAllOrderLines(), "BestelRegels");
                //report.Worksheets.Add(OrderStatusctx.GetAllStatuses(), "BestelStatussen");
                //report.Worksheets.Add(Productctx.GetAllProducts(), "Producten");
                //report.Worksheets.Add(Deliveryctx.GetAllDeliverys(), "Leveringen");
                //report.Worksheets.Add(DeliveryLinectx.GetAllDeliveryLines(), "LeverRegels");
                report.Worksheets.Add(Reportctx.GetOrderedReport(),"Bestel per bar");
                using (MemoryStream stream = new MemoryStream())
                {
                    report.SaveAs(stream);
                    return File(stream.ToArray(),"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","DistriFest-Repost.xlsx");
                }
            }

            throw new NotImplementedException();
        }


        void SendMessageTroughSignalR(int _recipientID,string _message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            extmodels.User recipient = new UserRepository().GetUserByID(_recipientID);
            if (LiveConnections.liveConnections.Exists(x => x.User.ID == recipient.ID))
            {
                hubContext.Clients.Client(LiveConnections.liveConnections.FirstOrDefault(p => p.User.ID == recipient.ID).ConnectionID).AddMessageToPage(_message);
            }
        }

        void UpdateDCOverviewTroughSignalR()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            extmodels.User dc = new UserRepository().GetUserByID(8);
            if (LiveConnections.liveConnections.Exists(x => x.User.Name == "DC"))
            {
                hubContext.Clients.Client(LiveConnections.liveConnections.FirstOrDefault(p => p.User.Name == "DC").ConnectionID).UpdateOverview("update");
            }
        }

        [HttpGet]
        public string GetLiveConnections()
        {
            string result = JsonConvert.SerializeObject(LiveConnections.liveConnections, Formatting.Indented);
            return result;
        }

        [HttpGet]
        public string GetChatHistory(int userID)
        {
            extmodels.User requestor = new UserRepository().GetUserByID(userID);
            List<ChatMessage> subResult = ChatHistory.chatHistory.FindAll(x => x.Sender.Equals(requestor));
            subResult.AddRange(ChatHistory.chatHistory.FindAll(x => x.Recipient.Equals(requestor)));
            List<ChatMessage> sortedResult = subResult.OrderBy(x => x.Date).ToList();
            string result = JsonConvert.SerializeObject(sortedResult, Formatting.Indented);
            return result;
        }
    }
}
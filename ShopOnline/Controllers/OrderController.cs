using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopOnline.Models;
using ShopOnline.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class OrderController : Controller
    {
        private readonly OrderSvc orderSvc = new OrderSvc();
        private readonly ILogger<OrderController> _logger;
        //private readonly string _clientId;
        //private readonly string _serectKey;

        public OrderController(ILogger<OrderController> logger, IHttpContextAccessor context, IConfiguration configuration)
        {
            _logger = logger;
            //_clientId = configuration["Paypal:ClientId"];
            //_serectKey = configuration["Paypal:SerectKey"];

        }

        #region Controller
        [Authorize(Roles = "USER, ADMIN")]
        public IActionResult Order()
        {
            return View();
        }

        [Authorize(Roles = "USER, ADMIN")]
        [HttpPost]
        public IActionResult Order(IFormCollection collection)
        {
            string jsonCarts = HttpContext.Session.GetString("carts");
            List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(jsonCarts);

            string res = orderSvc.AddOrder(collection, carts);
            switch (res)
            {
                case "t":
                    TempData["cNotificationOrder"] = "Đơn hàng đã được ghi nhận.";
                    break;
                case "f":
                    TempData["cNotificationOrder"] = "Đơn hàng ghi nhận thất bại. Vui lòng đặt lại sản phẩm.";
                    break;
                default:
                    TempData["cNotificationOrder"] = "Xảy ra lỗi hệ thống. Vui lòng quay lại sau.";
                    break;
            }
            ClearCartsInSession();
            return this.Order();

        }


        [Authorize(Roles = "USER, ADMIN")]
        public IActionResult MyOrder()
        {
            string username = User.Claims.FirstOrDefault().Value;
            if (User.IsInRole("ADMIN"))
            {
                return View(orderSvc.GetListOrderSales(null));
            }
            return View(orderSvc.GetListOrderSales(username));
        }

        [Authorize(Roles = "USER, ADMIN")]
        public IActionResult OrderDtl(int id)
        {
            var res = orderSvc.GetOrderSaleById(id);
            return View(res);
        }
        #endregion


        #region Api Controller
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("/Order/Update/{id}")]
        public IActionResult UpdateOrder(IFormCollection collection, int id)
        {
            var res = orderSvc.UpdateOrder(collection, id);
            switch (res)
            {
                case "t":
                    TempData["edNotificationOrder"] = "Chỉnh sửa thành công thông tin đơn hàng.";
                    break;
                case "f":
                    TempData["edNotificationOrder"] = "Chỉnh sửa không thành công đơn hàng.";
                    break;
                case "n":
                    TempData["edNotificationOrder"] = "Không tìm thấy đơn hàng.";
                    break;
                default:
                    TempData["edNotificationOrder"] = "Lỗi hệ thống. Vui lòng quay lại sau.";
                    break;
            }

            return NoContent();
        }

        [Authorize(Roles = "USER, ADMIN")]
        [HttpPatch("/Order/Delete/{id}")]
        public IActionResult DeleteOrder(int id)
        {
            string res = orderSvc.DeleteOrder(id);
            switch (res)
            {
                case "t":
                    TempData["dNotificationOrder"] = "Hủy đơn hàng thành công.";
                    break;
                case "f":
                    TempData["dNotificationOrder"] = "Hủy đơn hàng thất bại.";
                    break;
                case "to":
                    TempData["dNotificationOrder"] = "Hết thời gian hủy đơn hàng. Vui lòng liên hệ với shop để hỗ trợ.";
                    break;
                default:
                    TempData["dNotificationOrder"] = "Hệ thống bị lỗi. Vui lòng quay lại sau.";
                    break;
            }
            return NoContent();
        }
        #endregion


        #region Function
        public void ClearCartsInSession()
        {
            var session = HttpContext.Session;
            session.Remove("carts");
        } 
        #endregion
    }
}

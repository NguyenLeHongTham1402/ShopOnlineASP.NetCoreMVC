using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopOnline.Models;
using ShopOnline.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductSvc productSvc = new ProductSvc();
        private readonly CommentSvc commentSvc = new CommentSvc();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region Controller
        public IActionResult Index()
        {
            
            string fromPrice = HttpContext.Request.Query["fromPrice"].ToString();
            string toPrice = HttpContext.Request.Query["toPrice"].ToString();
            if (!string.IsNullOrWhiteSpace(fromPrice) && !string.IsNullOrWhiteSpace(toPrice))
            {
                double from_price = fromPrice != "" ? double.Parse(fromPrice) : 0;
                double to_price = toPrice != "" ? double.Parse(toPrice) : 0;
                if (to_price != 0 && from_price != 0 && from_price <= to_price)
                {
                    ViewData["ListProducts"] = productSvc.GetListProduct().Where(x => x.Price >= from_price && x.Price <= to_price).ToList();
                }
                else if (to_price != 0 && from_price == 0)
                {
                    ViewData["ListProducts"] = productSvc.GetListProduct().Where(x => x.Price <= to_price).ToList();
                }
                else if (from_price != 0 && to_price == 0)
                {
                    ViewData["ListProducts"] = productSvc.GetListProduct().Where(x => x.Price >= from_price).ToList();
                }
                else
                {
                    ViewData["ListProducts"] = null;
                }
            }
            else
            {
                ViewData["ListProducts"] = productSvc.GetListProduct();
            }
            

            return View();
        }

        public IActionResult DetailProduct(int id)
        {
            var res = productSvc.GetProductById(id);
            if (res != null)
            {
                ViewData["ListProductByCategoryId"] = productSvc.GetListProductByCategoryId((int)res.CategoryId);
                ViewData["ListComment"] = commentSvc.GetComments(id);
            }
                
            return View(res);
        }
        #endregion


        #region Api controller
        [Authorize(Roles = "ADMIN, USER")]
        [HttpPost("/Cart/AddCart/{productId}")]
        public IActionResult AddToCart([FromBody] Cart cart, int productId)
        {
            var carts = GetListCartsInSession();

            var p = carts.Find(x => x.productId == productId);

            if (p != null)
            {
                carts.Where(x => x.productId == productId).ToList().ForEach(x => x.quantity = x.quantity + cart.quantity);
            }
            else
            {
                carts.Add(cart);
            }
            SaveCartToSession(carts);
            return NoContent();
        }

        [Authorize(Roles ="USER, ADMIN")]
        [HttpPost("/Comment/Create/{id}")]
        public IActionResult AddComment(IFormCollection collection, int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("SignIn", "User");
            }
            else
            {
                string username = User.Claims.FirstOrDefault().Value.ToString();
                commentSvc.CreateComment(collection, username, 0, id);
                return NoContent();
            }
        }

        [Authorize(Roles = "USER, ADMIN")]
        [HttpPatch("/Comment/Delete/{id}")]
        public IActionResult DeleteComment(int id)
        {
            commentSvc.DeleteComment(id);
            return NoContent();
        }

        [HttpPatch("/Update/ProductView/{id}")]
        public IActionResult UpdataProductView(int id)
        {
            productSvc.UpdataView(id);
            return NoContent();
        }
        #endregion


        #region Session
        public List<Cart> GetListCartsInSession()
        {
            var session = HttpContext.Session;
            string jsonCart = session.GetString("carts");
            if (jsonCart != string.Empty && !string.IsNullOrWhiteSpace(jsonCart))
            {
                return JsonConvert.DeserializeObject<List<Cart>>(jsonCart);
            }
            return new List<Cart>();
        }

        public void SaveCartToSession(List<Cart> carts)
        {
            var session = HttpContext.Session;
            string jsonCarts = JsonConvert.SerializeObject(carts);
            session.SetString("carts", jsonCarts);
        }
        #endregion
    }
}

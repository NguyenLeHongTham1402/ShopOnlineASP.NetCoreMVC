using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class CartController : Controller
    {
        
        #region Controller
        public IActionResult ListCarts()
        {
            var carts = GetListCartsInSession();

            return View(carts);
        }
        #endregion


        #region Api Controller

        [Authorize(Roles = "ADMIN, USER")]
        [HttpDelete("/Cart/DeleteCart/{productId}")]
        public IActionResult DeleteCart(int productId)
        {
            var carts = GetListCartsInSession();
            var c = carts.SingleOrDefault(x => x.productId == productId);
            if (c != null)
                carts.Remove(c);
            if (carts.Count == 0)
                ClearCartsInSession();
            else
                SaveCartToSession(carts);
            return NoContent();
        }

        [Authorize(Roles = "ADMIN, USER")]
        [HttpPatch("/Cart/UpdateCart/{productId}")]
        public IActionResult UpdateCart([FromBody] Cart cart, int productId)
        {
            var carts = GetListCartsInSession();

            var c = carts.SingleOrDefault(x => x.productId == productId);

            if (c != null)
            {
                carts.Where(x => x.productId == productId).ToList().ForEach(x => x.quantity = cart.quantity);
            }
            SaveCartToSession(carts);
            return NoContent();
        }
        #endregion


        #region Function
        public List<Cart> GetListCartsInSession()
        {
            var session = HttpContext.Session;
            var jsonCarts = session.GetString("carts");
            List<Cart> carts = null;
            if (!string.IsNullOrWhiteSpace(jsonCarts))
            {
                carts = JsonConvert.DeserializeObject<List<Cart>>(jsonCarts);
            }
            return carts;
        }

        public void SaveCartToSession(List<Cart> carts)
        {
            var session = HttpContext.Session;
            string jsonCarts = JsonConvert.SerializeObject(carts);
            session.SetString("carts", jsonCarts);
        }

        public void ClearCartsInSession()
        {
            var session = HttpContext.Session;
            session.Remove("carts");
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopOnline.Models;
using ShopOnline.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductSvc productSvc = new ProductSvc();
        private readonly CategorySvc categorySvc = new CategorySvc();

        #region Controller

        [Authorize(Roles = "ADMIN")]
        public IActionResult ListProducts()
        {
            var res = productSvc.GetListProduct();
            ViewData["CategoryId"] = new SelectList(categorySvc.ListCategories(), "Id", "Name");
            return View(res);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(categorySvc.ListCategories(), "Id", "Name");
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Create(IFormCollection collection, IFormFile Image)
        {
            string res = productSvc.CreateProduct(collection, Image);
            switch (res)
            {
                case "t":
                    return RedirectToAction("ListProducts");
                case "f":
                    TempData["cProductNoti"] = "Sản phẩm được tạo thất bại.";
                    break;
                case "u":
                    TempData["cProductNoti"] = "Sản phẩm có thể đã tồn tại.";
                    break;
                default:
                    TempData["cProductNoti"] = res;
                    break;
            }
            return this.Create();
        }

        public IActionResult ProductByCate()
        {
            var res = productSvc.GetListProductByCategoryId(1001);
            ViewData["Category_Name"] = categorySvc.GetCategoryById(1001).Name;

            string categoryId = HttpContext.Request.Query["CategoryId"].ToString();
            if (!string.IsNullOrWhiteSpace(categoryId))
            {
                res = productSvc.GetListProductByCategoryId(int.Parse(categoryId));
                ViewData["Category_Name"] = categorySvc.GetCategoryById(int.Parse(categoryId)).Name;
            }
            ViewData["Categories"] = categorySvc.ListCategories();
            
            return View(res);
        }


        #endregion


        #region API Controller Product
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("/Product/Edit/{id}")]
        public IActionResult Update(IFormCollection collection, IFormFile image, int id)
        {
            string res = productSvc.UpdateProduct(collection, image, id);
            switch (res)
            {
                case "t":
                    TempData["eProductNoti"] = "Sản phẩm được cập nhật thành công.";
                    return Ok();
                case "f":
                    TempData["eProductNoti"] = "Sản phẩm được cập nhật thất bại.";
                    return BadRequest();
                default:
                    TempData["eProductNoti"] = res;
                    return StatusCode(500);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("/Product/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            string res = productSvc.DeleteProduct(id);
            switch (res)
            {
                case "t":
                    TempData["dProductNoti"] = "Xóa sản phẩm thành công.";
                    return Ok();
                case "f":
                    TempData["dProductNoti"] = "Xóa sản phẩm thất bại.";
                    return BadRequest();
                case "n":
                    TempData["dProductNoti"] = "Sản phẩm không tồn tại.";
                    return NotFound();
                default:
                    TempData["dProductNoti"] = res;
                    return StatusCode(500);
            }
        } 

        //[HttpGet("/Category/Products/{id}")]
        //public IActionResult ProductByCate(int id)
        //{
        //    var res = productSvc.GetListProductByCategoryId(id);
        //    ViewData["Products-Default"] = null;
        //    ViewData["Products"] = res;
        //    return Redirect("/Product/ProductByCate");
        //}
        #endregion
    }
}

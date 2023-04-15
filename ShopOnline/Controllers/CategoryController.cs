using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class CategoryController : Controller
    {
        private CategorySvc categorySvc = new CategorySvc();

        #region Controller
        [Authorize(Roles = "ADMIN")]
        public IActionResult ListCategories()
        {
            var res = categorySvc.ListCategories();
            return View(res);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult Create(IFormCollection collection)
        {
            string res = categorySvc.CreateCategory(collection);
            string cNoti = string.Empty;
            switch (res)
            {
                case "t":
                    return RedirectToAction("ListCategories");
                case "f":
                    cNoti = "Tạo danh mục thất bại.";
                    break;
                default:
                    cNoti = res;
                    break;
            }
            TempData["CreateCateNoti"] = cNoti;
            return this.Create();
        }
        #endregion


        #region API Controller
        [Authorize(Roles = "ADMIN")]
        [HttpPatch("/Category/Edit/{id}")]
        public IActionResult Edit(IFormCollection collection, int id)
        {
            string res = categorySvc.UpdateCategory(collection, id);

            switch (res)
            {
                case "t":
                    TempData["EditCateNoti"] = "Cập nhật danh mục thành công.";
                    return Ok();
                case "f":
                    TempData["EditCateNoti"] = "Cập nhật danh mục thất bại.";
                    return BadRequest();
                default:
                    TempData["EditCateNoti"] = res;
                    return StatusCode(500);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("/Category/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            string res = categorySvc.DeleteCategory(id);

            switch (res)
            {
                case "t":
                    TempData["DeleteCateNoti"] = "Xóa danh mục thành công.";
                    return Ok();
                case "f":
                    TempData["DeleteCateNoti"] = "Xóa danh mục thất bại.";
                    return BadRequest();
                case "n":
                    TempData["DeleteCateNoti"] = "Không tìm thấy danh mục mà bạn muốn xóa.";
                    return NotFound();
                default:
                    TempData["DeleteCateNoti"] = res;
                    return StatusCode(500);
            }
        } 
        #endregion


    }
}

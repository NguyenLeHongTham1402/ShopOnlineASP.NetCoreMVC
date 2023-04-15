using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopOnline.Models;
using ShopOnline.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ShopOnline.Controllers
{
    public class UserController : Controller
    {
        private UserSvc userSvc = new UserSvc();

        #region Controller
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(IFormCollection collection, IFormFile Avatar)
        {
            string res = userSvc.CreateUser(collection, Avatar);
            string noti = string.Empty;
            switch (res)
            {
                case "t":
                    noti = "Tài khoản đăng ký thành công.";
                    break;
                case "f":
                    noti = "Tài khoản đăng ký thất bại. Vui lòng quay lại sau.";
                    break;
                case "u":
                    noti = "Tên đăng nhập đã được đăng ký. Vui lòng thay đổi tên đăng nhập.";
                    break;
                default:
                    noti = res;
                    break;
            }
            TempData["SignUpNoti"] = noti;
            return this.SignUp();

        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(IFormCollection collection)
        {
            var res = userSvc.Signin(collection);
            if (res != null)
            {
                var claims = new List<Claim> { };
                claims.Add(new Claim(ClaimTypes.NameIdentifier, res.Username));
                claims.Add(new Claim(ClaimTypes.Name, res.Name));
                claims.Add(new Claim(ClaimTypes.Email, res.Email));
                claims.Add(new Claim(ClaimTypes.Role, res.Role));
                claims.Add(new Claim("Avatar", res.Avatar));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
                HttpContext.SignInAsync(claimPrincipal);
                return RedirectToAction("Index", "Home");
            }
            TempData["SignInNoti"] = "Thông tin đăng nhập không hợp lệ. Vui lòng đăng nhập lại hoặc đăng ký tài khoản mới.";
            return this.SignIn();
        }

        public IActionResult SignOut()
        {
            HttpContext.SignOutAsync();
            return Redirect("/");
        }

        #endregion
    }
}

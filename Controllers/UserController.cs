using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>Login(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.Email))
            {
                return Content(  
                    "<script> alert('Invalid Login Credential') </script>"  
                ); 
            }
            var login = await _userService.GetUser(model);
            if (!login.Status)
            {
                ViewBag.Message = login.Message;
                return View();
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,$"{login.Data.UserName}"),
                new Claim(ClaimTypes.NameIdentifier, $"{login.Data.UserId}"),
                new Claim(ClaimTypes.Email, $"{login.Data.Email}"),
                new Claim(ClaimTypes.Role, $"{login.Data.RoleName}")
            };
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authenticationProperty = new AuthenticationProperties();
            var principal = new ClaimsPrincipal(claimIdentity);
            var signIn = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                authenticationProperty);
            //temporary redirection
            if (login.Data.RoleName == "Realtor")
            {
                return RedirectToAction("RealtorDashBoard", "Realtor");
            }
            return RedirectToAction("index", "Home");
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
using System.Collections.Generic;
using System.Security.Claims;
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
            
            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,$"{login.Data.AppUserId}"),
                new Claim(ClaimTypes.NameIdentifier, $"{login.Data.UserId}"),
                new Claim(ClaimTypes.GivenName, $"{login.Data.ProfilePicture}"),
                new Claim(ClaimTypes.Surname,$"{login.Data.UserName}"),
                new Claim(ClaimTypes.Role, $"{login.Data.RoleName}"),
                
            };
            var claimIdentity = new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme);
            var authenticationProperty = new AuthenticationProperties();
            var principal = new ClaimsPrincipal(claimIdentity);
            var signIn = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                authenticationProperty);
            //temporary redirection
            if (login.Data.RoleName == "Realtor")
            {
                return RedirectToAction("DashBoard", "Realtor");
            }
            if (login.Data.RoleName == "Administrator")
            {
                return RedirectToAction("AdminDashBoard", "Admin");
            }
            
            return RedirectToAction("DashBoard", "Buyer");
        }
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
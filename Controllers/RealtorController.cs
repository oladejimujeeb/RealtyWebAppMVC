using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    public class RealtorController : Controller
    {
        private readonly IRealtorService _realtorService;

        public RealtorController(IRealtorService realtorService)
        {
            _realtorService = realtorService;
        }
        // GET
        public IActionResult RealtorRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RealtorRegistration(RealtorRequestModel model)
        {
            var realtor = await _realtorService.RegisterRealtor(model);
            if (realtor.Status)
            {
                return RedirectToAction("Login", "User");
            }

            ViewBag.Message = realtor.Message;
            return View();
        }
        [HttpGet]
        [Authorize (Roles = "Realtor")]
        public IActionResult DashBoard()
        {
            var realtorId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var realtorProperties = _realtorService.GetRealtorApprovedProperty(realtorId);
            return View(realtorProperties.Data);
        }
        [Authorize]
        public IActionResult AddNewProperty()
        {
            return View();
        }

        [HttpPost]
        [Authorize (Roles = "Realtor")]
        public async Task<IActionResult> AddNewProperty(PropertyRequestModel model)
        {
            var realtorId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var property = await _realtorService.AddProperty(model, realtorId);
            if (property.Status)
            {
                return RedirectToAction("DashBoard");
            }

            ViewBag.Message = property.Message;
            return View();
        }
        [Authorize (Roles = "Realtor")]
        public IActionResult UnApprovedProperties()
        {
            var realtorId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var property = _realtorService.GetPropertyByRealtorId(realtorId);
            if (property.Status)
            {
                return View(property.Data);
            }

            ViewBag.Message = property.Message;
            return View();
        }
    }
}
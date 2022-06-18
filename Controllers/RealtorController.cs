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
        [Authorize]
        public IActionResult RealtorDashBoard()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var realtorProperties = _realtorService.GetRealtorApprovedProperty(userId);
            if (!realtorProperties.Status)
            {
                return View(realtorProperties.Message);
            }

            return View(realtorProperties.Data);
        }
        [Authorize]
        public IActionResult AddNewProperty()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewProperty(PropertyRequestModel model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var property = await _realtorService.AddProperty(model, userId);
            if (property.Status)
            {
                return RedirectToAction("RealtorDashBoard");
            }

            ViewBag.Message = property.Message;
            return View();
        }
    }
}
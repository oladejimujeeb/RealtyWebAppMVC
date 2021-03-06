using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.DTOs;
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
            string name = User.FindFirst(ClaimTypes.Surname)?.Value;
            ViewBag.Name = name;
            TempData["profilePic"] = User.FindFirst(ClaimTypes.GivenName).Value;
            var realtorProperties = _realtorService.GetRealtorApprovedProperty(realtorId);
            if (!realtorProperties.Status)
            {
                ViewBag.Message = realtorProperties.Message;
                return View();
            }
            return View(realtorProperties.Data);
        }
        [Authorize (Roles = "Realtor")]
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
        [Authorize(Roles = "Realtor")]
        public async Task<IActionResult> EditProperties(int id )
        {
            var property =await _realtorService.GetProperty(id);
            if (property.Status)
            {
                return View(property.Data);
            }

            return NotFound($"{property.Message}");
        }

        [Authorize(Roles = "Realtor")]
        [HttpPost]
        public async Task<IActionResult> EditProperties(int id, UpdatePropertyModel propertyModel )
        {
            var property =await _realtorService.EditProperty(id,propertyModel);
            ViewBag.Message = property.Message;
            return RedirectToAction("UnApprovedProperties");
        }
        [Authorize(Roles = "Realtor")]
        public async Task<IActionResult> UpdateProfileInfo()
        {
            var realtorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var realtorInfo = await _realtorService.GetUser(realtorId);
            if (realtorInfo.Status)
            {
                
                return View(realtorInfo.Data);
            }

            return NotFound(realtorInfo.Message);

        }
        [HttpPost]
        [Authorize(Roles = "Realtor")]
        public async Task<IActionResult> UpdateProfileInfo( RealtorUpdateRequest requestModel)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var updateUser = await _realtorService.UpdateRealtorInfo(requestModel, userId);
            if (updateUser.Status)
            {
                ViewBag.Message = "Updated Successfully";
                return RedirectToAction("DashBoard");
            }

            return NotFound($"{updateUser.Message}");
        }
       
        [HttpGet]
        [Authorize(Roles = "Realtor")]
        public IActionResult RealtorSoldProperty()
        {
            var realtorId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var property = _realtorService.GetSoldPropertyByRealtor(realtorId);
            if (property.Status)
            {
                return View(property.Data);
            }

            ViewBag.Info = property.Message;
            return View();
        }
    }
}
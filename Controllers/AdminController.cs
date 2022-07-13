using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.DTOs;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet]
        [Authorize (Roles = "Administrator")]
        public IActionResult AdminDashBoard()
        {
            var properties = _adminService.AllUnverifiedProperty();
            
            if (properties.Status)
            {
                TempData["profilePic"] =User.FindFirst(ClaimTypes.GivenName).Value.ToString();
                return View(properties.Data);
            }
            TempData["profilePic"] =User.FindFirst(ClaimTypes.GivenName).Value.ToString();
            ViewBag.Message = properties.Message;
            return View();
        }
        [Authorize (Roles = "Administrator")]
        public IActionResult RegisterNewAdmin()
        {
            return View();
        }
        [HttpPost]
        [Authorize (Roles = "Administrator")]
        public async Task<IActionResult> RegisterNewAdmin(AdminRequestModel model)
        {
            var newAdmin = await _adminService.RegisterAdmin(model);
            if (newAdmin.Status)
            {
                return RedirectToAction("AdminDashBoard");
            }

            ViewBag.Message = newAdmin.Message;
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> VerifyProperty(int id)
        {
            var property = await _adminService.GetPropertyById(id);
            if (property.Status)
            {
                return View(property.Data);
            }

            return NotFound(property.Message);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> VerifyProperty(int id, UpdatePropertyModel updateProperty)
        {
            var property = await _adminService.UpdateRealtorPropertyForSale(id, updateProperty);
            if (property.Status)
            {
                TempData["verify"] = "Done...";
                return RedirectToAction("AdminDashBoard");
            }

            TempData["verify"] = "Verification failed";
            return RedirectToAction("VerifyProperty");
        }
        
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ViewProperty(int id)
        {
            var property = await _adminService.GetPropertyById(id);
            if (property.Status)
            {
                return View(property.Data);
            }

            return NotFound(property.Message);
        }
        
        [Authorize(Roles = "Administrator")]
        public IActionResult VisitationRequests()
        {
            var request = _adminService.VisitationRequest();
            if (request.Status)
            {
                return View(request.Data);
            }

            ViewBag.request = "No visitation Request in the last three days";
            return View();
        }

        [HttpPost]
        public JsonResult Property()
        {
            var properties = _adminService.AllVerifiedProperty();
            if (!properties.Status)
            {
                ViewBag.Message = properties.Message;
            }
            return Json(properties.Data);
        }
        
        [Authorize(Roles = "Administrator")]
        public IActionResult ApprovedProperties()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ViewSoldProperties()
        {
            var properties = _adminService.AllSoldProperty();
            if (!properties.Status)
            {
                ViewBag.Message =properties.Message;
            }

            return Json(properties.Data);
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult SoldProperties()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ViewAllRealtor()
        {
            var realtor = _adminService.AllRealtors();
            if (!realtor.Status)
            {
                ViewBag.Message = "No Registered Realtor";
            }

            return Json(realtor.Data);
        }
        
        [Authorize(Roles = "Administrator")]
        public IActionResult AllRealtor()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ViewAllBuyers()
        {
            var buyers = await _adminService.AllBuyers();
            if (!buyers.Status)
            {
                ViewBag.Message = "No Registered Buyer";
            }

            return Json(buyers.Data);
        }
        
        [Authorize(Roles = "Administrator")]
        public IActionResult AllBuyer()
        {
            return View();
        }
        
        
        [HttpPost]
        public async Task<JsonResult> ViewAllInspections()
        {
            var inspection = await _adminService.AllInspectionRequest();
            if (!inspection.Status)
            {
                ViewBag.Message = inspection.Message;
            }

            return Json(inspection.Data);
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult AllInspectionRequest()
        {
            return View();
        }
    }
}
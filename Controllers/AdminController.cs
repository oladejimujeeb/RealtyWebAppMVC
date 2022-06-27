using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult AdminDashBoard()
        {
            return View();
        }
        
        public async Task<IActionResult> RegisterNewAdmin(AdminRequestModel model)
        {
            var newAdmin = await _adminService.RegisterAdmin(model);
            if (newAdmin.Status)
            {
                return RedirectToAction("AdminDashBoard");
            }

            return View();
        }
    }
}
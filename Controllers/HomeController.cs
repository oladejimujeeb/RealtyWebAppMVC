using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models;

namespace RealtyWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPropertyService _propertyService;

        public HomeController(ILogger<HomeController> logger, IPropertyService propertyService)
        {
            _logger = logger;
            _propertyService = propertyService;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AllProperties()
        {
            var allProperty = _propertyService.AllAvailablePropertyWithImage();
            if (allProperty.Status)
            {
                _logger.LogInformation("loading");
                return View(allProperty.Data);
            }

            return NoContent();
        }
    
        public async Task<IActionResult> Property(int id)
        {
            var property =await _propertyService.GetProperty(id);
            if (property.Status)
            {
                return View(property.Data);
            }
            
            ViewBag.Message = property.Message;
            return NoContent();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.Interface.IServices;
using RealtyWebApp.Models.RequestModel;

namespace RealtyWebApp.Controllers
{
    public class BuyerController : Controller
    {
        private readonly IBuyerService _buyerService;

        public BuyerController(IBuyerService buyerService)
        {
            _buyerService = buyerService;
        }
        // GET
        public IActionResult BuyerRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BuyerRegistration(BuyerRequestModel model)
        {
            var addBuyer = await _buyerService.RegisterBuyer(model);
            if (addBuyer.Status)
            {
                ViewBag.Message = addBuyer.Message;
                return RedirectToAction("Login", "User");
            }
            ViewBag.Message = addBuyer.Message;
            return View();
        }
    }
}
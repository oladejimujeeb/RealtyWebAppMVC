using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> BookInspectionDate( int id)
        {
            var buyerId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var dateRequest = await _buyerService.MakeVisitationRequest(buyerId,id);
            if (dateRequest.Status)
            {
                return View(dateRequest.Data);
            }

            return NotFound("You can not make Inspection date\nCall Customer service on +2348136794915 or Send " +
                            "Mail to oladejimujeeb@yahoo.com");
        }
         [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> DownloadPropertyDocument(int id)
        {
            var file = await _buyerService.DownloadPropertyDocument(id);
            if (file.Status)
            {
                return File(file.Data.Data, file.Data.FileType, file.Data.DocumentPath + file.Data.Extension);
                //return File(Url.Content("~/Files/text.txt"), "text/plain", "testFile.txt");
            }

            ViewBag.download = file.Message;
            return null;
        }
    }
}
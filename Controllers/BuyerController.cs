using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtyWebApp.DTOs;
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
        public async Task<IActionResult> BookInspectionDate(int id)
        {
            var buyerId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var dateRequest = await _buyerService.MakeVisitationRequest(buyerId, id);
            if (dateRequest.Status)
            {
                return View(dateRequest.Data);
            }

            ViewBag.Message = dateRequest.Message;
            return View();
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

        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> Dashboard()
        {
            var buyerId = int.Parse(User.FindFirst(ClaimTypes.Name).Value);
            var inspectedProperty = await _buyerService.ListOfBuyerVisitedProperty(buyerId);
            string name = User.FindFirst(ClaimTypes.Surname)?.Value;
            ViewBag.Name = name;
            TempData["profilePic"] = User.FindFirst(ClaimTypes.GivenName).Value;
            if (inspectedProperty.Status)
            {
                return View(inspectedProperty.Data);
            }

            ViewBag.Message = inspectedProperty.Message;
            return View();
        }

        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> GetProperty(string id)
        {
            var property = await _buyerService.GetProperty(id);
            if (property.Status)
            {
                return View(property.Data);
            }

            ViewBag.Message = property.Message;
            return View();
        }

        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> UpdateProfileInfo()
        {
            var buyerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var buyerInfo = await _buyerService.GetBuyer(buyerId);
            if (buyerInfo.Status)
            {

                return View(buyerInfo.Data);
            }

            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> UpdateProfileInfo( UpdateBuyerModel model)
        {
            var buyerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var update = await _buyerService.UpdateBuyer(buyerId, model);
            if (update.Status)
            {
                ViewBag.Message = update.Message;
                return RedirectToAction("Dashboard");
            }

            return NotFound();
        }

        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> PaymentBreakDown(int id)
        {
            var buyerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var breakdown = await _buyerService.PaymentBreakDown(id, buyerId);
            if (breakdown.Status)
            {
                return View(breakdown.Data);
            }

            ViewBag.Message = breakdown.Message;
            return View();
        }

        public async Task<IActionResult> ProcessPayment(PaymentRequestModel model)
        {
            var buyerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var payment = await _buyerService.MakePayment(buyerId, model);
            if (payment.Status)
            {
                return Redirect(payment.Message);
            }

            ViewData["PaymentError"] = payment.Message;
            return View("PaymentBreakDown");
        }

        public async Task<IActionResult> VerifyPayment(string reference)
        {
            var verify = await _buyerService.VerifyPayment(reference);
            if (verify.Status)
            {
                ViewData["PaymentSucceed"] = verify.Message;
                return View("PaymentBreakDown");
            }
            ViewData["PaymentError"] = verify.Message;
            return View("PaymentBreakDown");
        }
    }
}
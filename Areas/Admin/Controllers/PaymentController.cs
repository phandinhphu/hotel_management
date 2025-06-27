using Microsoft.AspNetCore.Mvc;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index(string searchTerm = "", int pageIndex = 1)
        {
            try
            {
                var payments = await _paymentService.GetAllAsync(searchTerm, pageIndex);
                ViewBag.SearchTerm = searchTerm;
                ViewBag.CurrentPage = pageIndex;
                return View(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy danh sách thanh toán.");
                return Json(new { error = true, message = "Không thể lấy danh sách thanh toán" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var payment = await _paymentService.GetByIdAsync(id);
                if (payment == null) return NotFound();
                return PartialView("Details", payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy chi tiết thanh toán.");
                return Json(new { error = true, message = "Không thể lấy chi tiết thanh toán" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _paymentService.DeleteAsync(id);
                if (result) TempData["SuccessMessage"] = "Xóa thanh toán thành công!";
                else TempData["ErrorMessage"] = "Xóa thanh toán thất bại!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi xóa thanh toán.");
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

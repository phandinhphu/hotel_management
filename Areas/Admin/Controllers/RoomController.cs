using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class RoomController : Controller
    {
        private readonly ILogger<RoomController> _logger;
        private readonly IRoomService _roomService;

        public RoomController(ILogger<RoomController> logger, IRoomService roomService)
        {
            _logger = logger;
            _roomService = roomService;
        }

        public async Task<IActionResult> IndexAsync(
            string searchTerm, 
            string roomType, 
            string status, 
            decimal? minPrice, 
            decimal? maxPrice, 
            int pageIndex = 1)
        {
            try
            {
                // Get all available room types for the dropdown
                var roomTypes = await _roomService.GetAllRoomTypesAsync();
                ViewBag.RoomTypes = roomTypes;

                // Get filtered rooms
                var rooms = await _roomService.GetAllAsync(
                    searchTerm, 
                    roomType, 
                    status, 
                    minPrice, 
                    maxPrice, 
                    pageIndex, 
                    8);

                // Store filter values in ViewBag to maintain state
                ViewBag.CurrentSearchTerm = searchTerm;
                ViewBag.CurrentRoomType = roomType;
                ViewBag.CurrentStatus = status;
                ViewBag.CurrentMinPrice = minPrice;
                ViewBag.CurrentMaxPrice = maxPrice;
                ViewBag.CurrentPageIndex = pageIndex;

                if (rooms == null || !rooms.Any())
                {
                    _logger.LogInformation("Không có phòng nào được tìm thấy.");
                }
                return View(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy danh sách phòng.");
                return View("Error", new { message = "Không thể lấy danh sách phòng. Vui lòng thử lại sau." });
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(RoomVM roomVM)
        {
            if (!ModelState.IsValid) return View(roomVM);

            try
            {
                var result = await _roomService.CreateAsync(roomVM);
                if (result) 
                    TempData["SuccessMessage"] = $"Thêm phòng số '{roomVM.RoomNumber}' thành công!";
                else TempData["ErrorMessage"] = $"Thêm phòng số '{roomVM.RoomNumber}' thất bại!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo phòng.");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(roomVM);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var room = await _roomService.GetByIdAsync(id);
                if (room == null)
                {
                    _logger.LogWarning($"Không tìm thấy phòng với ID {id}.");
                    return NotFound();
                }
                return View(room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin phòng.");
                return View("Error", new { message = "Không thể lấy thông tin phòng. Vui lòng thử lại sau." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(RoomVM roomVM)
        {
            if (!ModelState.IsValid) return View(roomVM);
            try
            {
                var result = await _roomService.UpdateAsync(roomVM);
                if (result)
                    TempData["SuccessMessage"] = $"Cập nhật phòng số '{roomVM.RoomNumber}' thành công!";
                else TempData["ErrorMessage"] = $"Cập nhật phòng số '{roomVM.RoomNumber}' thất bại!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật phòng.");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(roomVM);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _roomService.DeleteAsync(id);
                if (result) TempData["SuccessMessage"] = $"Xóa phòng thành công!";
                else TempData["ErrorMessage"] = $"Xóa phòng thất bại!";
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Không thể xóa phòng do đã có người đặt.");
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi xóa phòng.");
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }


    }
}

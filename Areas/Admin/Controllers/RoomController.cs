using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Areas.Admin.Services.Interfaces;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                    20);

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
        [HttpPost]
        public async Task<IActionResult> CreateAsync(RoomVM roomVM, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Dữ liệu không hợp lệ",
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });
            }

            try
            {
                // Process image upload if provided
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Save image to /wwwroot/images/room folder
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "room", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    roomVM.Image = fileName;
                }
                else
                {
                    // Set default image
                    roomVM.Image = "room-default.png";
                }

                // Save to database
                var roomId = await _roomService.CreateAsync(roomVM);

                return Json(new { success = true, message = "Tạo phòng thành công", roomId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo phòng.");
                return Json(new { success = false, message = "Đã xảy ra lỗi: " + ex.Message });
            }
        }






    }
}

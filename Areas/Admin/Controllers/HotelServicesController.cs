using AutoMapper;
using Hotel_Management.Areas.Admin.Services;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class HotelServicesController : Controller
    {
        private readonly IHotelSServices _hotelSServices;
        private readonly IMapper _mapper;

        public HotelServicesController(IHotelSServices hotelSServices, IMapper mapper)
        {
            _hotelSServices = hotelSServices;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string? QuerySearch { get; set; } = string.Empty;

        // [GET] /Admin/HotelServices
        public async Task<IActionResult> Index()
        {
            var services = await _hotelSServices.GetAllAsync(QuerySearch, PageIndex);

            ViewBag.QuerySearch = QuerySearch;

            return View(services);
        }

        // [GET] /Admin/HotelServices/Detail/{id}
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            try
            {
                var service = await _hotelSServices.GetByIdAsync(id);
                return View(service);
            }
            catch (ArgumentNullException ex)
            {
                // Log the exception (ex) if necessary
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Log the exception (ex) if necessary
                return BadRequest(ex.Message);
            }
        }

        // [GET] /Admin/HotelServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // [POST] /Admin/HotelServices/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelServicesVM model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var service = _mapper.Map<Service>(model);
                    await _hotelSServices.CreateAsync(service);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(model);
        }

        // [GET] /Admin/HotelServices/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var service = await _hotelSServices.GetByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            var model = _mapper.Map<HotelServicesVM>(service);
            return View(model);
        }

        // [POST] /Admin/HotelServices/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HotelServicesVM model)
        {
            if (id <= 0 || id != model.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var service = _mapper.Map<Service>(model);
                    await _hotelSServices.UpdateAsync(service);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(model);
        }

        // [POST] /Admin/HotelServices/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            try
            {
                await _hotelSServices.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", await _hotelSServices.GetAllAsync(QuerySearch, PageIndex));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using AutoMapper;
using Hotel_Management.Areas.Admin.ViewModels;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class HotelfacilitiesController : Controller
    {
        private readonly IHotelfacilitiesServices _hotelfacilitiesServices;
        private readonly IHotelServices _hotelServices;
        private readonly IMapper _mapper;

        public HotelfacilitiesController(
            IHotelfacilitiesServices hotelfacilitiesServices,
            IHotelServices hotelServices,
            IMapper mapper)
        {
            _hotelfacilitiesServices = hotelfacilitiesServices;
            _hotelServices = hotelServices;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        // [GET] Admin/Hotelfacilities
        public async Task<IActionResult> Index()
        {
            var hotelManagementContext = await _hotelfacilitiesServices.GetAllAsync(PageIndex);

            return View(hotelManagementContext);
        }

        // [GET] Admin/Hotelfacilities/Detail/5
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var hotelfacilities = await _hotelfacilitiesServices.GetByIdAsync(id.Value);
                return View(hotelfacilities);
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

        // [GET] Admin/Hotelfacilities/Create
        public IActionResult Create()
        {
            ViewData["HotelId"] = new SelectList(_hotelServices.GetAllAsync().Result, "Id", "Name");
            return View();
        }

        // POST: Admin/Hotelfacilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelfacilitiesVM hotelfacility)
        {
            if (ModelState.IsValid)
            {
                var hotelfacilityEntity = _mapper.Map<Hotelfacility>(hotelfacility);

                var result = await _hotelfacilitiesServices.CreateAsync(hotelfacilityEntity);
                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Failed to create hotelfacility. Please try again.");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            ViewData["HotelId"] = new SelectList(_hotelServices.GetAllAsync().Result, "Id", "Name", hotelfacility.HotelId);
            return View(hotelfacility);
        }

        // [GET] Admin/Hotelfacilities/Edit/{id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var hotelfacility = await _hotelfacilitiesServices.GetByIdAsync(id.Value);
                if (hotelfacility == null)
                {
                    return NotFound();
                }

                ViewData["HotelId"] = new SelectList(_hotelServices.GetAllAsync().Result, "Id", "Name", hotelfacility.HotelId);
                return View(_mapper.Map<HotelfacilitiesVM>(hotelfacility));
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

        // POST: Admin/Hotelfacilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(HotelfacilitiesVM hotelfacility)
        {
            if (ModelState.IsValid)
            {
                var hotelfacilityEntity = _mapper.Map<Hotelfacility>(hotelfacility);

                var result = await _hotelfacilitiesServices.UpdateAsync(hotelfacilityEntity);
                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Failed to update hotelfacility. Please try again.");
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            ViewData["HotelId"] = new SelectList(_hotelServices.GetAllAsync().Result, "Id", "Name", hotelfacility.HotelId);
            return View(hotelfacility);
        }

        // [POST] Admin/Hotelfacilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var hotelfacility = await _hotelfacilitiesServices.DeleteAsync(id.Value);
                if (!hotelfacility)
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
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
    }
}

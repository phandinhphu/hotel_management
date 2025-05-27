using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StaffController : Controller
    {
        private readonly IStaffServices _staffServices;
        private readonly UserManager<ApplicationUser> _userManager;

        public StaffController(IStaffServices staffServices, UserManager<ApplicationUser> userManager)
        {
            _staffServices = staffServices;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string? QuerySearch { get; set; } = string.Empty;

        // [GET] /Admin/Staff/Index
        public async Task<IActionResult> Index()
        {
            var customers = await _staffServices.getAllAsync(QuerySearch, PageIndex);

            return View(customers);
        }

        // [GET] /Admin/Staff/Detail/{id}
        public async Task<IActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var customer = await _staffServices.getByIdAsync(id);
                return View(customer);
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

        // [GET] /Admin/Staff/Create
        public IActionResult Create()
        {
            return View();
        }

        // [POST] /Admin/Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM customer)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(customer.UserName);
                Console.WriteLine(customer.Email);
                var user = new ApplicationUser
                {
                    UserName = customer.UserName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    EmailConfirmed = true
                };


                var result = await _userManager.CreateAsync(user, customer.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Staff");
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            return View(customer);
        }

        // [GET] /Admin/Staff/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var staff = await _staffServices.getByIdAsync(id);
                if (staff == null)
                {
                    return NotFound();
                }
                var editStaffVM = new EditUserVM
                {
                    UserId = staff.Id,
                    UserName = staff.UserName ?? string.Empty,
                    Email = staff.Email ?? string.Empty,
                    PhoneNumber = staff.PhoneNumber ?? string.Empty
                };
                return View(editStaffVM);
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

        // [POST] /Admin/Staff/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserVM staff)
        {
            if (string.IsNullOrEmpty(staff.UserId))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingStaff = await _staffServices.getByIdAsync(staff.UserId);
                    if (existingStaff == null)
                    {
                        return NotFound();
                    }

                    existingStaff.UserName = staff.UserName;
                    existingStaff.Email = staff.Email;
                    existingStaff.PhoneNumber = staff.PhoneNumber;

                    var result = await _userManager.UpdateAsync(existingStaff);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
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
            return View(staff);
        }

        // [POST] /Admin/Staff/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            try
            {
                var staff = await _staffServices.getByIdAsync(id);
                if (staff == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(staff);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
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
            return RedirectToAction("Index");
        }
    }
}

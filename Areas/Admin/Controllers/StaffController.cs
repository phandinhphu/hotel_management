using AutoMapper;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class StaffController : Controller
    {
        private readonly IUserServices _userServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public StaffController(
            Func<string, IUserServices> userServiceFactory,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _userServices = userServiceFactory("staff");
            _userManager = userManager;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string? QuerySearch { get; set; }

        // [GET] /Admin/Staff/Index
        public async Task<IActionResult> Index()
        {
            var customers = await _userServices.getAllAsync(QuerySearch, PageIndex);

            ViewBag.QuerySearch = QuerySearch;

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
                var customer = await _userServices.getByIdAsync(id);
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
                var user = _mapper.Map<ApplicationUser>(customer);


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
                var staff = await _userServices.getByIdAsync(id);
                if (staff == null)
                {
                    return NotFound();
                }

                var editStaffVM = _mapper.Map<EditUserVM>(staff);

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
                    var existingStaff = await _userServices.getByIdAsync(staff.UserId);
                    if (existingStaff == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(staff, existingStaff);

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
                var staff = await _userServices.getByIdAsync(id);
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

        // [GET] /Admin/Staff/ResetPassword/{id}
        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            try
            {
                var staff = await _userServices.getByIdAsync(id);
                if (staff == null)
                {
                    return NotFound();
                }
                var passwordVM = _mapper.Map<PasswordVM>(staff);

                return View(passwordVM);
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

        // [POST] /Admin/Staff/ResetPassword/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(PasswordVM passwordVM)
        {
            if (string.IsNullOrEmpty(passwordVM.UserId))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var staff = await _userServices.getByIdAsync(passwordVM.UserId);
                    if (staff == null)
                    {
                        return NotFound();
                    }

                    var result = await _userServices.ResetPasswordAsync(passwordVM.UserId, passwordVM.NewPassword);

                    if (result)
                    {
                        TempData.Clear();

                        TempData["SuccessMessage"] = "Đặt lại mật khẩu thành công.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Đặt lại mật khẩu không thành công. Vui lòng thử lại.");
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
            return View(passwordVM);
        }
    }
}

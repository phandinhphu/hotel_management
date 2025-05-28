using AutoMapper;
using Hotel_Management.Areas.Admin.Services.Interfaces;
using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly ICustomerServices _customerServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public CustomerController(
            ICustomerServices customerServices,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _customerServices = customerServices;
            _userManager = userManager;
            _mapper = mapper;
        }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public string? QuerySearch { get; set; } = string.Empty;

        // [GET] /Admin/Customer/Index
        public async Task<IActionResult> Index()
        {
            var customers = await _customerServices.getAllAsync(QuerySearch, PageIndex);

            return View(customers);
        }

        // [GET] /Admin/Customer/Detail/{id}
        public async Task<IActionResult> Detail(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var customer = await _customerServices.getByIdAsync(id);
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

        // [GET] /Admin/Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // [POST] /Admin/Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM customer)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine(customer.UserName);
                Console.WriteLine(customer.Email);
                var user = _mapper.Map<ApplicationUser>(customer);


                var result = await _userManager.CreateAsync(user, customer.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Customer");
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

        // [GET] /Admin/Customer/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var customer = await _customerServices.getByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                var editCustomerVM = _mapper.Map<EditUserVM>(customer);

                return View(editCustomerVM);
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

        // [POST] /Admin/Customer/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserVM customer)
        {
            if (string.IsNullOrEmpty(customer.UserId))
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCustomer = await _customerServices.getByIdAsync(customer.UserId);
                    if (existingCustomer == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(customer, existingCustomer);

                    var result = await _userManager.UpdateAsync(existingCustomer);

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
            return View(customer);
        }

        // [POST] /Admin/Customer/Delete/{id}
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
                var customer = await _customerServices.getByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(customer);

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

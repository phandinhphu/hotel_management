using Hotel_Management.Areas.Admin.ViewModels;
using Hotel_Management.Models;
using Hotel_Management.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotel_Management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;

        public RoleController(
            UserManager<ApplicationUser> userManager,
            IUserService userService,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        [BindProperty(SupportsGet = true)]
        public string? SelectedRole { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? QuerySearch { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        // [GET] /Admin/Role/Index
        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUsersWithRoleAsync(SelectedRole, QuerySearch, PageIndex, 20);

            ViewBag.SelectedRole = SelectedRole;
            ViewBag.QuerySearch = QuerySearch;

            return View(user);
        }

        // [GET] /Admin/Role/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList();

            var vm = new EditRoleUserVM
            {
                Id = user.Id,
                Email = user.Email,
                CurrentRole = currentRoles.FirstOrDefault(),
                SelectedRole = currentRoles.FirstOrDefault(),
                AvailableRoles = allRoles
            };

            return View(vm);
        }

        // [POST] /Admin/Role/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleUserVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.Id);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);
            var currentRole = currentRoles.FirstOrDefault();

            // Chỉ đổi khi khác role
            if (vm.SelectedRole != currentRole)
            {
                if (currentRole != null)
                    await _userManager.RemoveFromRoleAsync(user, currentRole);

                if (!string.IsNullOrEmpty(vm.SelectedRole))
                    await _userManager.AddToRoleAsync(user, vm.SelectedRole);
            }

            return RedirectToAction("Index");
        }
    }
}

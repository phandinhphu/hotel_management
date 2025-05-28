using System.ComponentModel.DataAnnotations;
using Hotel_Management.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class EditRoleUserVM : ApplicationUser
    {
        [Display(Name = "Quyền hiện tại")]
        public string? CurrentRole { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quyền mới")]
        [Display(Name = "Quyền mới")]
        public required string SelectedRole { get; set; }

        public List<SelectListItem>? AvailableRoles { get; set; }
    }
}

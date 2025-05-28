using Hotel_Management.Models;

namespace Hotel_Management.Areas.Admin.ViewModels
{
    public class UserAndRoleVM : ApplicationUser
    {
        public string? Role { get; set; }
    }
}

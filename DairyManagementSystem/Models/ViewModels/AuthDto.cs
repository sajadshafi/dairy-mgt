using System.ComponentModel.DataAnnotations;

namespace DairyManagementSystem.Models.ViewModels
{
    public class LoginDto
    {

        [EmailAddress, Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [MinLength(8), Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }

    public class RoleDto
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DairyManagementSystem.Models
{
    public class ResetPassword
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }
    }
}

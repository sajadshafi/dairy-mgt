using System.ComponentModel.DataAnnotations;

namespace DairyManagementSystem.Models.ViewModels {
   public class PasswordChangeVM {

      [Required]
      public string OldPassword { get; set; }

      [Required]
      public string NewPassword { get; set; }

      [Required]
      [Compare(nameof(NewPassword), ErrorMessage = "Password does not match!")]
      public string ConfirmNewPassword { get; set; }
   }
}

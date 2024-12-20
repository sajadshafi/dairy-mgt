using System.ComponentModel.DataAnnotations;

namespace DairyManagementSystem.Models.ViewModels {
   public class UserProfileVM {
      public Guid Id { get; set; }

      [Required(ErrorMessage = "Name is required")]
      public string Name { get; set; }

      [Required(ErrorMessage = "Email is requred"), EmailAddress(ErrorMessage = "Please enter a valid email address")]
      public string Email { get; set; }

      [Required(ErrorMessage = "Phone is required")]
      public string Phone { get; set; }
      public string Address { get; set; }
      public IFormFile ProfileImage { get; set; }
      public byte[] ImageArray { get; set; }

      [Required(ErrorMessage = "Username is required")]
      public string UserName { get; set; }
   }
}

using System.ComponentModel.DataAnnotations;

namespace DairyManagementSystem.Models.ViewModels {
   public class CustomerModel {
      public Guid Id { get; set; }

      [Required(ErrorMessage = "Name is required")]
      public string Name { get; set; }

      [Required(ErrorMessage = "Email is requred"), EmailAddress(ErrorMessage = "Please enter a valid email address")]
      public string Email { get; set; }

      [Required(ErrorMessage = "Phone is required")]
      public string Phone { get; set; }
      public string Address { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username cannot be less than 3 characters")]
        [MaxLength(25, ErrorMessage = "Username cannot be more than 25 characters")]
        [RegularExpression(@"^[A-Za-z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores, spaces not allowed")]
        [NoSpace(ErrorMessage = "Username cannot contain spaces")]
        public string UserName { get; set; }
   }
}

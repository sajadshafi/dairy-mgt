using System.ComponentModel.DataAnnotations;

namespace DairyManagementSystem.Models.ViewModels {
   public class SupplierModel {
      public Guid Id { get; set; }

      [Required(ErrorMessage = "Name is required")]
      public string Name { get; set; }
      public string Address { get; set; }

      [Required(ErrorMessage = "Phone is required")]
      public string Phone { get; set; }

      [Required(ErrorMessage = "Email is requred"), EmailAddress(ErrorMessage = "Please enter a valid email address")]
      public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username cannot be less than 3 characters")]
        [MaxLength(25, ErrorMessage = "Username cannot be more than 25 characters")]
        [RegularExpression(@"^[A-Za-z0-9_]+$", ErrorMessage = "Username can only contain letters, numbers, and underscores, spaces not allowed")]
        [NoSpace(ErrorMessage = "Username cannot contain spaces")]
        public string UserName { get; set; }
      public string Password { get; set; }

      public IEnumerable<DropdownItem> Customers { get; set; }
      public List<CustomerModel> SupplierCustomers { get; set; }
   }
    public class NoSpaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var userName = value.ToString();
                if (userName.Contains(" "))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace DairyManagementSystem.Models.ViewModels {
   public class ProductModel {
      public Guid Id { get; set; }

      [Required(ErrorMessage = "Product name is required")]
      public string ProductName { get; set; }

      [Required(ErrorMessage = "Price is required")]
      public decimal Price { get; set; }

      [Required(ErrorMessage = "Quantity is required")]
      public float Quantity { get; set; }

      [Required(ErrorMessage = "Please select a unit")]
      public Guid UnitId { get; set; }
      public string UnitName { get; set; }
   }
}

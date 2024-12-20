using System.ComponentModel.DataAnnotations;

namespace DairyManagementSystem.Models.ViewModels {
   public class SalesVM {
      public Guid Id { get; set; }
      public Guid SellerId { get; set; }

      [Required(ErrorMessage = "Please select a customer.")]
      public Guid BuyerId { get; set; }

      [Required(ErrorMessage = "Quantity is required")]
      public int Quantity { get; set; }

      [Required(ErrorMessage = "Please select a product.")]
      public Guid ProductId { get; set; }
      public DateTime Date { get; set; }
   }
}

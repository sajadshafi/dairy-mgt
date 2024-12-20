using System.ComponentModel.DataAnnotations.Schema;

namespace DairyManagementSystem.Models {
   public class Sales : BaseEntity {
      /// <summary>
      /// Seller is the Supplier who will be selling the product
      /// </summary>
      public Guid SellerId { get; set; }

      /// <summary>
      /// Buyer will be the customer who purchases
      /// </summary>
      public Guid BuyerId { get; set; }

      public int Quantity { get; set; }
      public decimal CurrentPrice { get; set; }
      public decimal TotalAmount { get; set; }

      public Guid ProductId { get; set; }
      public DateTime Date { get; set; }

      [ForeignKey(nameof(ProductId))]
      public Product Product { get; set; }

      [ForeignKey(nameof(BuyerId))]
      public SystemUser Buyer { get; set; }
   }
}

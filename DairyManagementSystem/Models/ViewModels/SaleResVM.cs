namespace DairyManagementSystem.Models.ViewModels {
   public class SaleResVM {
      public Guid SaleId { get; set; }
      public string BuyerName { get; set; }
      public string SellerName { get; set; }
      public string ProductName { get; set; }
      public decimal Quantity { get; set; }
      public string UnitSymbol { get; set; }
      public decimal PricePerUnit { get; set; }
      public decimal TotalAmount { get; set; }
      public DateTime Date { get; set; }
   }
}

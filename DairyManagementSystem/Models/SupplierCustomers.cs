namespace DairyManagementSystem.Models {
   public class SupplierCustomers {
      public Guid Id { get; set; }
      public Guid SupplierId { get; set; }
      public Guid CustomerId { get; set; }
      public bool IsDeleted { get; set; }
        public bool IsSeller { get; set; }
        public bool IsBuyer { get; set; }
    }
}

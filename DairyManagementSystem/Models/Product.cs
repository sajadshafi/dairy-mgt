using System.ComponentModel.DataAnnotations.Schema;

namespace DairyManagementSystem.Models {
   public class Product : BaseEntity {
      public string ProductName { get; set; }
      public decimal Price { get; set; }
      public float Quantity { get; set; }
      public Guid MeasurementUnitId { get; set; }

      [ForeignKey(nameof(MeasurementUnitId))]
      public MeasurementUnit MeasurementUnit { get; set; }
   }
}

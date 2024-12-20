﻿namespace DairyManagementSystem.Models {
   public class BaseEntity : IBaseEntity {
      public Guid Id { get; set; }
      public Guid CreatedBy { get; set; }
      public Guid ModifiedBy { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime ModifiedDate { get; set; }
      public bool IsDeleted { get; set; } = false;
   }
}

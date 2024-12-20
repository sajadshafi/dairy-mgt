using Microsoft.AspNetCore.Identity;

namespace DairyManagementSystem.Models {
   public class SystemUser : IdentityUser<Guid> {
      public string Name { get; set; }
      public string Address { get; set; }
      public byte[] ProfileImage { get; set; }
      public Guid CreatedBy { get; set; }
      public Guid ModifiedBy { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime ModifiedDate { get; set; }
      public bool IsDeleted { get; set; }
   }

   public class SystemRole : IdentityRole<Guid> {
      public Guid CreatedBy { get; set; }
      public Guid ModifiedBy { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime ModifiedDate { get; set; }
      public bool IsDeleted { get; set; }
   }

   public class SystemUserRole: IdentityUserRole<Guid> {
      public Guid CreatedBy { get; set; }
      public Guid ModifiedBy { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime ModifiedDate { get; set; }
      public bool IsDeleted { get; set; }
   }


}

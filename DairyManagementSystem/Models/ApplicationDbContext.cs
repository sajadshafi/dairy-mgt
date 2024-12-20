using DairyManagementSystem.DbConfigurations;
using DairyManagementSystem.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DairyManagementSystem.Models {
   public class ApplicationDbContext : IdentityDbContext<SystemUser, SystemRole, Guid, IdentityUserClaim<Guid>, SystemUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>> {
      public DbSet<Product> Products { get; set; }
      public DbSet<SupplierCustomers> SupplierCustomers { get; set; }
      public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
      public DbSet<Sales> Sales { get; set; }
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) {

      }

      protected override void OnModelCreating(ModelBuilder builder) {

         foreach(var type in builder.Model.GetEntityTypes()) {
            if(typeof(IBaseEntity).IsAssignableFrom(type.ClrType))
               builder.SetSoftDeleteFilter(type.ClrType);
         }

         Guid roleId = Guid.NewGuid();
         Guid userId = Guid.NewGuid();
         string email = InitialAdminUser.EMAIL;
         string username = InitialAdminUser.USERNAME;
         string password = InitialAdminUser.PASSWORD;

         builder.Entity<SystemRole>().HasData(new SystemRole {
            Id = roleId,
            Name = RoleConstants.ADMIN,
            NormalizedName = RoleConstants.ADMIN.ToUpper(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
         });
         builder.Entity<SystemRole>().HasData(new SystemRole {
            Id = Guid.NewGuid(),
            Name = RoleConstants.SUPPLIER,
            NormalizedName = RoleConstants.SUPPLIER.ToUpper(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
         }); ; ;
         builder.Entity<SystemRole>().HasData(new SystemRole {
            Id = Guid.NewGuid(),
            Name = RoleConstants.CUSTOMER,
            NormalizedName = RoleConstants.CUSTOMER.ToUpper(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
         });

         var hasher = new PasswordHasher<SystemUser>();
         builder.Entity<SystemUser>().HasData(new SystemUser {
            Id = userId,
            Name = InitialAdminUser.NAME,
            UserName = username,
            NormalizedUserName = username.ToUpper(),
            Email = email,
            NormalizedEmail = email.ToUpper(),
            EmailConfirmed = true,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            PasswordHash = hasher.HashPassword(null, password),
            SecurityStamp = string.Empty
         });

         builder.Entity<SystemUserRole>().HasData(new SystemUserRole {
            RoleId = roleId,
            UserId = userId,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
         });

         base.OnModelCreating(builder);
      }

      public override int SaveChanges() {
         UpdateSoftDeleteStatuses();
         return base.SaveChanges();
      }
      public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default) {
         UpdateSoftDeleteStatuses();
         return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
      }
      private void UpdateSoftDeleteStatuses() {
         foreach(var entry in ChangeTracker.Entries()) {
            switch(entry.State) {
               case EntityState.Added:
                  entry.CurrentValues["IsDeleted"] = false;
                  break;
               case EntityState.Deleted:  // remove
                  entry.State = EntityState.Modified;
                  entry.CurrentValues["IsDeleted"] = true;
                  break;
            }
         }
      }
   }
}

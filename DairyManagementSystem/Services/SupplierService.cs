using DairyManagementSystem.EmailConfig;
using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DairyManagementSystem.Services {
   public class SupplierSerivce : ISupplierService {

      #region Private Fields
      private readonly ApplicationDbContext _context;
      private readonly ILogger<SupplierSerivce> _logger;
      private readonly GlobalHelper _globalHelper;
      private readonly UserManager<SystemUser> _userManager;
      private readonly IEmailService _emailService;
      #endregion

      #region Constructors
      public SupplierSerivce(
         ApplicationDbContext context,
         ILogger<SupplierSerivce> logger,
         GlobalHelper globalHelper,
         IEmailService emailService,
         UserManager<SystemUser> userManager
         ) {
         _context = context;
         _logger = logger;
         _globalHelper = globalHelper;
         _emailService = emailService;
         _userManager = userManager;
      }
      #endregion

      public async Task<Guid?> DeleteAsync(Guid id) {
         SystemUser user = await _userManager.FindByIdAsync(id.ToString());

         if(user == null) return null;

         Guid deletedId = user.Id;
         IdentityResult result = await _userManager.DeleteAsync(user);
         await _context.SaveChangesAsync();
         return deletedId;
      }

      public async Task<List<SupplierModel>> GetAsync(Guid? id = null) {
         List<SupplierModel> suppliers = new();
         if(id != null) {
            SystemUser user = await _userManager.FindByIdAsync(id.ToString());
            if(user.IsDeleted) return default;
            SupplierModel model = new();
            MapEntityToVM(user, model);
            suppliers.Add(model);
         } else {
            IList<SystemUser> supplierList = await _userManager.GetUsersInRoleAsync(RoleConstants.SUPPLIER);
            supplierList = supplierList.Where(x => !x.IsDeleted).ToList();
            foreach(SystemUser sup in supplierList) {
               SupplierModel model = new();
               MapEntityToVM(sup, model);
               suppliers.Add(model);
            }
         }
         return suppliers;
      }
      
      public async Task<bool> SaveAsync(SupplierModel model) {
         try {
            if(model == null) {
               return false;
            }

            // Generate a random password with 10 characters
            string password = PasswordGenerator.GeneratePassword(10);
            model.Password = password;
            if(model.Id == Guid.Empty) {
               // Create a new customer
               SystemUser user = new();
               MapVMToEntity(model, user);

               // Set the user's password
               IdentityResult result = await _userManager.CreateAsync(user, password);
               if(!result.Succeeded) {
                  // Log any errors when creating the user
                  _logger.LogError("Error creating user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                  return false;
               }

               // Add the user to the SUPPLIER role
               result = await _userManager.AddToRoleAsync(user, RoleConstants.SUPPLIER);
               if(!result.Succeeded) {
                  // Log any errors when adding the user to the role
                  _logger.LogError("Error adding user to role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                  return false;
               }

               // Prepare and send the invitation email
               EmailModel emailModel = new() {
                  EmailTo = model.Email,
                  Subject = "Invitation to Dairy Management System",
                  Body = EmailTemplates.CreateInvitationTemplate(model.Email, password, model.Name, "localhost"),
               };

               // Send the email
               bool emailSent = await _emailService.SendEmailAsync(emailModel);
               if(!emailSent) {
                  // Log any errors when sending the email
                  _logger.LogError("Error sending invitation email.");
                  return false;
               }
            } else {
               // Update an existing customer
               SystemUser existingUser = await _userManager.FindByIdAsync(model.Id.ToString());
               if(existingUser != null) {
                  MapVMToEntity(model, existingUser);

                  await _userManager.UpdateAsync(existingUser);
               }
            }

            return true;
         } catch(Exception ex) {
            _logger.LogError("An Error has occurred", ex.ToString());
            return false;
         }
      }

      public async Task<List<CustomerModel>> GetSupplierCustomersAsync(Guid supplierId) {
         SystemUser user = await _userManager.FindByIdAsync(supplierId.ToString());

         if(user != null) {
            List<SupplierCustomers> suppCustomers = await _context.SupplierCustomers
                .Where(x => x.SupplierId == supplierId && !x.IsDeleted)
                .ToListAsync();

            if(suppCustomers != null && suppCustomers.Any()) {
               // Retrieve all users and convert to a list
               var allCustomers = await _userManager.GetUsersInRoleAsync(RoleConstants.CUSTOMER);

               // Perform an in-memory join between users and suppCustomers
               List<CustomerModel> customers = (from sc in suppCustomers
                                                join u in allCustomers
                                                on sc.CustomerId equals u.Id
                                                select new CustomerModel {
                                                   Id = u.Id,
                                                   Name = u.Name,
                                                   Email = u.Email,
                                                   Phone = u.PhoneNumber,
                                                   Address = u.Address,
                                                   UserName = u.UserName,
                                                }).ToList();

               return customers;
            } else {
               // Handle the case when no matching records are found
               return new List<CustomerModel>();
            }
         }
         return new List<CustomerModel>(); // Return an empty list or handle as needed
      }

      public async Task<List<DropdownItem>> GetCustomersDropdownAsync(Guid supplierId) {
         var customers = await _userManager.GetUsersInRoleAsync(RoleConstants.CUSTOMER);
         
         var customerIds = await _context.SupplierCustomers
            .Where(x => x.SupplierId == supplierId && !x.IsDeleted)
            .Select(x => x.CustomerId).ToListAsync();

         List<DropdownItem> dropdown = customers
            .Where(x => !customerIds.Contains(x.Id))
            .Select(x => new DropdownItem { Key = x.Id, Value = x.Name })
            .ToList();

         return dropdown;
      }

      public async Task<bool> AddCustomerToSupplierAsync(SupplierCustomerVM model) {
         try {
            SystemUser customer = await _userManager.FindByIdAsync(model.CustomerId.ToString());
            SystemUser supplier = await _userManager.FindByIdAsync(model.SupplierId.ToString());

            if(customer != null && supplier != null) {
               SupplierCustomers sc = new() {
                  CustomerId = customer.Id,
                  SupplierId = supplier.Id,
                  IsDeleted = false
               };

               await _context.SupplierCustomers.AddAsync(sc);
               await _context.SaveChangesAsync();
               return true;
            }
            return false;
         } catch(Exception ex) {
            _logger.LogError("An Error occurred: ", ex.ToString());
            return false;
         }
      }

      public async Task<bool> DeleteCustomerFromSupplierAsync(Guid customerId, Guid supplierId) {
         try {
            SystemUser customer = await _userManager.FindByIdAsync(customerId.ToString());
            SystemUser supplier = await _userManager.FindByIdAsync(supplierId.ToString());

            if(customer != null && supplier != null) {
               SupplierCustomers sc = await _context.SupplierCustomers
                  .FirstOrDefaultAsync(x => x.CustomerId == customer.Id && x.SupplierId == supplier.Id && !x.IsDeleted);

               _context.SupplierCustomers.Remove(sc);
               await _context.SaveChangesAsync();
               return true;
            }
            return false;
         } catch(Exception ex) {
            _logger.LogError("An Error occurred: ", ex.ToString());
            return false;
         }
      }

      public async Task<bool> IsEmailUniqueAsync(Guid id, string email) {
         return true;
      }

      public async Task<List<DropdownItem>> GetSuppliersCustomerDropdownAsync(Guid supplierId) {
         var customers = await _userManager.GetUsersInRoleAsync(RoleConstants.CUSTOMER);

         var customerIds = await _context.SupplierCustomers
            .Where(x => x.SupplierId == supplierId && !x.IsDeleted)
            .Select(x => x.CustomerId).ToListAsync();

         List<DropdownItem> dropdown = customers
            .Where(x => customerIds.Contains(x.Id))
            .Select(x => new DropdownItem { Key = x.Id, Value = x.Name })
            .ToList();

         return dropdown;
      }

      #region Mappers
      private static void MapEntityToVM(SystemUser source, SupplierModel destination) {
         destination.Id = source.Id;
         destination.Name = source.Name;
         destination.Address = source.Address;
         destination.Phone = source.PhoneNumber;
         destination.Email = source.Email;
         destination.UserName = source.UserName;
      }
      private void MapVMToEntity(SupplierModel source, SystemUser destination) {
         destination.Id = source.Id;
         destination.Name = source.Name;
         destination.Address = source.Address;
         destination.PhoneNumber = source.Phone;
         destination.Email = source.Email;
         destination.UserName = source.UserName;
         destination.ModifiedBy = _globalHelper.GetCurrentUserId();
         destination.ModifiedDate = DateTime.Now;
         if(source.Id == Guid.Empty) {
            destination.CreatedBy = _globalHelper.GetCurrentUserId();
            destination.CreatedDate = DateTime.Now;
         }
      }
      #endregion
   }
}
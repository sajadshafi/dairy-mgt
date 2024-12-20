using DairyManagementSystem.EmailConfig;
using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DairyManagementSystem.Services {
   public class CustomerService : ICustomerService {
      private readonly ApplicationDbContext _context;
      private readonly UserManager<SystemUser> _userManager;
      private readonly RoleManager<SystemRole> _roleManager;
      private readonly IEmailService _emailService;
      private readonly ILogger<CustomerService> _logger;
      private readonly GlobalHelper _globalHelper;

      public CustomerService(ApplicationDbContext context, ILogger<CustomerService> logger, GlobalHelper globalHelper, IEmailService emailService, UserManager<SystemUser> userManager, RoleManager<SystemRole> roleManager) {
         _context = context;
         _logger = logger;
         _globalHelper = globalHelper;
         _emailService = emailService;
         _userManager = userManager;
         _roleManager = roleManager;
      }

      public async Task<Guid?> DeleteAsync(Guid? id) {
         SystemUser user = await _userManager.FindByIdAsync(id.ToString());
         if(user == null) return null;

         Guid deletedId = user.Id;
         await _userManager.DeleteAsync(user);

         return deletedId;
      }

      public async Task<List<CustomerModel>> GetAsync(Guid? id = null) {
         List<CustomerModel> customers = new();
         if(id != null) {
            SystemUser user = await _userManager.FindByIdAsync(id.ToString());
            if(user.IsDeleted) return default;
            CustomerModel model = new();
            MapEntityToVM(user, model);
            customers.Add(model);
         } else {
            IList<SystemUser> customerList = await _userManager.GetUsersInRoleAsync(RoleConstants.CUSTOMER);
            customerList = customerList.Where(x => !x.IsDeleted).ToList();

            Guid currentUserId = _globalHelper.GetCurrentUserId();
            string currentUserRole = _globalHelper.GetCurrentUserRole();
            if(currentUserRole == RoleConstants.SUPPLIER) {
               List<Guid> scs = await _context.SupplierCustomers
               .Where(x => x.SupplierId == currentUserId)
               .Select(x => x.CustomerId)
               .ToListAsync();

               customerList = customerList.Where(x => scs.Contains(x.Id)).ToList();
            }
            foreach(SystemUser sup in customerList) {
               CustomerModel model = new();
               MapEntityToVM(sup, model);
               customers.Add(model);
            }
         }
         return customers;
      }

      public async Task<bool> SaveAsync(CustomerModel model) {
         try {
            if(model == null) {
               return false;
            }
            string password = PasswordGenerator.GeneratePassword(10);
            if(model.Id == Guid.Empty) {
               // Create a new customer
               SystemUser user = new();
               MapVMToEntity(model, user);
               IdentityResult result = await _userManager.CreateAsync(user, password);
               if(result.Succeeded) {
                  await _userManager.AddToRoleAsync(user, RoleConstants.CUSTOMER);

                  EmailModel emailModel = new() {
                     EmailTo = model.Email,
                     Subject = "Invitation to Dairy Management System",
                     Body = EmailTemplates.CreateInvitationTemplate(model.Email, password, model.Name, "http://localhost:5028/"),
                  };
                  await _emailService.SendEmailAsync(emailModel);
               } else return false;

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
            _logger.LogError("An Error has occured", ex.ToString());
            return false;
         }
      }

      public async Task<bool> IsEmailUniqueAsync(Guid id, string email) {
         return true;
      }

      #region Mappers
      public void MapEntityToVM(SystemUser source, CustomerModel destination) {
         destination.Id = source.Id;
         destination.Name = source.Name;
         destination.Address = source.Address;
         destination.Phone = source.PhoneNumber;
         destination.Email = source.Email;
         destination.UserName = source.UserName;
      }

      public void MapVMToEntity(CustomerModel source, SystemUser destination) {
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

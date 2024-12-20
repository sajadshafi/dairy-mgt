using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DairyManagementSystem.Services {
   public class AuthService : IAuthService {
      private readonly SignInManager<SystemUser> _signInManager;
      private readonly UserManager<SystemUser> _userInManager;
      private readonly RoleManager<SystemRole> _roleManager;
      private readonly ILogger<AuthService> _logger;
      private readonly GlobalHelper _helper;

      public AuthService(SignInManager<SystemUser> signInManager, UserManager<SystemUser> userInManager, GlobalHelper helper, RoleManager<SystemRole> roleManager, ILogger<AuthService> logger) {
         _signInManager = signInManager;
         _userInManager = userInManager;
         _helper = helper;
         _roleManager = roleManager;
         _logger = logger;
      }

      public async Task<bool> CreateRoleAsync(string roleName) {
         try {
            SystemRole role = new() {
                  Name = roleName,
                  CreatedDate = DateTime.Now,
                  ModifiedDate = DateTime.Now,
                  CreatedBy = _helper.GetCurrentUserId(),
                  ModifiedBy = _helper.GetCurrentUserId(),
               };

            IdentityResult result = await _roleManager.CreateAsync(role);
            return result.Succeeded;

         } catch(Exception ex) {
            _logger.LogError("An error occurred: ", ex.ToString());
            return false;
         }
      }

      public async Task<bool> LoginAsync(LoginDto model) {
         var user = await _userInManager.FindByEmailAsync(model.Email);

         if(user == null) {
            return false;
         }

         var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
         return result.Succeeded;
      }

      public async Task<bool> RegisterAsync(RegisterDto model) {
         var user = new SystemUser {
            UserName = model.UserName,
            Email = model.Email
         };

         var result = await _userInManager.CreateAsync(user, model.Password);

         return result.Succeeded;
      }

      public async Task<List<RoleDto>> GetAllRolesAsync() {
         List<SystemRole> roles = await _roleManager.Roles.ToListAsync();

         return roles.Select(x => new RoleDto { Id = x.Id, RoleName = x.Name }).ToList();
      }

   }
}

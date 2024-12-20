using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace DairyManagementSystem.Services {
   public class UserService : IUserService {

      private readonly GlobalHelper _global;
      private readonly UserManager<SystemUser> _userManager;
      private readonly ILogger<UserService> _logger;

      public UserService(GlobalHelper global, UserManager<SystemUser> userManager, ILogger<UserService> logger) {
         _global = global;
         _userManager = userManager;
         _logger = logger;
      }

      public async Task<UserProfileVM> GetUserProfileAsync() {
         Guid currentUserId = _global.GetCurrentUserId();
         SystemUser user = await _userManager.FindByIdAsync(currentUserId.ToString());
         if(user != null) {
            return new() {
               Id = user.Id,
               Name = user.Name,
               UserName = user.UserName,
               Email = user.Email,
               Address = user.Address,
               Phone = user.PhoneNumber,
               ImageArray = user.ProfileImage
            };
         }
         return null;
      }

      public async Task<bool> UpdateProfileAsync(UserProfileVM user) {
         try {
            Guid currentUserId = _global.GetCurrentUserId();
            SystemUser existingUser = await _userManager.FindByIdAsync(currentUserId.ToString());
            if(existingUser == null) return false;

            if(user.ProfileImage != null) {
               using MemoryStream memoryStream = new();
               user.ProfileImage.CopyTo(memoryStream);
               existingUser.ProfileImage = memoryStream.ToArray();
            }

            existingUser.Name = user.Name;
            existingUser.Address = user.Address;
            existingUser.PhoneNumber = user.Phone;

            IdentityResult result = await _userManager.UpdateAsync(existingUser);
            if(!result.Succeeded) return false;
            return true;
         } catch (Exception ex) {
            _logger.LogError("An error has occured: {0}", ex.ToString());
            return false;
         }
      }

      public async Task<bool> ChangePasswordAsync(PasswordChangeVM model) {
         try {
            Guid currentUserId = _global.GetCurrentUserId();
            SystemUser existingUser = await _userManager.FindByIdAsync(currentUserId.ToString());
            if(existingUser == null) return false;

            IdentityResult result = await _userManager
               .ChangePasswordAsync(existingUser, model.OldPassword, model.NewPassword);
            if(!result.Succeeded) return false;
            return true;
         } catch(Exception ex) {
            _logger.LogError("An error has occured: {0}", ex.ToString());
            return false;
         }
      }
   }
}

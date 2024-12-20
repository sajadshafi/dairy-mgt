using DairyManagementSystem.Models.ViewModels;

namespace DairyManagementSystem.IServices {
   public interface IUserService {
      Task<UserProfileVM> GetUserProfileAsync();
      Task<bool> UpdateProfileAsync(UserProfileVM user);
      Task<bool> ChangePasswordAsync(PasswordChangeVM model);
   }
}

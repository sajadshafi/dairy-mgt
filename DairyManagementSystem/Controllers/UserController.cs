using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DairyManagementSystem.Controllers {
   [Authorize]
   public class UserController : Controller {
      private readonly IUserService _userService;
      private readonly IToastNotification _toast;
      public UserController(IUserService userService, IToastNotification toast) {
         _userService = userService;
         _toast = toast;
      }

      public async Task<IActionResult> Profile() {
         UserProfileVM user = await _userService.GetUserProfileAsync();
         if(user == null)
            return RedirectToAction("logout", "auth");
         return View(user);
      }

      [HttpPost]
      public async Task<IActionResult> UpdateProfile(UserProfileVM user) {
         bool result = await _userService.UpdateProfileAsync(user);
         if(result) _toast.AddSuccessToastMessage("Profile details updated successfully");
         else _toast.AddErrorToastMessage("Failed to update profile");
         return RedirectToAction(nameof(Profile));
      }

      [HttpPost]
      public async Task<IActionResult> ChangePassword(PasswordChangeVM model) {
         bool result = await _userService.ChangePasswordAsync(model);
         if(result) _toast.AddSuccessToastMessage("Password changed successfully");
         else _toast.AddErrorToastMessage("Failed to change password");
         return RedirectToAction(nameof(Profile));
      }
   }
}

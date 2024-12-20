using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;

namespace DairyManagementSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<SystemUser> _signInManager;
        private readonly IAuthService _authService;
        private readonly IToastNotification _toast;
        public AuthController(SignInManager<SystemUser> signInManager, IAuthService authService, IToastNotification toast
            )
        {
            _signInManager = signInManager;
            _authService = authService;
            _toast = toast;
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await _signInManager.SignOutAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool loginResult = await _authService.LoginAsync(model);
            if (loginResult)
            {
                // Login successful
                _toast.AddSuccessToastMessage("Logged In Successfully!");
                return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            ViewBag.ErrorMessage = "* Email or password is wrong!";
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            if (ModelState.IsValid)
            {
                bool registrationResult = await _authService.RegisterAsync(model);
                if (registrationResult)
                {
                    return View();
                }
                else
                {
                    _toast.AddSuccessToastMessage("Registration Failed!");
                    ModelState.AddModelError("", "Registration failed.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _toast.AddSuccessToastMessage("Logged out Successfully!");
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user != null)
        //        {
        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //            var resetLink = Url.Action("ResetPassword", "Auth", new { userId = user.Id, token = token }, Request.Scheme);

        //            // Send the reset password link to the user's email
        //            // Implement email sending logic here

        //            return RedirectToAction("ForgotPasswordConfirmation");
        //        }
        //        // Handle invalid email (user not found) scenario here.
        //    }
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            var model = new ResetPasswordViewModel { UserId = userId, Token = token };
            return View(model);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByIdAsync(model.UserId);
        //        if (user != null)
        //        {
        //            var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        //            if (resetResult.Succeeded)
        //            {
        //                // Password reset successful; redirect to a success page
        //                return RedirectToAction("ResetPasswordConfirmation");
        //            }
        //            // Handle password reset failure (e.g., invalid token) here.
        //        }
        //        // Handle user not found here.
        //    }
        //    return View(model);
        //}

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}

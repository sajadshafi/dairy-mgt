using DairyManagementSystem.IServices;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DairyManagementSystem.Controllers {
   [Authorize]
   public class DashboardController : Controller {
      private readonly IDashboardService _service;
      private readonly IUnitsService _unitsService;
      private readonly IToastNotification _toast;
      private readonly IAuthService _authService;

      public DashboardController(IDashboardService service, IUnitsService unitsService, IToastNotification toast, IAuthService authService) {
         _service = service;
         _unitsService = unitsService;
         _toast = toast;
         _authService = authService;
      }

      public async Task<IActionResult> Index() {

         DashboardVM dashboardData = new() {
            Counts = await _service.GetCountsAsync(),
         };

         var sales = await _service.GetSalesOverviewAsync();

         ViewBag.Sales = sales;
         return View(dashboardData);
      }

      [HttpGet]
      public async Task<IActionResult> GetGraphDetails() {
         GraphVM graphs = await _service.GetGraphsAsync();

         return Ok(graphs);
      }

      public async Task<IActionResult> Setting() {
         SettingVM viewmodel = new() {
            MeasurementUnits = await _unitsService.GetAsync(),
            Roles = await _authService.GetAllRolesAsync(),
         };
         return View(viewmodel);
      }

      [HttpPost]
      public async Task<IActionResult> SaveUnit(MeasurementUnitVM model) {
         bool result = await _unitsService.SaveAsync(model);
         if(result) {
            _toast.AddSuccessToastMessage("Units Saved Successfully!");
            return RedirectToAction(nameof(Setting));
         } else {
            _toast.AddErrorToastMessage("Failed to Save Unit!");
            return RedirectToAction(nameof(Setting));
         }
      }

      [HttpGet]
      public async Task<IActionResult> DeleteUnit(Guid? id) {

         bool result = await _unitsService.DeleteAsync(id);
         if(!result) {
            _toast.AddErrorToastMessage("Measurement Unit Not Found!");
            return RedirectToAction("Setting");
         }
         _toast.AddWarningToastMessage("Measurement Unit Deleted Successfully!");
         return RedirectToAction("Setting");
      }

      [HttpPost]
      public async Task<IActionResult> SaveRole([FromForm]string roleName) {
         bool result = await _authService.CreateRoleAsync(roleName);
         if(result) {
            _toast.AddSuccessToastMessage("Role Saved Successfully!");
            return RedirectToAction(nameof(Setting));
         } else {
            _toast.AddErrorToastMessage("Failed to Save Role!");
            return RedirectToAction(nameof(Setting));
         }
      }

   }
}

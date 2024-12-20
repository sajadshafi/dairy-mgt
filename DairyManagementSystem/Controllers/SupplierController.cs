using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DairyManagementSystem.Controllers {
   [Authorize(Roles = RoleConstants.ADMIN)]
   public class SupplierController : Controller {
      private readonly ISupplierService _service;
      private readonly IToastNotification _toast;
      public SupplierController(ISupplierService service, IToastNotification toast) {
         _service = service;
         _toast = toast;
      }
      public async Task<IActionResult> Index()
          => View(await _service.GetAsync());

      public async Task<IActionResult> Save(Guid? id = null) {
         if(id != null) {
            List<SupplierModel> suppliers = await _service.GetAsync(id);
            if(suppliers == null || suppliers.Count <= 0) {
               return View();
            }

            return View(suppliers.FirstOrDefault());
         }
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Save(SupplierModel supplier) {
         if(ModelState.IsValid) {
            bool result = await _service.SaveAsync(supplier);
            if(result) _toast.AddSuccessToastMessage("Supplier Saved Successfully");
            else _toast.AddErrorToastMessage("Failed To Save Supplier");
            return RedirectToAction("Index");
         }
         return View(supplier);
      }

      public async Task<IActionResult> Details(Guid id) {
         if(id == Guid.Empty) {
            return NotFound();
         }

         var supplier = await _service.GetAsync(id);
         if(supplier == null || supplier.Count <= 0) {
            return NotFound();
         }

         ViewBag.CustomerDD = await _service.GetCustomersDropdownAsync(id);

         var sup = supplier.FirstOrDefault();
         sup.SupplierCustomers = await _service.GetSupplierCustomersAsync(sup.Id);
         return View(sup);
      }

      [HttpGet]
      public async Task<IActionResult> Delete(Guid id) {
         Guid? result = await _service.DeleteAsync(id);
         if(result == null) {
            _toast.AddErrorToastMessage("Supplier Not Found!");
            return RedirectToAction("Index");
         }
         _toast.AddWarningToastMessage("Supplier deleted successfully!");
         return RedirectToAction("Index");
      }

      [HttpPost]
      public async Task<IActionResult> AddCustomerToSupplier(SupplierCustomerVM model) {
         bool result = await _service.AddCustomerToSupplierAsync(model);
         if(result) {
            _toast.AddSuccessToastMessage("Customer added to supplier!");
            return RedirectToAction(nameof(Details), new { id = model.SupplierId });
         }
         _toast.AddWarningToastMessage("Failed to add customer to supplier!");
         return RedirectToAction(nameof(Details), new { id = model.SupplierId });
      }

      [HttpGet]
      public async Task<IActionResult> DeleteCustomerFromSupplier(Guid customerId, Guid supplierId) {
         bool result = await _service.DeleteCustomerFromSupplierAsync(customerId, supplierId);
         if(!result) {
            _toast.AddErrorToastMessage("Failed to remove customer!");
            return RedirectToAction(nameof(Details), new { id = supplierId });
         }
         _toast.AddSuccessToastMessage("Customer removed successfully!");
         return RedirectToAction(nameof(Details), new { id = supplierId });
      }

   }
}
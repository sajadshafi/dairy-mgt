using Microsoft.AspNetCore.Mvc;
using DairyManagementSystem.Models.ViewModels;
using DairyManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;
using DairyManagementSystem.Helpers;

namespace DairyManagementSystem.Controllers {

   [Authorize(Roles = $"{RoleConstants.ADMIN}, {RoleConstants.SUPPLIER}")]
   public class CustomerController : Controller {
      private readonly ICustomerService _service;
      private readonly IToastNotification _toast;
      public CustomerController(ICustomerService customerService, IToastNotification toast) {
         _service = customerService;
         _toast = toast;
      }

      public async Task<IActionResult> Index() {
         var customers = await _service.GetAsync();
         return View(customers);
      }

      public async Task<IActionResult> Save(Guid? id = null) {
         if(id != null) {
            List<CustomerModel> customers = await _service.GetAsync(id);
            if(customers == null || customers.Count <= 0) {
               return View();
            }

            return View(customers.FirstOrDefault());
         }
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Save(CustomerModel customer) {
         if(ModelState.IsValid) {
            bool isEmailUnique = await _service.IsEmailUniqueAsync(customer.Id, customer.Email);
            if(!isEmailUnique) {
               ModelState.AddModelError(nameof(customer.Email), "Email is already in use.");
               _toast.AddErrorToastMessage("Email is already registered. Try Login!");
               return View(customer);
            }
            bool result = await _service.SaveAsync(customer);
            if(result) _toast.AddSuccessToastMessage("Customer Saved Successfully");
            else _toast.AddErrorToastMessage("Failed To Save Customer");
            return RedirectToAction("Index");
         }
         return View(customer);
      }

      public async Task<IActionResult> Details(Guid? id) {
         if(id == null) {
            _toast.AddErrorToastMessage("Invalid Id!");
            return RedirectToAction("Index");
         }

         var customer = await _service.GetAsync(id.Value);
         if(customer == null || customer.Count <= 0) {
            _toast.AddErrorToastMessage("Customer Not Found!");
            return RedirectToAction("Index");
         }

         return View(customer.FirstOrDefault());
      }

      [HttpGet]
      public async Task<IActionResult> Delete(Guid? id) {

         Guid? result = await _service.DeleteAsync(id);
         if(result == null) {
            _toast.AddErrorToastMessage("Customer Not Found!");
            return RedirectToAction("Index");
         }
         _toast.AddWarningToastMessage("Customer Deleted Successfully!");
         return RedirectToAction("Index");
      }
   }
}

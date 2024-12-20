using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace DairyManagementSystem.Controllers {
   [Authorize]
   public class SalesController : Controller {
      private readonly ISalesService _salesService;
      private readonly GlobalHelper _helpers;
      private readonly IToastNotification _toast;
      private readonly IProductService _productService;
      private readonly ISupplierService _supplierService; // Assuming you have a service for suppliers

      public SalesController(ISalesService salesService, ISupplierService supplierService, GlobalHelper helpers, IToastNotification toast, IProductService productService) {
         _salesService = salesService;
         _supplierService = supplierService;
         _helpers = helpers;
         _toast = toast;
         _productService = productService;
      }

      public async Task<IActionResult> Index() {
         var sales = await _salesService.GetAsync();
         return View(sales);
      }
      public async Task<IActionResult> Details(Guid? id) {
         if(id == null) {
            _toast.AddErrorToastMessage("Invalid Id!");
            return RedirectToAction("Index");
         }
         var sale = await _salesService.GetAsync(id.Value);
         if(sale == null || sale.Count <= 0) {
            _toast.AddErrorToastMessage("Sale Not Found!");
            return RedirectToAction("Index");
         }

         return View(sale.FirstOrDefault());
      }

      public async Task<IActionResult> Save(Guid? id = null) {
         ViewBag.Buyers = await _supplierService.GetSuppliersCustomerDropdownAsync(_helpers.GetCurrentUserId());
         ViewBag.Products = await _productService.GetSupplierProductDropdownAsync();
         if(id != null) {
            SalesVM sale = await _salesService.GetSaleAsync(id);
            if(sale == null) {
               return View();
            }
            return View(sale);
         }
         return View();
      }

      [HttpPost]
      public async Task<IActionResult> Save(SalesVM sale) {
         bool result = await _salesService.SaveAsync(sale);
         if(!result) {
            _toast.AddErrorToastMessage("Failed to make a sale!");
            return View(sale);
         }
         _toast.AddSuccessToastMessage("Sold successfully!");
         return RedirectToAction("Index");
         
      }

      public async Task<IActionResult> Delete(Guid? id) {
         bool result = await _salesService.DeleteAsync(id);
         if(!result) {
            _toast.AddErrorToastMessage("Sale Not Found!");
            return RedirectToAction("Index");
         }
         _toast.AddWarningToastMessage("Sale Deleted Successfully!");
         return RedirectToAction("Index");
      }
   }
}

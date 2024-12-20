using DairyManagementSystem.Models.ViewModels;
using DairyManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using DairyManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using NToastNotify;

namespace DairyManagementSystem.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly IToastNotification _toast;
        private readonly IUnitsService _unitsService;

        public ProductController(IProductService service, IToastNotification toast, IUnitsService unitsService)
        {
            _service = service;
            _toast = toast;
            _unitsService = unitsService;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _service.GetAsync();
            return View(products);
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                _toast.AddErrorToastMessage("Invalid Id!");
                return RedirectToAction("Index");
            }
            var product = await _service.GetAsync(id.Value);
            if (product == null || product.Count <= 0)
            {
                _toast.AddErrorToastMessage("Product Not Found!");
                return RedirectToAction("Index");
            }

            return View(product.FirstOrDefault());
        }
        public async Task<IActionResult> Save(Guid? id = null)
        {
            ViewBag.Units = await _unitsService.GetMeasurementUnitsDropDownAsync();
            if (id != null)
            {
                List<ProductModel> products = await _service.GetAsync(id);
                if (products == null || products.Count <= 0)
                {
                    return View();
                }
                return View(products.FirstOrDefault());
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductModel product)
        {
            if (ModelState.IsValid)
            {
                bool result = await _service.SaveAsync(product);
                if (result) _toast.AddSuccessToastMessage("Product Saved Successfully");
                else _toast.AddErrorToastMessage("Failed To Save Product");
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            bool result = await _service.DeleteAsync(id);
            if (!result)
            {
                _toast.AddErrorToastMessage("Product Not Found!");
                return RedirectToAction("Index");
            }
            _toast.AddWarningToastMessage("Product Deleted Successfully!");
            return RedirectToAction("Index");
        }
    }
}

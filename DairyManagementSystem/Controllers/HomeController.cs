using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DairyManagementSystem.Controllers {

   [Authorize]
   public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config) {
            _logger = logger;
            _config = config;
        }
        public IActionResult Index() {
           
            //var customers = context.Customers.Include(x => x.).ToList();
            //if(customers.Count > 0) {
            //    List<SupplierModel> supplierModels = new();
            //    foreach(var student in customers) {
            //        CustomerModel stdMd = new CustomerModel();
            //        CustomerModel.MapStudent(student, stdMd);
            //        studentsmodel.Add(stdMd);
            //    }
            //    return View(studentsmodel);
            //}
            return View();
        }
        public IActionResult Privacy() {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
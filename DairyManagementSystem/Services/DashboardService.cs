using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DairyManagementSystem.Services {
   public class DashboardService : IDashboardService {

      private readonly ApplicationDbContext _context;
      private readonly UserManager<SystemUser> _userManager;
      private readonly GlobalHelper _globalHelper;

      public DashboardService(ApplicationDbContext context, UserManager<SystemUser> userManager, GlobalHelper globalHelper) {
         _context = context;
         _userManager = userManager;
         _globalHelper = globalHelper;
      }
      public async Task<List<CountVM>> GetCountsAsync() {
         string currentRole = _globalHelper.GetCurrentUserRole();
         List<SystemUser> suppliers = currentRole.Equals(RoleConstants.ADMIN, StringComparison.OrdinalIgnoreCase) ?
            (List<SystemUser>)await _userManager.GetUsersInRoleAsync(RoleConstants.SUPPLIER) : new();

         Guid currentId = _globalHelper.GetCurrentUserId();
         List<SystemUser> customers = currentRole.Equals(RoleConstants.ADMIN, StringComparison.OrdinalIgnoreCase) ?
            (List<SystemUser>)await _userManager.GetUsersInRoleAsync(RoleConstants.CUSTOMER) : new();

         int salesCount = 0;
         if(currentRole.Equals(RoleConstants.ADMIN, StringComparison.OrdinalIgnoreCase))
            salesCount = await _context.Sales
               .Where(sale => !sale.IsDeleted).CountAsync();

         if(currentRole.Equals(RoleConstants.SUPPLIER, StringComparison.OrdinalIgnoreCase))
            salesCount = await _context.Sales
               .Where(sale => !sale.IsDeleted && sale.SellerId == currentId).CountAsync();

         if(currentRole.Equals(RoleConstants.CUSTOMER, StringComparison.OrdinalIgnoreCase))
            salesCount = await _context.Sales
               .Where(sale => !sale.IsDeleted && sale.BuyerId == currentId).CountAsync();

         List<CountVM> counts = new()
          {
               new() { CountOf = Constants.SUPPLIERS, Icon = Icons.USERS, Count = suppliers.Where(x => !x.IsDeleted).Count(), Role = RoleConstants.ADMIN },
               new() {
                  CountOf = Constants.CUSTOMERS, Icon = Icons.USERS, NotInRole = RoleConstants.CUSTOMER,
                  Count = currentRole.Equals(RoleConstants.ADMIN, StringComparison.OrdinalIgnoreCase) ?
                  customers.Count : await _context.SupplierCustomers.Where(x => x.SupplierId == currentId).CountAsync() },
               new() { CountOf = Constants.PRODUCTS, Icon = Icons.BOXES, Count = await _context.Products.Where(product => !product.IsDeleted).CountAsync() },
               new() { CountOf = Constants.SALES, Icon = Icons.TRUCK_LOADING, Count = salesCount }
            };

         


         return counts;
      }

      public async Task<GraphVM> GetGraphsAsync() {
         try {
            #region Count of sales
            DateTime today = DateTime.Today;
            int daysTillToday = (int)today.DayOfWeek;
            DateTime startOfWeek = today.AddDays(-daysTillToday);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            List<CountGraphVM> currentWeekSales = await _context.Sales
                                     .Where(s => s.Date >= startOfWeek && s.Date <= endOfWeek)
                                     .GroupBy(s => s.Date.Date)
                                     .Select(g => new CountGraphVM {
                                        Day = g.Key.DayOfWeek.ToString().Substring(0, 3).ToUpper(),
                                        Count = g.Count()
                                     })
                                     .ToListAsync();

            DateTime startOfPreviousWeek = today.AddDays(-daysTillToday - 7);
            DateTime endOfPreviousWeek = startOfPreviousWeek.AddDays(6);

            List<CountGraphVM> previousWeekSales = await _context.Sales
                                         .Where(s => s.Date >= startOfPreviousWeek && s.Date <= endOfPreviousWeek)
                                         .GroupBy(s => s.Date.Date)
                                         .Select(g => new CountGraphVM {
                                            Day = g.Key.DayOfWeek.ToString().Substring(0, 3).ToUpper(),
                                            Count = g.Count()
                                         })
                                         .ToListAsync();

            #endregion

            #region Amount of sales

            var currentWeekStart = today.AddDays(-(int)today.DayOfWeek);
            var currentWeekEnd = currentWeekStart.AddDays(6);
            List<AmountGraphVM> currentWeekSalesAmount = _context.Sales
                   .Where(s => s.Date >= currentWeekStart && s.Date <= currentWeekEnd)
                  .AsEnumerable() // Fetch data into memory
                  .GroupBy(s => s.Date.DayOfWeek)
                   .Select(g => new AmountGraphVM {
                      Day = g.Key.ToString().Substring(0,3).ToUpper(),
                      Amount = g.Sum(s => s.TotalAmount)
                   })
                   .ToList();

            var lastWeekStart = today.AddDays(-(int)today.DayOfWeek - 7);
            var lastWeekEnd = lastWeekStart.AddDays(6);
            List<AmountGraphVM> lastWeekSalesAmount = _context.Sales
                .Where(s => s.Date >= lastWeekStart && s.Date <= lastWeekEnd)
                  .AsEnumerable() // Fetch data into memory
                  .GroupBy(s => s.Date.DayOfWeek)
                .Select(g => new AmountGraphVM {
                   Day = g.Key.ToString().Substring(0, 3).ToUpper(),
                   Amount = g.Sum(s => s.TotalAmount)
                })
                .ToList();
            #endregion

            GraphVM graph = new() {
               CurrentWeekSalesCount = currentWeekSales,
               LastWeekSalesCount = previousWeekSales,
               LastWeekSalesAmount = lastWeekSalesAmount,
               CurrentWeekSalesAmount = currentWeekSalesAmount
            };

            return graph;
         } catch(Exception ex) {
            Console.WriteLine("Error: ", ex.StackTrace);
            return new();
         }
      }

      public async Task<DashboardSalesVM> GetSalesOverviewAsync() {
         DashboardSalesVM sales = new() {
            TotalSalesCount = await _context.Sales.CountAsync(),
            TotalSalesAmount = await _context.Sales.SumAsync(x => x.TotalAmount)
         };

         return sales;
      }
   }
}

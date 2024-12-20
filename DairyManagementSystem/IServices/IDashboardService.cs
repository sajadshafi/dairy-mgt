using DairyManagementSystem.Models.ViewModels;

namespace DairyManagementSystem.IServices {
   public interface IDashboardService {
      Task<List<CountVM>> GetCountsAsync();
      Task<DashboardSalesVM> GetSalesOverviewAsync();
      Task<GraphVM> GetGraphsAsync();
   }
}

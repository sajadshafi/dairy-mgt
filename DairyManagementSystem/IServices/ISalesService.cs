using DairyManagementSystem.Models.ViewModels;

namespace DairyManagementSystem.IServices {
   public interface ISalesService {
      Task<List<SaleResVM>> GetAsync(Guid? id = null);
      Task<SalesVM> GetSaleAsync(Guid? id = null);
      Task<bool> SaveAsync(SalesVM model);
      Task<bool> DeleteAsync(Guid? id);

   }
}

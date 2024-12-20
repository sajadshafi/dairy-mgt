using DairyManagementSystem.Models.ViewModels;

namespace DairyManagementSystem.IServices {
   public interface IProductService {
      Task<List<ProductModel>> GetAsync(Guid? id = null);
      Task<bool> SaveAsync(ProductModel model);
      Task<bool> DeleteAsync(Guid? id);
      Task<List<DropdownItem>> GetSupplierProductDropdownAsync(Guid? supplierId = null);
   }
}

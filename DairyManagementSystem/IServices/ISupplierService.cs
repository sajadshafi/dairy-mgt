using DairyManagementSystem.Models.ViewModels;

namespace DairyManagementSystem.IServices {
   public interface ISupplierService {
      Task<List<SupplierModel>> GetAsync(Guid? id = null);
      Task<bool> SaveAsync(SupplierModel model);
      Task<Guid?> DeleteAsync(Guid id);
      Task<bool> IsEmailUniqueAsync(Guid id, string email);
      Task<List<CustomerModel>> GetSupplierCustomersAsync(Guid supplierId);
      Task<bool> AddCustomerToSupplierAsync(SupplierCustomerVM model);
      Task<List<DropdownItem>> GetCustomersDropdownAsync(Guid supplierId);
      Task<List<DropdownItem>> GetSuppliersCustomerDropdownAsync(Guid supplierId);
      Task<bool> DeleteCustomerFromSupplierAsync(Guid customerId, Guid supplierId);
    }
}
using DairyManagementSystem.Models.ViewModels;

namespace DairyManagementSystem.IServices
{
    public interface ICustomerService
    {
        Task<List<CustomerModel>> GetAsync(Guid? id = null);
        Task<bool> SaveAsync(CustomerModel model);
        Task<Guid?> DeleteAsync(Guid? id);
        Task<bool> IsEmailUniqueAsync(Guid id, string email);
    }
}

using DairyManagementSystem.Models.ViewModels;

namespace DairyManagementSystem.IServices {
   public interface IUnitsService {
      Task<List<DropdownVM>> GetMeasurementUnitsDropDownAsync();
      Task<List<MeasurementUnitVM>> GetAsync(Guid? id = null);
      Task<bool> SaveAsync(MeasurementUnitVM model);
      Task<bool> DeleteAsync(Guid? id);
   }
}

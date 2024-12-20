using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DairyManagementSystem.Services {
   public class UnitsService : IUnitsService {
      private readonly ApplicationDbContext _context;
      private readonly ILogger<UnitsService> _logger;

      public UnitsService(ApplicationDbContext context, ILogger<UnitsService> logger) {
         _context = context;
         _logger = logger;
      }

      public async Task<bool> DeleteAsync(Guid? id) {
         try {
            var unit = await _context.MeasurementUnits.FirstOrDefaultAsync(x => x.Id == id);
            if(unit == null) return false;

            Guid? deletedId = unit.Id;

            _context.MeasurementUnits.Remove(unit);
            await _context.SaveChangesAsync();

            return true;
         } catch(Exception ex) {
            _logger.LogError("An Error occurred: ", ex.ToString());
            return false;
         }
      }

      public async Task<List<MeasurementUnitVM>> GetAsync(Guid? id = null) {
         if(id != null) {

            return await _context.MeasurementUnits.Where(x => x.Id == id).Select(x => new MeasurementUnitVM {
               Id = x.Id,
               Name = x.Name,
               Description = x.Description,
               Symbol = x.Symbol,
            }).ToListAsync();

         } else {

            return await _context.MeasurementUnits.Select(x => new MeasurementUnitVM {
               Id = x.Id,
               Name = x.Name,
               Description = x.Description,
               Symbol = x.Symbol,
            }).ToListAsync();

         }
      }

      public async Task<List<DropdownVM>> GetMeasurementUnitsDropDownAsync() {
         return await _context.MeasurementUnits
            .Select(x => new DropdownVM 
            { Key = x.Id, Value = x.Name + " (" + x.Symbol +")" })
            .ToListAsync();
      }

      public async Task<bool> SaveAsync(MeasurementUnitVM model) {
         try {
            if(model == null) {
               return false;
            }
            if(model.Id == Guid.Empty) {
               MeasurementUnit newUnit = new() {
                  Name = model.Name,
                  Description = model.Description,
                  Symbol = model.Symbol?.ToUpper(),
               };
               _context.MeasurementUnits.Add(newUnit);
            } else {
               MeasurementUnit existingUnit = await _context.MeasurementUnits.FirstOrDefaultAsync(x => x.Id == model.Id);
               if(existingUnit != null) {
                  existingUnit.Name = model.Name;
                  existingUnit.Description = model.Description;
                  existingUnit.Symbol = model.Symbol?.ToUpper();
               }
            }
            await _context.SaveChangesAsync();
            return true;

         } catch(Exception ex) {
            _logger.LogError("An Error has occured", ex.ToString());
            return false;
         }
      }
   }
}

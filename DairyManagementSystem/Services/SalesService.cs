using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DairyManagementSystem.Services {
   public class SalesService : ISalesService {

      private readonly ApplicationDbContext _context;
      private readonly GlobalHelper _helper;
      private readonly ILogger<SalesService> _logger;

      public SalesService(ApplicationDbContext context, GlobalHelper helper, ILogger<SalesService> logger) {
         _context = context;
         _helper = helper;
         _logger = logger;
      }

      public async Task<bool> DeleteAsync(Guid? id) {
         try {
            var sale = await _context.Sales.FirstOrDefaultAsync(x => x.Id == id);
            if(sale == null) return false;

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return true;
         } catch(Exception ex) {
            _logger.LogError("An Error occurred: ", ex.ToString());
            return false;
         }
      }

      public async Task<List<SaleResVM>> GetAsync(Guid? id = null) {
         Guid currentUserId = _helper.GetCurrentUserId();
         List<SaleResVM> Sales = await _context.Sales
            .Where(x => id == null || x.Id == id)
            .Where(x => x.SellerId == currentUserId)
            .Include(x => x.Buyer)
            .Include(x => x.Product)
            .ThenInclude(x => x.MeasurementUnit)
            .AsNoTracking()
            .AsSplitQuery()
            .Select(x => new SaleResVM {
               SaleId = x.Id,
               BuyerName = x.Buyer.Name,
               ProductName = x.Product.ProductName,
               Quantity = x.Quantity,
               UnitSymbol = x.Product.MeasurementUnit.Symbol,
               PricePerUnit = x.CurrentPrice,
               TotalAmount = x.TotalAmount,
               Date = x.Date == default ? DateTime.UtcNow : x.Date,
            })
            .OrderByDescending(x => x.Date)
            .ToListAsync() ?? new();

         return Sales;
      }

      public async Task<SalesVM> GetSaleAsync(Guid? id = null) {
         if(id != null) {
            SalesVM sale = await _context.Sales
               .Where(x => x.Id == (Guid)id)
               .Select(x => new SalesVM() {
                  Id = x.Id,
                  BuyerId = x.BuyerId,
                  ProductId = x.ProductId,
                  Date = x.Date == default ? DateTime.UtcNow : x.Date,
               })
               .FirstOrDefaultAsync();

            return sale;
         }
         return null;
      }

      public async Task<bool> SaveAsync(SalesVM model) {
         try {
            if(model == null) {
               return false;
            }
            if(model.Id == Guid.Empty) {
               Sales sale = new();
               MapVMToEntity(model, sale);
               decimal currentPrice =  await _context.Products
                  .Where(x => x.Id == sale.ProductId)
                  .Select(x => x.Price)
                  .FirstOrDefaultAsync();

               sale.CurrentPrice = currentPrice;
               sale.TotalAmount = sale.Quantity * currentPrice;
               _context.Sales.Add(sale);
            } else {
               Sales existingSale = await _context.Sales.FirstOrDefaultAsync(x => x.Id == model.Id);
               if(existingSale != null) {
                  MapVMToEntity(model, existingSale);
                  _context.Entry(existingSale).State = EntityState.Modified;
               }
            }
            await _context.SaveChangesAsync();
            return true;

         } catch(Exception ex) {
            _logger.LogError("An Error has occured", ex.ToString());
            return false;
         }
      }

      private void MapVMToEntity(SalesVM source, Sales destination) {
         destination.SellerId = _helper.GetCurrentUserId();
         destination.BuyerId = source.BuyerId;
         destination.Quantity = source.Quantity;
         destination.ProductId = source.ProductId;
         destination.Date = source.Date == default ? DateTime.UtcNow : source.Date;
         destination.CreatedDate = DateTime.Now;
         destination.ModifiedDate = DateTime.Now;
         if(source.Id == Guid.Empty) {
            destination.CreatedBy = _helper.GetCurrentUserId();
         }
         destination.ModifiedBy = _helper.GetCurrentUserId();
      }
   }
}

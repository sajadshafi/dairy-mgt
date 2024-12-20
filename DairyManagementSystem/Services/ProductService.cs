using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Models;
using DairyManagementSystem.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DairyManagementSystem.Services {
   public class ProductService : IProductService {

      private readonly ApplicationDbContext _context;
      private readonly GlobalHelper _helper;
      private readonly ILogger<ProductService> _logger;

      public ProductService(ApplicationDbContext context, ILogger<ProductService> logger, GlobalHelper helper) {
         _context = context;
         _logger = logger;
         _helper = helper;
      }

      public async Task<bool> DeleteAsync(Guid? id) {
         try {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if(product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
         } catch(Exception ex) {
            _logger.LogError("An Error occurred: ", ex.ToString());
            return false;
         }
      }

      public async Task<List<ProductModel>> GetAsync(Guid? id = null) {
         List<ProductModel> products = new();
         if(id != null) {
            Product product = await _context.Products.Include(x => x.MeasurementUnit).FirstOrDefaultAsync(x => x.Id == id);
            ProductModel model = new();
            MapEntityToVM(product, model);
            model.UnitName = product.MeasurementUnit.Name + " (" + product.MeasurementUnit.Symbol + ")";
            products.Add(model);
         } else {
            List<Product> productList = await _context.Products.Include(x => x.MeasurementUnit).ToListAsync();
            foreach(Product prod in productList) {
               ProductModel model = new();
               MapEntityToVM(prod, model);
               model.UnitName = prod.MeasurementUnit.Name + " (" + prod.MeasurementUnit.Symbol + ")";
               products.Add(model);
            }
         }
         return products;
      }

      public async Task<bool> SaveAsync(ProductModel model) {
         try {
            if(model == null) {
               return false;
            }
            if(model.Id == Guid.Empty) {
               Product product = new();
               MapVMToEntity(model, product);
               _context.Products.Add(product);
            } else {
               Product existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == model.Id);
               if(existingProduct != null) {
                  MapVMToEntity(model, existingProduct);
                  _context.Entry(existingProduct).State = EntityState.Modified;
               }
            }
            await _context.SaveChangesAsync();
            return true;

         } catch(Exception ex) {
            _logger.LogError("An Error has occured", ex.ToString());
            return false;
         }
      }

      public async Task<List<DropdownItem>> GetSupplierProductDropdownAsync(Guid? supplierId = null) {
         if(supplierId == null) supplierId = _helper.GetCurrentUserId();
         List<DropdownItem> products = await _context.Products
            .Select(x => new DropdownItem { Key = x.Id, Value = x.ProductName })
            .ToListAsync();

         return products;
      }

      private void MapVMToEntity(ProductModel source, Product destination) {
         destination.Id = source.Id;
         destination.ProductName = source.ProductName;
         destination.Price = source.Price;
         destination.Quantity = source.Quantity;
         destination.MeasurementUnitId = source.UnitId;
         destination.CreatedDate = DateTime.Now;
         destination.ModifiedDate = DateTime.Now;
         if(source.Id == Guid.Empty) {
            destination.CreatedBy = _helper.GetCurrentUserId();
         }
         destination.ModifiedBy = _helper.GetCurrentUserId();
      }

      private static void MapEntityToVM(Product source, ProductModel destination) {
         destination.Id = source.Id;
         destination.ProductName = source.ProductName;
         destination.Price = source.Price;
         destination.UnitId = source.MeasurementUnitId;
         destination.Quantity = source.Quantity;
      }

   }
}

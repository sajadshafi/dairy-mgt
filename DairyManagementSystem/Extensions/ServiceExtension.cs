using DairyManagementSystem.EmailConfig;
using DairyManagementSystem.Helpers;
using DairyManagementSystem.IServices;
using DairyManagementSystem.Services;

namespace DairyManagementSystem.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services) {
            services.AddScoped<IAuthService, AuthService>();  // Register the correct implementation
            services.AddScoped<ISupplierService, SupplierSerivce>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IUnitsService, UnitsService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<GlobalHelper>();
            return services;
        }
    }
}

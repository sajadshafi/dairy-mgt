using DairyManagementSystem.Models.ViewModels;

namespace DairyManagementSystem.IServices {
   public interface IAuthService {
      Task<bool> LoginAsync(LoginDto model);
      Task<bool> RegisterAsync(RegisterDto model);
      Task<bool> CreateRoleAsync(string roleName);
      Task<List<RoleDto>> GetAllRolesAsync();
   }
}

using System.Security.Claims;

namespace DairyManagementSystem.Helpers {
   public class GlobalHelper {

      private readonly IHttpContextAccessor _httpContextAccessor;
      public GlobalHelper(IHttpContextAccessor httpContextAccessor) {
         _httpContextAccessor = httpContextAccessor;
      }
      public Guid GetCurrentUserId() {
         var value = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
         if(!string.IsNullOrEmpty(value))
            return new Guid(value);
         return Guid.Empty;
      }

      public string GetCurrentUserRole() {
         var value = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
         if(!string.IsNullOrEmpty(value))
            return value;
         return string.Empty;
      }
   }
}

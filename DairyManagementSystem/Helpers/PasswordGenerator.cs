using System.Text;

namespace DairyManagementSystem.Helpers {
   public class PasswordGenerator {
      private static readonly string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()+=[]{}|<>?";
      private static readonly Random Random = new();

      public static string GeneratePassword(int length) {
         StringBuilder password = new(length);

         for(int i = 0; i < length; i++) {
            int index = Random.Next(Characters.Length);
            password.Append(Characters[index]);
         }

         return password.ToString();
      }
   }
}

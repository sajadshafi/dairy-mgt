namespace DairyManagementSystem.EmailConfig {
   public class EmailTemplates {
      public static string CreateInvitationTemplate(string email, string password, string name, string url) {
         string template = @"<html>
           <body style=""font-family: Arial, Helvetica, sans-serif"">
             <h4>Dear " + name + @",</h4>
             <p>
               We're excited to welcome you to the Dairy Management System. Your login
               credentials are:
             </p>
             <p>Email: <b>" + email + @"</b></p>
             <p>Password: <b>" + password + @"</b></p>

             <p>Please visit <a href="+ url + @">" + url + @"</a> and log in to explore all the exciting features.</p>
           </body>
         </html>";

         return template;
      }
   }
}

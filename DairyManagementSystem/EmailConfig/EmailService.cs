using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace DairyManagementSystem.EmailConfig {
   public class EmailService : IEmailService {
      private readonly ILogger<EmailService> _logger;

      public EmailService(ILogger<EmailService> logger) {
         _logger = logger;
      }

      public async Task<bool> SendEmailAsync(EmailModel model) {
         try {
            // Create a new MimeMessage
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(EmailConstants.USERNAME));
            email.To.Add(MailboxAddress.Parse(model.EmailTo));
            email.Subject = model.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = model.Body };

            // Create a new SmtpClient
            using var smtp = new SmtpClient();

            // Connect to the SMTP server
            await smtp.ConnectAsync(EmailConstants.HOST, EmailConstants.PORT, SecureSocketOptions.StartTls);
            // Authenticate with the SMTP server
            await smtp.AuthenticateAsync(EmailConstants.USERNAME, EmailConstants.PASSWORD);

            // Send the email
            var result = await smtp.SendAsync(email);

            // Disconnect from the SMTP server
            await smtp.DisconnectAsync(true);
            _logger.LogInformation("Email sent successfully.");
            return true;
         } catch(Exception ex) {
            _logger.LogError("Error sending email: " + ex.ToString());
            return false;
         }
      }

   }

   public interface IEmailService {
      Task<bool> SendEmailAsync(EmailModel model);
   }
}

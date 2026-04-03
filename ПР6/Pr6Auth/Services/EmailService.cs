using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Pr6Auth.Services
{
    public class EmailService
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string emailFrom = "podstrel385@gmail.com";
        private readonly string appPassword = "hyjd xjpo utrd xgap";

        public async Task<bool> SendCodeAsync(string toEmail, string code, string subject = "Код подтверждения")
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Авиакомпания", emailFrom));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = $@"
                        <h2>Здравствуйте!</h2>
                        <p>Ваш код подтверждения: <b>{code}</b></p>
                        <p>Никому не сообщайте этот код.</p>
                        <hr/>
                        <small>Это автоматическое сообщение, не отвечайте на него.</small>"
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(emailFrom, appPassword);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка SMTP: {ex.Message}", "Ошибка отправки", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
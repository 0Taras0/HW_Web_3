using MimeKit;
using System.Net.Mail;
using EmailSender.Constants;

namespace EmailSender.Service;

public static class EmailService
{
    public async static void SendEmail(string to, string subject, string htmlFilePath, string userName)
    {
        await Task.Run(() =>
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(EmailConfiguration.From));
            message.To.Add(new MailboxAddress(to));
            message.Subject = subject;

            string htmlContent = File.ReadAllText(htmlFilePath);
            htmlContent = htmlContent.Replace("{Name}", userName);

            var bodyHtml = new TextPart("html")
            {
                Text = htmlContent
            };

            var bodyText = new TextPart("plain")
            {
                Text = "Ваш поштовий клієнт не підтримує HTML. Будь ласка, відкрийте лист у веб-браузері."
            };

            var alternative = new Multipart("alternative")
        {
            bodyText,
            bodyHtml
        };

            message.Body = alternative;

            using var client = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                client.Connect(EmailConfiguration.SmtpServer, EmailConfiguration.Port, true);
                client.Authenticate(EmailConfiguration.UserName, EmailConfiguration.Password);
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка надсилання: {ex.Message}");
            }
            finally
            {
                client.Disconnect(true);
            }
        });
    }


    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

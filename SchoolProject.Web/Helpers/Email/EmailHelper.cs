using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace SchoolProject.Web.Helpers.Email;

public class EmailHelper : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendEmailAsync(
        string emailTo, string subject, string htmlMessage)
    {
        var fromEmail = _configuration["Email:Email"];
        var fromName = _configuration["Email:Name"];
        var smtpHost = _configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        var smtpUsername = _configuration["Email:SmtpUsername"];
        var smtpPassword = _configuration["Email:SmtpPassword"];


        using (var client = new SmtpClient(smtpHost, smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials =
                new NetworkCredential(smtpUsername, smtpPassword);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(
                    fromEmail ?? "nuno.santos.26288.cinel.pt@gmail.com",
                    fromName ?? "Admin School Project"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                BodyEncoding = System.Text.Encoding.UTF8,
                To = {emailTo},
            };

            Console.WriteLine(
                "Sending an email message to {0} by using SMTP host {1} port {2}.",
                emailTo, client.Host, client.Port);

            try
            {
                return client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "Exception caught in CreateTestMessage4(): {0}",
                    ex.ToString());

                return Task.CompletedTask;
            }

            // return client.SendMailAsync(mailMessage);
        }
    }
}
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

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
        var fromEmail = _configuration[key: "Email:Email"];
        var fromName = _configuration[key: "Email:Name"];
        var smtpHost = _configuration[key: "Email:SmtpHost"];
        var smtpPort = int.Parse(s: _configuration[key: "Email:SmtpPort"] ?? "587");
        var smtpUsername = _configuration[key: "Email:SmtpUsername"];
        var smtpPassword = _configuration[key: "Email:SmtpPassword"];


        using (var client = new SmtpClient(host: smtpHost, port: smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials =
                new NetworkCredential(userName: smtpUsername, password: smtpPassword);
            client.EnableSsl = true;

            var mailMessage = new MailMessage
            {
                From = new MailAddress(
                    address: fromEmail ?? "nuno.santos.26288.cinel.pt@gmail.com",
                    displayName: fromName ?? "Admin School Project"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                To = {emailTo}
            };

            Console.WriteLine(
                format: "Sending an email message to {0} by using SMTP host {1} port {2}.",
                arg0: emailTo, arg1: client.Host, arg2: client.Port);

            try
            {
                return client.SendMailAsync(message: mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    format: "Exception caught in CreateTestMessage4(): {0}",
                    arg0: ex);

                return Task.CompletedTask;
            }

            // return client.SendMailAsync(mailMessage);
        }
    }
}
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SchoolProject.Web.Helpers.Email;

public class E_MailHelper : IE_MailHelper
{
    private readonly IConfiguration _configuration;


    public E_MailHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public AppResponse SendEmail(
        string emailTo, string subject, string htmlMessage)
    {
        var fromEmail = _configuration["Email:Email"];
        var fromName = _configuration["Email:Name"];
        var smtpHost = _configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        var smtpUsername = _configuration["Email:SmtpUsername"];
        var smtpPassword = _configuration["Email:SmtpPassword"];


        try
        {
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
                    BodyEncoding = Encoding.UTF8,
                    To = {emailTo}
                };


                Console.WriteLine(
                    "Sending an email message to {0} " +
                    "using SMTP host {1} and port {2}.",
                    emailTo, client.Host, client.Port);


                client.SendMailAsync(mailMessage);

                return new AppResponse
                {
                    IsSuccess = true
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Exception caught in CreateTestMessage4(): {0}",
                ex);

            return new AppResponse
            {
                IsSuccess = false,
                Message = ex.ToString()
            };
        }
    }
}
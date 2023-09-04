using Azure.Identity;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SchoolProject.Web.Helpers.Email;

public class EMailHelper : IEMailHelper
{
    private readonly IConfiguration _configuration;


    
    
    public EMailHelper(IConfiguration configuration) => _configuration = configuration;




/// <inheritdoc/>
    public AppResponse SendEmail(
        string emailTo, string subject, string htmlMessage)
    {
        var fromEmail = _configuration["Email:From"];
        var fromName = _configuration["Email:NameFrom"];
        var smtpHost = _configuration["Email:Smtp"];
        var smtpPort = int.Parse(_configuration["Email:Port"] ?? "587");
        var smtpUsername = _configuration["Email:From"];
        var smtpPassword = _configuration["Email:Password"];


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


                // TODO: Send email, verify this sh.....
                var result = client.SendMailAsync(mailMessage);
                if (result.IsCompleted)
                {
                    if (result.IsFaulted)
                    {
                        return new AppResponse
                        {
                            IsSuccess = false,
                            Message = result.Exception?.Message
                        };
                    }
                }


                // TODO: Send email, verify this sh.....
                var result1 = client.SendMailAsync(fromEmail, emailTo, subject, htmlMessage);
                if (result.IsCompleted)
                {
                    if (result1.IsFaulted)
                    {
                        return new AppResponse
                        {
                            IsSuccess = false,
                            Message = result1.Exception?.Message
                        };
                    }
                }

                return new AppResponse
                {
                    IsSuccess = true
                };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Exception caught in CreateTestMessage4(): {0}", ex);

            return new AppResponse
            {
                IsSuccess = false,
                Message = ex.ToString()
            };
        }
    }



    /// <inheritdoc/>
    public async Task<AppResponse> SendEmailAsync(
        string emailTo, string subject, string htmlMessage) =>      await            Task.Run(() => SendEmail(emailTo, subject, htmlMessage));



}
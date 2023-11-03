using MailKit.Net.Smtp;
using MimeKit;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Helpers.Email;

public class MailHelper : IMailHelper
{
    private readonly IConfiguration _configuration;

    public MailHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public AppResponse SendEmail(string to, string subject, string body)
    {
        var nameFrom = _configuration["Email:NameFrom"];
        var from = _configuration["Email:EmailFrom"];
        var smtp = _configuration["Email:Smtp"];
        var port = _configuration["Email:Port"];
        var password = _configuration["Email:Password"];

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(nameFrom, from));
        message.To.Add(new MailboxAddress(to, to));
        message.Subject = subject;

        var bodybuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        message.Body = bodybuilder.ToMessageBody();

        try
        {
            using (var client = new SmtpClient())
            {
                client.Connect(smtp, int.Parse(port), false);
                client.Authenticate(from, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            return new AppResponse
            {
                IsSuccess = false, Message = ex.ToString()
            };
        }

        return new AppResponse
        {
            IsSuccess = true, Message = "Email send with success..."
        };
    }

    public bool SendEmail1(string emailTo, string subject, string body)
    {
        var nameFrom = _configuration["Email:NameFrom"];
        var emailFrom = _configuration["Email:EmailFrom"];
        var smtp = _configuration["Email:Smtp"];
        var port = _configuration["Email:Port"];
        var password = _configuration["Email:Password"];

        var length = emailTo.IndexOf("@");
        var nameTo = emailTo[..length];

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(nameFrom, emailFrom));
        message.To.Add(new MailboxAddress(nameTo, emailTo));

        message.Subject = subject;

        message.Body = new BodyBuilder {HtmlBody = body}.ToMessageBody();

        try
        {
            using var client = new SmtpClient();

            client.Connect(smtp, int.Parse(port), false);
            client.Authenticate(emailFrom, password);
            client.Send(message);
            client.Disconnect(true);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool SendPasswordResetEmail(AppUser appUser, string tokenUrl)
    {
        var emailBody = "<h2>Password reset</h2>" +
                        $"<p>Click <a href=\"{tokenUrl}\"><u>here</u></a> to reset password.</p>";

        try
        {
            return SendEmail1(appUser.Email, "Password reset", emailBody);
        }
        catch
        {
            return false;
        }
    }

    public bool SendConfirmationEmail(AppUser appUser, string tokenUrl)
    {
        var emailBody = "<h2>Email confirmation</h2>" +
                        $"<p>Click <a href=\"{tokenUrl}\"><u>here</u></a> to activate account.</p>";

        try
        {
            return SendEmail1(appUser.Email, "Email confirmation", emailBody);
        }
        catch
        {
            return false;
        }
    }
}
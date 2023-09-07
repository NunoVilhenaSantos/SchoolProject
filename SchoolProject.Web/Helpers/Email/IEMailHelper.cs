namespace SchoolProject.Web.Helpers.Email;

public interface IEMailHelper
{
    AppResponse SendEmail(string emailTo, string subject, string htmlMessage);


    Task<AppResponse> SendEmailAsync(string emailTo, string subject,
        string htmlMessage);
}
namespace SchoolProject.Web.Helpers.Email;

public interface IE_MailHelper
{
    AppResponse SendEmail(string emailTo, string subject, string htmlMessage);
}
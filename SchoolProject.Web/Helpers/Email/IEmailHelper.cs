using SchoolProject.Web.Helpers;

namespace SchoolProject.Web.Helpers.Email
{
    public interface IEmailHelper
    {
        Response SendEmail(string emailTo, string subject, string htmlMessage);
    }
}
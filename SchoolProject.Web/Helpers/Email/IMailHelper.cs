using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Helpers.Email;

/// <summary>
/// </summary>
public interface IMailHelper
{
    AppResponse SendEmail(string to, string subject, string body);

    public bool SendEmail1(string emailTo, string subject, string body);

    public bool SendPasswordResetEmail(AppUser appUser, string tokenUrl);

    public bool SendConfirmationEmail(AppUser appUser, string tokenUrl);
}
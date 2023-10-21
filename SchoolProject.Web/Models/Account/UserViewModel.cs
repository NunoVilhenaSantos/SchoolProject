using System.ComponentModel;

namespace SchoolProject.Web.Models.Account;

/// <summary>
///     View model for the appUser class to add an image to the appUser personnel webpage.
/// </summary>
public class UserViewModel
{
    /// <summary>
    ///     image file to be added to the users personnel data
    /// </summary>
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }
}
using System.ComponentModel;

namespace SchoolProject.Web.Models.Users;

/// <summary>
/// View model for the user class to add an image to the user personnel webpage.
/// </summary>
public class UserViewModel
{
    /// <summary>
    /// image file to be added to the users personnel data
    /// </summary>
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }
}
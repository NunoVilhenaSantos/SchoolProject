using System.ComponentModel;

namespace SchoolProject.Web.Models.Users;

/// <summary>
///    View model for the user to add an image.
/// </summary>
public class UserViewModel
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}
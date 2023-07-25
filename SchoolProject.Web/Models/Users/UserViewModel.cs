using System.ComponentModel;

namespace SchoolProject.Web.Models.Users;

public class UserViewModel
{
    [DisplayName("Image")] public IFormFile ImageFile { get; set; }
}
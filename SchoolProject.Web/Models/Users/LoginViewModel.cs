using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Users;

public class LoginViewModel
{
    [Required][EmailAddress] public required string Username { get; set; }


    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public required string Password { get; set; }


    [Required]
    [DisplayName("Remember Me?")]
    public required bool RememberMe { get; set; }
}
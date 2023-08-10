using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Users;

public class LoginViewModel
{
    [Required] [EmailAddress] public required string Username { get; set; }


    [Required]
    [MinLength(length: 6)]
    [DataType(dataType: DataType.Password)]
    public required string Password { get; set; }


    [Required]
    [DisplayName(displayName: "Remember Me?")]
    public required bool RememberMe { get; set; }
}
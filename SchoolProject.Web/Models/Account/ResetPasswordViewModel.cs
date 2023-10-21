using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

public class ResetPasswordViewModel
{
    [Required] public required string UserName { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public required string ConfirmPassword { get; set; }

    [Required] public required string Token { get; set; }
}

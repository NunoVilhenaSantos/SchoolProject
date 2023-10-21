using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

public class ForgotPasswordViewModel
{
    [EmailAddress]
    [Required]
    public required string Email { get; set; }
}

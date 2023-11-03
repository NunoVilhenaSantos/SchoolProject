using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

/// <summary>
/// </summary>
public class ForgotPasswordViewModel
{
    /// <summary>
    /// </summary>
    [EmailAddress]
    [Required]
    public required string Email { get; set; }
}
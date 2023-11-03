using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

/// <summary>
/// </summary>
public class RecoverPasswordViewModel
{
    /// <summary>
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
}
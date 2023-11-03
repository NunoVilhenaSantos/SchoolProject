using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

/// <summary>
/// </summary>
public class LoginViewModel
{
    /// <summary>
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Username { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public required string Password { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Remember Me?")]
    public required bool RememberMe { get; set; }
}
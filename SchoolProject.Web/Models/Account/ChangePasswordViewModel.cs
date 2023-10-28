using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

/// <summary>
///
/// </summary>
public class ChangePasswordViewModel
{
    /// <summary>
    ///
    /// </summary>
    [Required]
    [DisplayName("Current Password")]
    [DataType(DataType.Password)]
    public required string OldPassword { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    [Display(Name = "New Password")]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public required string NewPassword { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    [MinLength(6)]
    public required string ConfirmPassword { get; set; }
}

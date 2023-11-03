using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Account;

/// <summary>
///     View model for the appUser class to add an image to the appUser personnel webpage.
/// </summary>
public class AppUserViewModel : AppUser
{
    /// <summary>
    /// </summary>
    [DisplayName("AppUser Role")]
    public required string Role { get; set; }

    /// <summary>
    /// </summary>
    public bool HasPhoto { get; set; }

    /// <summary>
    /// </summary>
    public bool DeletePhoto { get; set; }
}
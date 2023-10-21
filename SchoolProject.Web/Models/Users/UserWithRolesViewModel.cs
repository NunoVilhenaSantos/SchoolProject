using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Users;

/// <summary>
///     view model to help view the appUser register in the system
///     and with there respective roles in it.
/// </summary>
public class UserWithRolesViewModel
{
    /// <summary>
    ///     appUser class to help display the data
    /// </summary>
    public required AppUser AppUser { get; set; }

    // public IdentityUser IUser { get; set; }


    /// <summary>
    ///     appUser roles
    /// </summary>
    public IdentityUserRole<string>? Role { get; set; }


    /// <summary>
    ///     appUser primary role
    /// </summary>
    public string? RoleName => Roles?.FirstOrDefault();


    /// <summary>
    ///     List of roles for this appUser
    /// </summary>
    public required List<string?> Roles { get; set; }
}
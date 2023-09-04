using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Users;

/// <summary>
///     view model to help view the user register in the system
///     and with there respective roles in it.
/// </summary>
public class UserWithRolesViewModel
{
    /// <summary>
    ///     user class to help display the data
    /// </summary>
    public required User User { get; set; }

    // public IdentityUser IUser { get; set; }


    /// <summary>
    ///     user roles
    /// </summary>
    public IdentityUserRole<string>? Role { get; set; }


    /// <summary>
    ///     user primary role
    /// </summary>
    public string? RoleName => Roles?.FirstOrDefault();


    /// <summary>
    ///     List of roles for this user
    /// </summary>
    public List<string>? Roles { get; set; }
}
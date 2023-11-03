using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Account;

/// <summary>
/// </summary>
public class UpdateAppUserViewModel : AppUser
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
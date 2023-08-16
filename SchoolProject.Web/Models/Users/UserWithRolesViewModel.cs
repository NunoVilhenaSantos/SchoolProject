using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Users;

public class UserWithRolesViewModel
{
    public User User { get; set; }

    public IdentityUser IUser { get; set; }

    public IdentityUserRole<string> Role { get; set; }

    public string RoleName => Roles.FirstOrDefault();

    public List<string> Roles { get; set; }
}
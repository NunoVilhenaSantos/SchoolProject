using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.ExtraTables;

namespace SchoolProject.Web.Helpers;

public interface IUserHelper
{
    Task<User?> GetUserByIdAsync(string id);


    Task<User?> GetUserByEmailAsync(string email);


    Task<IdentityResult> AddUserAsync(User user, string password);
}
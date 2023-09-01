using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Account;

namespace SchoolProject.Web.Helpers.Users;

public interface IUserHelper
{
    Task<User?> GetUserByEmailAsync(string email);


    Task<User?> GetUserByIdAsync(string id);


    Task<string?> GetUserInitialsAsync();


    Task<IdentityResult> AddUserAsync(User? user, string password);


    /// <summary>
    ///     Check if role exists, if not create it.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task CheckRoleAsync(string roleName);


    Task<SignInResult> LoginAsync(LoginViewModel model);


    Task LogOutAsync();


    Task<IdentityResult> UpdateUserAsync(User? user);


    Task<IdentityResult> ChangePasswordAsync(
        User? user, string oldPassword, string newPassword);


    Task AddUserToRoleAsync(User? user, string roleName);


    Task RemoveUserFromRoleAsync(User? user, string roleName);


    Task<bool> IsUserInRoleAsync(User? user, string roleName);


    Task<SignInResult> ValidatePasswordAsync(User user, string password);


    Task<string> GenerateEmailConfirmationTokenAsync(User user);


    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
}
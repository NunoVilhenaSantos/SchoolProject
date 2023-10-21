using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Account;

namespace SchoolProject.Web.Helpers.Users;

/// <summary>
///     The appUser helper interface.
/// </summary>
public interface IUserHelper
{
    /// <summary>
    ///     Gets the appUser by email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<AppUser?> GetUserByEmailAsync(string email);


    /// <summary>
    ///     Gets the appUser by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AppUser?> GetUserByIdAsync(string id);


    /// <summary>
    ///     Gets the appUser by appUser name.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<AppUser?> GetUserByUserNameAsync(string userName);


    /// <summary>
    ///     Gets the appUser initials.
    /// </summary>
    /// <returns></returns>
    Task<string?> GetUserInitialsAsync();


    /// <summary>
    ///     Creates a new appUser.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<IdentityResult> AddUserAsync(AppUser appUser, string password);


    /// <summary>
    ///     Check if role exists, if not create it.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task CheckRoleAsync(string roleName);


    /// <summary>
    ///     logs in the appUser.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<SignInResult> LoginAsync(LoginViewModel model);


    /// <summary>
    ///     Logs out the appUser.
    /// </summary>
    /// <returns></returns>
    Task LogOutAsync();


    /// <summary>
    ///     Updates the appUser.
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    Task<IdentityResult> UpdateUserAsync(AppUser appUser);


    /// <summary>
    ///     Changes the password of the appUser.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    Task<IdentityResult> ChangePasswordAsync(
        AppUser appUser, string oldPassword, string newPassword);


    /// <summary>
    ///     Adds the appUser to the specified role.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task AddUserToRoleAsync(AppUser appUser, string roleName);


    /// <summary>
    ///     Removes the appUser from the specified role.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task RemoveUserFromRoleAsync(AppUser appUser, string roleName);


    /// <summary>
    ///     Checks if the appUser is in the specified role.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<bool> IsUserInRoleAsync(AppUser appUser, string roleName);


    /// <summary>
    ///     Checks if the appUser and the password are valid.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<SignInResult> ValidatePasswordAsync(AppUser appUser, string password);


    /// <summary>
    ///     Generates a token for the appUser to validate the email.
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser);


    /// <summary>
    ///     Confirms the email of the appUser.
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<IdentityResult> ConfirmEmailAsync(AppUser appUser, string token);


    Task SignInAsync(AppUser appUser, bool rememberMe = true,
        string? authenticationMethod = null);


    Task<bool> PasswordSignInAsync(AppUser appUser, bool isPersistent = false,
        bool lockoutOnFailure = false);


    bool IsUserSignInAsync(AppUser appUser, bool rememberMe = true,
        string? authenticationMethod = null);


    bool IsUserAuthenticated();
}
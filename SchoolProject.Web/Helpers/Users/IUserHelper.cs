using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Account;

namespace SchoolProject.Web.Helpers.Users;

/// <summary>
///     The appUser helper interface.
/// </summary>
public interface IUserHelper
{
    /// <summary>
    /// </summary>
    /// <param name="dataContextInUse"></param>
    /// <param name="userManager"></param>
    void Initialize(DataContextMySql dataContextInUse,
        UserManager<AppUser> userManager);


    /// <summary>
    ///     Gets all appUser.
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<AppUser>> GetAllUsersAsync();


    /// <summary>
    ///     Gets the appUser by email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<AppUser?> GetUserByEmailAsync(string email);


    /// <summary>
    ///     Gets the appUser by email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public Task<IQueryable<AppUser>> GetUserByEmailWithCity(string email);


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
    ///     Creates a new appUser.
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    Task<IdentityResult> AddUserAsync(AppUser appUser);


    /// <summary>
    ///     deletes the appUser.
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    Task DeleteUserAsync(AppUser appUser);

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
    ///     Logs out the appUser.
    /// </summary>
    /// <returns></returns>
    Task LogoutAsync();


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


    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="rememberMe"></param>
    /// <param name="authenticationMethod"></param>
    /// <returns></returns>
    Task SignInAsync(
        AppUser appUser, bool rememberMe = true,
        string? authenticationMethod = null);


    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="rememberMe"></param>
    /// <param name="authenticationMethod"></param>
    /// <returns></returns>
    bool IsUserSignInAsync(
        AppUser appUser, bool rememberMe = true,
        string? authenticationMethod = null);


    /// <summary>
    /// </summary>
    /// <returns></returns>
    bool IsUserAuthenticated();


    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="isPersistent"></param>
    /// <param name="lockoutOnFailure"></param>
    /// <returns></returns>
    Task<bool> PasswordSignInAsync(AppUser appUser, bool isPersistent = false,
        bool lockoutOnFailure = false);


    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    Task<string> GeneratePasswordResetTokenAsync(AppUser appUser);

    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="token"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<IdentityResult> ResetPasswordAsync(
        AppUser appUser, string token, string password);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<SelectListItem>> GetComboRolesAsync();

    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <param name="newRole"></param>
    /// <returns></returns>
    Task SetUserRoleAsync(AppUser appUser, string newRole);

    /// <summary>
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<IEnumerable<AppUser>> GetUsersInRoleAsync(string roleName);

    /// <summary>
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    Task<string> GetUserRoleAsync(AppUser appUser);

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> UserExistsAsync(string id);


    /// <summary>
    /// </summary>
    /// <returns></returns>
    IOrderedQueryable<AppUser> GetUsersWithFullName();
}
using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Account;

namespace SchoolProject.Web.Helpers.Users;

/// <summary>
///  The user helper interface.
/// </summary>
public interface IUserHelper
{
    /// <summary>
    ///   Gets the user by email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<User?> GetUserByEmailAsync(string email);


    /// <summary>
    ///  Gets the user by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<User?> GetUserByIdAsync(string id);


    /// <summary>
    /// Gets the user by user name.
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    Task<User?> GetUserByUserNameAsync(string userName);


    /// <summary>
    ///  Gets the user initials.
    /// </summary>
    /// <returns></returns>
    Task<string?> GetUserInitialsAsync();


    /// <summary>
    ///    Creates a new user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<IdentityResult> AddUserAsync(User user, string password);


    /// <summary>
    ///     Check if role exists, if not create it.
    /// </summary>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task CheckRoleAsync(string roleName);


    /// <summary>
    ///  logs in the user.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<SignInResult> LoginAsync(LoginViewModel model);


    /// <summary>
    ///   Logs out the user.
    /// </summary>
    /// <returns></returns>
    Task LogOutAsync();


    /// <summary>
    ///    Updates the user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<IdentityResult> UpdateUserAsync(User user);


    /// <summary>
    ///    Changes the password of the user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="oldPassword"></param>
    /// <param name="newPassword"></param>
    /// <returns></returns>
    Task<IdentityResult> ChangePasswordAsync(
        User user, string oldPassword, string newPassword);


    /// <summary>
    ///   Adds the user to the specified role.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task AddUserToRoleAsync(User user, string roleName);


    /// <summary>
    ///  Removes the user from the specified role.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task RemoveUserFromRoleAsync(User user, string roleName);


    /// <summary>
    ///   Checks if the user is in the specified role.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    Task<bool> IsUserInRoleAsync(User user, string roleName);


    /// <summary>
    ///   Checks if the user and the password are valid.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<SignInResult> ValidatePasswordAsync(User user, string password);


    /// <summary>
    ///    Generates a token for the user to validate the email.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<string> GenerateEmailConfirmationTokenAsync(User user);


    /// <summary>
    ///    Confirms the email of the user.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<IdentityResult> ConfirmEmailAsync(User user, string token);
}
using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Helpers.Users;

public class UserHelper : IUserHelper
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;


    public UserHelper(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }


    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email: email);
    }


    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(userId: id);
    }


    public async Task<IdentityResult> AddUserAsync(
        User? user, string password)
    {
        // Check if user object is not null before proceeding
        if (user == null)
            throw new ArgumentNullException(paramName: nameof(user),
                message: "User object cannot be null.");

        return await _userManager.CreateAsync(user: user, password: password);
    }


    public async Task CheckRoleAsync(string roleName)
    {
        var result = await _roleManager.RoleExistsAsync(roleName: roleName);

        if (!result)
            await _roleManager.CreateAsync(role: new IdentityRole {Name = roleName});
    }


    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return await _signInManager.PasswordSignInAsync(
            userName: model.Username,
            password: model.Password,
            isPersistent: model.RememberMe,
            lockoutOnFailure: false);
    }


    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> UpdateUserAsync(User? user)
    {
        return await _userManager.UpdateAsync(user: user);
    }


    public async Task<IdentityResult> ChangePasswordAsync(
        User? user, string oldPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(
            user: user, currentPassword: oldPassword, newPassword: newPassword);
    }

    public async Task AddUserToRoleAsync(User? user, string roleName)
    {
        await _userManager.AddToRoleAsync(user: user, role: roleName);
    }


    public async Task RemoveUserFromRoleAsync(User? user, string roleName)
    {
        await _userManager.RemoveFromRoleAsync(user: user, role: roleName);
    }


    public async Task<bool> IsUserInRoleAsync(User? user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user: user, role: roleName);
    }


    public async Task<SignInResult> ValidatePasswordAsync(
        User user, string password)
    {
        return await _signInManager.CheckPasswordSignInAsync(
            user: user, password: password, lockoutOnFailure: false);
    }


    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName: userName);
    }


    public async Task<string> GenerateEmailConfirmationTokenAsync(User? user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user: user);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(
        User? user, string token)
    {
        return await _userManager.ConfirmEmailAsync(user: user, token: token);
    }
}
using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Helpers.Users;

public class UserHelperRuben : IUserHelper
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserHelperRuben(UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> AddUserAsync(User user,
        string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return await _signInManager.PasswordSignInAsync(model.Username,
            model.Password, model.RememberMe, false);
    }

    public async Task LogOutAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> ChangePasswordAsync(User user,
        string oldPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, oldPassword,
            newPassword);
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task CheckRoleAsync(string roleName)
    {
        var roleExists = await _roleManager.RoleExistsAsync(roleName);

        if (!roleExists)
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = roleName
            });
    }

    public async Task AddUserToRoleAsync(User user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task RemoveUserFromRoleAsync(User user, string roleName)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserInRoleAsync(User user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<SignInResult> ValidatePasswordAsync(User user,
        string pass)
    {
        return await _signInManager.CheckPasswordSignInAsync(user, pass,
            false);
    }

    public async Task<User> GetUserByIdAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(User user,
        string token)
    {
        return await _userManager.ConfirmEmailAsync(user, token);
    }
}
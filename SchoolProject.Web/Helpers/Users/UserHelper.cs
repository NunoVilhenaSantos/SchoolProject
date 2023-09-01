using System.Text;
using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Account;

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
        return await _userManager.FindByEmailAsync(email);
    }


    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }


    public async Task<string?> GetUserInitialsAsync()
    {
        // var user = await _userManager.GetUserAsync(
        //     _signInManager.Context.User);
        //
        // if (user == null) return null;
        //
        // return string.Concat(
        //     user.FirstName.AsSpan(0, 1),
        //     user.LastName.AsSpan(0, 1));


        var user = await _userManager.GetUserAsync(
            _signInManager.Context.User);

        if (user == null || string.IsNullOrEmpty(user.FullName)) return null;

        var nameParts = user.FullName.Split(' ',
            StringSplitOptions.RemoveEmptyEntries);

        if (nameParts.Length == 0) return null;

        var initials = new StringBuilder();

        foreach (var part in nameParts) initials.Append(part[0]);

        return initials.ToString();
    }


    public async Task<IdentityResult> AddUserAsync(
        User user, string password)
    {
        // Check if user object is not null before proceeding
        if (user == null)
            throw new ArgumentNullException(nameof(user),
                "User object cannot be null.");

        return await _userManager.CreateAsync(user, password);
    }


    public async Task CheckRoleAsync(string roleName)
    {
        var result = await _roleManager.RoleExistsAsync(roleName);

        if (!result)
            await _roleManager.CreateAsync(new IdentityRole {Name = roleName});
    }


    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return await _signInManager.PasswordSignInAsync(
            model.Username,
            model.Password,
            model.RememberMe,
            false);
    }


    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }


    public async Task<IdentityResult> ChangePasswordAsync(
        User user, string oldPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(
            user, oldPassword, newPassword);
    }

    public async Task AddUserToRoleAsync(User user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName);
    }


    public async Task RemoveUserFromRoleAsync(User user, string roleName)
    {
        await _userManager.RemoveFromRoleAsync(user, roleName);
    }


    public async Task<bool> IsUserInRoleAsync(User user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user, roleName);
    }


    public async Task<SignInResult> ValidatePasswordAsync(
        User user, string password)
    {
        return await _signInManager.CheckPasswordSignInAsync(
            user, password, false);
    }


    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }

    public async Task<IdentityResult> ConfirmEmailAsync(
        User user, string token)
    {
        return await _userManager.ConfirmEmailAsync(user, token);
    }


    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }
}
using System.Text;
using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Account;

namespace SchoolProject.Web.Helpers.Users;

/// <summary>
///     The appUser helper.
/// </summary>
public class UserHelper : IUserHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;


    /// <summary>
    ///     The appUser helper constructor.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="roleManager"></param>
    public UserHelper(
        IHttpContextAccessor httpContextAccessor,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        RoleManager<IdentityRole> roleManager
    )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;

        _httpContextAccessor = httpContextAccessor;
    }


    /// <inheritdoc />
    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }


    /// <inheritdoc />
    public async Task<AppUser?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }


    /// <inheritdoc />
    public async Task<AppUser?> GetUserByUserNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }


    /// <inheritdoc />
    public async Task<string?> GetUserInitialsAsync()
    {
        // var appUser = await _userManager.GetUserAsync(
        //     _signInManager.Context.AppUser);
        //
        // if (appUser == null) return null;
        //
        // return string.Concat(
        //     appUser.FirstName.AsSpan(0, 1),
        //     appUser.LastName.AsSpan(0, 1));


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


    /// <inheritdoc />
    public async Task<IdentityResult> AddUserAsync(
        AppUser appUser, string password)
    {
        // Check if appUser object is not null before proceeding
        if (appUser == null)
            throw new ArgumentNullException(nameof(appUser),
                "AppUser object cannot be null.");

        return await _userManager.CreateAsync(appUser, password);
    }


    /// <inheritdoc />
    public async Task CheckRoleAsync(string roleName)
    {
        var result = await _roleManager.RoleExistsAsync(roleName);

        if (!result)
            await _roleManager.CreateAsync(new IdentityRole {Name = roleName});
    }


    /// <inheritdoc />
    public async Task<SignInResult> LoginAsync(LoginViewModel model)
    {
        return await _signInManager.PasswordSignInAsync(
            model.Username,
            model.Password,
            model.RememberMe,
            false);
    }


    /// <inheritdoc />
    public async Task LogOutAsync()
    {
        await _signInManager.SignOutAsync();
    }


    /// <inheritdoc />
    public async Task<IdentityResult> UpdateUserAsync(AppUser appUser)
    {
        return await _userManager.UpdateAsync(appUser);
    }


    /// <inheritdoc />
    public async Task<IdentityResult> ChangePasswordAsync(
        AppUser appUser, string oldPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(
            appUser, oldPassword, newPassword);
    }


    /// <inheritdoc />
    public async Task AddUserToRoleAsync(AppUser appUser, string roleName)
    {
        await _userManager.AddToRoleAsync(appUser, roleName);
    }


    /// <inheritdoc />
    public async Task RemoveUserFromRoleAsync(AppUser appUser, string roleName)
    {
        await _userManager.RemoveFromRoleAsync(appUser, roleName);
    }


    /// <inheritdoc />
    public async Task<bool> IsUserInRoleAsync(AppUser appUser, string roleName)
    {
        return await _userManager.IsInRoleAsync(appUser, roleName);
    }


    /// <inheritdoc />
    public async Task<SignInResult>
        ValidatePasswordAsync(AppUser appUser, string password)
    {
        return await _signInManager.CheckPasswordSignInAsync(
            appUser, password, false);
    }


    /// <inheritdoc />
    public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
    }


    /// <inheritdoc />
    public async Task<IdentityResult> ConfirmEmailAsync(AppUser appUser, string token)
    {
        return await _userManager.ConfirmEmailAsync(appUser, token);
    }


    /// <inheritdoc />
    public async Task SignInAsync(AppUser appUser, bool rememberMe = true,
        string? authenticationMethod = null)
    {
        await _signInManager.SignInAsync(appUser, rememberMe,
            authenticationMethod);
    }


    /// <inheritdoc />
    public async Task<bool> PasswordSignInAsync(AppUser appUser,
        bool isPersistent = false, bool lockoutOnFailure = false)
    {
        var signInResult = false;


        // Faz o signin do usuário
        var result = await _signInManager.PasswordSignInAsync(appUser.UserName,
            appUser.PasswordHash, isPersistent, lockoutOnFailure);


        // Verifica se o usuário foi autenticado com sucesso
        signInResult = result.Succeeded;


        return signInResult;
    }


    /// <inheritdoc />
    public bool IsUserSignInAsync(
        AppUser appUser, bool rememberMe = true, string? authenticationMethod = null)
    {
        return _signInManager.Context.User.Identity.IsAuthenticated;
    }


    /// <inheritdoc />
    public bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }
}
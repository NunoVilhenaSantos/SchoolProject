using System.Text;
using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Account;


namespace SchoolProject.Web.Helpers.Users;

/// <summary>
///     The user helper.
/// </summary>
public class UserHelper : IUserHelper
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    

    private readonly IHttpContextAccessor _httpContextAccessor;

    



    /// <summary>
    ///     The user helper constructor.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="roleManager"></param>
    public UserHelper(
        IHttpContextAccessor httpContextAccessor,
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;

        _httpContextAccessor = httpContextAccessor;
    }


    /// <inheritdoc />
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }


    /// <inheritdoc />
    public async Task<User?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }


    /// <inheritdoc />
    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }


    /// <inheritdoc />
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


    /// <inheritdoc />
    public async Task<IdentityResult> AddUserAsync(
        User user, string password)
    {
        // Check if user object is not null before proceeding
        if (user == null)
            throw new ArgumentNullException(nameof(user),
                "User object cannot be null.");

        return await _userManager.CreateAsync(user, password);
    }


    /// <inheritdoc />
    public async Task CheckRoleAsync(string roleName)
    {
        var result = await _roleManager.RoleExistsAsync(roleName);

        if (!result)
            await _roleManager.CreateAsync(new() {Name = roleName});
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
    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        return await _userManager.UpdateAsync(user);
    }


    /// <inheritdoc />
    public async Task<IdentityResult> ChangePasswordAsync(
        User user, string oldPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(
            user, oldPassword, newPassword);
    }


    /// <inheritdoc />
    public async Task AddUserToRoleAsync(User user, string roleName)
    {
        await _userManager.AddToRoleAsync(user, roleName);
    }


    /// <inheritdoc />
    public async Task RemoveUserFromRoleAsync(User user, string roleName)
    {
        await _userManager.RemoveFromRoleAsync(user, roleName);
    }


    /// <inheritdoc />
    public async Task<bool> IsUserInRoleAsync(User user, string roleName)
    {
        return await _userManager.IsInRoleAsync(user, roleName);
    }


    /// <inheritdoc />
    public async Task<Microsoft.AspNetCore.Identity.SignInResult> 
        ValidatePasswordAsync(User user, string password)
    {
        return await _signInManager.CheckPasswordSignInAsync(
            user, password, false);
    }


    /// <inheritdoc />
    public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(user);
    }


    /// <inheritdoc />
    public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
    {
        return await _userManager.ConfirmEmailAsync(user, token);
    }



    /// <inheritdoc/>
    public async Task SignInAsync(User user, bool rememberMe = true, string? authenticationMethod = null)
    {
        await _signInManager.SignInAsync(user, rememberMe, authenticationMethod);
    }


    /// <inheritdoc/>
    public async Task<bool> PasswordSignInAsync(User user, bool isPersistent= false, bool lockoutOnFailure = false)
    {
        bool signInResult = false;


        // Faz o signin do usuário
        var result = await _signInManager.PasswordSignInAsync(user.UserName, user.PasswordHash, isPersistent, lockoutOnFailure);


        // Verifica se o usuário foi autenticado com sucesso
        signInResult = result.Succeeded;


        return signInResult;
    }


    /// <inheritdoc/>
    public bool IsUserSignInAsync(
        User user, bool rememberMe = true, string? authenticationMethod = null) => _signInManager.Context.User.Identity.IsAuthenticated;



    /// <inheritdoc/>
    public bool IsUserAuthenticated() => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

}
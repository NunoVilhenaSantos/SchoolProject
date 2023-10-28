using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Models.Account;

namespace SchoolProject.Web.Helpers.Users;

/// <summary>
///     The appUser helper.
/// </summary>
public class UserHelper : IUserHelper
{
    // Data contexts
    private readonly DataContextMySql _dataContextMySql;

    // HttpContext
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Identity
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;


    // Data contexts and user manager in use
    private UserManager<AppUser> _userManagerInUse;
    private DataContextMySql _dataContextInUse;


    /// <summary>
    ///     The appUser helper constructor.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="userManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="dataContextMySql"></param>
    public UserHelper(
        IHttpContextAccessor httpContextAccessor,
        RoleManager<IdentityRole> roleManager,
        SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
        DataContextMySql dataContextMySql)
    {
        _httpContextAccessor = httpContextAccessor;
        _dataContextMySql = dataContextMySql;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userManager = userManager;
    }


    /// <summary>
    ///    The appUser helper constructor.
    /// </summary>
    /// <param name="dataContextInUse"></param>
    /// <param name="userManagerInUse"></param>
    public void Initialize(
        DataContextMySql dataContextInUse,
        UserManager<AppUser> userManagerInUse)
    {
        _dataContextInUse = dataContextInUse;
        _userManagerInUse = userManagerInUse;
    }


    /// <inheritdoc />
    public IOrderedQueryable<AppUser> GetUsersWithFullName()
    {
        // return await _userManager.Users.AsNoTracking().Select(o =>
        //         new UserWithFullNameViewModel
        //         {
        //             Id = o.Id,
        //             FullName = o.FullName
        //         }
        //     ).OrderBy(o => o.FullName).ToListAsync();

        return _userManager.Users.OrderBy(e => e.FirstName);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
    {
        return await _userManager.Users.AsNoTracking().ToListAsync();
    }

    /// <inheritdoc />
    public async Task<AppUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    /// <inheritdoc />
    public async Task<IQueryable<AppUser>> GetUserByEmailWithCity(string email)
    {
        await _userManager.FindByEmailAsync(email);

        return _dataContextMySql.Users
            // .Include(o => o.City)
            // .ThenInclude(o => o.Country)
            .Where(o => o.Email == email);
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

        if (user == null || string.IsNullOrEmpty(user.FirstName)) return null;

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
    public async Task<IdentityResult> AddUserAsync(AppUser appUser)
    {
        return await _userManager.CreateAsync(appUser);
    }


    /// <inheritdoc />
    public async Task DeleteUserAsync(AppUser appUser)
    {
        await _userManager.DeleteAsync(appUser);
    }


    /// <inheritdoc />
    public async Task CheckRoleAsync(string roleName)
    {
        var result = await _roleManager.RoleExistsAsync(roleName);

        if (!result)
            await _roleManager.CreateAsync(new IdentityRole {Name = roleName});
    }


    /// <inheritdoc />
    public IdentityRole? GetUserRole(string userId)
    {
        // Get the appUser from the database.
        var user = _dataContextMySql.Users.Find(userId);

        // If the appUser is not found, return null.
        if (user == null) return null;

        var userRoleId = _dataContextMySql.UserRoles
            .Where(e => e.UserId == userId)
            .ToList()[0].RoleId;

        // If the appUser is found, get their role.
        return _dataContextMySql.Roles.Find(userRoleId);
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
    public async Task LogoutAsync()
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
    public async Task<string> GenerateEmailConfirmationTokenAsync(
        AppUser appUser)
    {
        return await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
    }


    /// <inheritdoc />
    public async Task<IdentityResult> ConfirmEmailAsync(AppUser appUser,
        string token)
    {
        return await _userManager.ConfirmEmailAsync(appUser, token);
    }


    /// <inheritdoc />
    public async Task SignInAsync(
        AppUser appUser, bool rememberMe = true,
        string? authenticationMethod = null)
    {
        await _signInManager.SignInAsync(appUser, rememberMe,
            authenticationMethod);
    }


    /// <inheritdoc />
    public async Task<bool> PasswordSignInAsync(
        AppUser appUser,
        bool isPersistent = false, bool lockoutOnFailure = false)
    {
        var signInResult = false;


        // Faz o signin do usuário
        var result = await _signInManager.PasswordSignInAsync(
            appUser.UserName, appUser.PasswordHash, isPersistent,
            lockoutOnFailure);


        // Verifica se o usuário foi autenticado com sucesso
        signInResult = result.Succeeded;


        return signInResult;
    }


    /// <inheritdoc />
    public bool IsUserSignInAsync(
        AppUser appUser, bool rememberMe = true,
        string? authenticationMethod = null)
    {
        return _signInManager.Context.User.Identity.IsAuthenticated;
    }


    /// <inheritdoc />
    public bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }


    /// <inheritdoc />
    public async Task<IdentityResult> ResetPasswordAsync(
        AppUser appUser, string token, string password)
    {
        return await _userManager.ResetPasswordAsync(appUser, token, password);
    }


    /// <inheritdoc />
    public async Task<string> GeneratePasswordResetTokenAsync(AppUser appUser)
    {
        return await _userManager.GeneratePasswordResetTokenAsync(appUser);
    }


    /// <inheritdoc />
    public async Task<IEnumerable<SelectListItem>> GetComboRolesAsync()
    {
        return await _roleManager.Roles
            .Select(r => new SelectListItem
                {Text = r.Name, Value = r.Name}).ToListAsync();
    }


    /// <inheritdoc />
    public async Task SetUserRoleAsync(AppUser appUser, string newRole)
    {
        var userRoles = await _userManager.GetRolesAsync(appUser);

        await _userManager.RemoveFromRolesAsync(appUser, userRoles);

        await _userManager.AddToRoleAsync(appUser, newRole);
    }


    /// <inheritdoc />
    public async Task<IEnumerable<AppUser>> GetUsersInRoleAsync(string roleName)
    {
        return await _userManager.GetUsersInRoleAsync(roleName);
    }


    /// <inheritdoc />
    public async Task<string> GetUserRoleAsync(AppUser appUser)
    {
        return (await _userManager.GetRolesAsync(appUser))[0];
    }


    /// <inheritdoc />
    public async Task<bool> UserExistsAsync(string id)
    {
        return await _userManager.Users.AsNoTracking()
            .AnyAsync(user => user.Id == id);
    }
}
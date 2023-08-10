using System.Security.Claims;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Helpers.Users;

public class AuthenticatedUserInApp
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserHelper _userHelper;


    public AuthenticatedUserInApp(
        IUserHelper userHelper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userHelper = userHelper;
        _httpContextAccessor = httpContextAccessor;
    }


    /// <summary>
    ///    Get authenticated user from HttpContext
    /// </summary>
    /// <returns></returns>
    public async Task<User> GetAuthenticatedUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var userId = user?.FindFirstValue(
            claimType: ClaimTypes.NameIdentifier);

        var userEmail = user?.FindFirstValue(claimType: ClaimTypes.Email);

        if (userId is null || userEmail is null) return null;

        return
            await _userHelper.GetUserByIdAsync(id: userId) ??
            await _userHelper.GetUserByEmailAsync(email: userEmail);
    }
}
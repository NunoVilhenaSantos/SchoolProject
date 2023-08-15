using SchoolProject.Web.Data.Entities.Users;
using System.Security.Claims;

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
    ///     Get authenticated user from HttpContext
    /// </summary>
    /// <returns></returns>
    public async Task<User> GetAuthenticatedUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var userId = user?.FindFirstValue(
            ClaimTypes.NameIdentifier);

        var userEmail = user?.FindFirstValue(ClaimTypes.Email);

        if (userId is null || userEmail is null) return null;

        return
            await _userHelper.GetUserByIdAsync(userId) ??
            await _userHelper.GetUserByEmailAsync(userEmail);
    }
}
using System.Security.Claims;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.EntitiesMatrix;

public class AuthenticatedUser
{
    private static IUserHelper _userHelper;
    private static IHttpContextAccessor _httpContextAccessor;


    public AuthenticatedUser(
        IUserHelper userHelper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userHelper = userHelper;
        _httpContextAccessor = httpContextAccessor;
    }


    public static async Task<User?> GetUser()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var userId = user?.FindFirstValue(
            ClaimTypes.NameIdentifier);

        var userEmail = user?.FindFirstValue(
            ClaimTypes.Email);

        if (userId is null || userEmail is null) return null;

        return
            await _userHelper.GetUserByIdAsync(userId) ??
            await _userHelper.GetUserByEmailAsync(userEmail);
    }
}
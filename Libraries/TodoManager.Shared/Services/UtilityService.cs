using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using TodoManager.Shared.CustomExceptions;
using TodoManager.Shared.Entities;

namespace TodoManager.Shared.Services;

public class UtilityService : IUtilityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public UtilityService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<ApplicationUser> GetLoggedInUserAsync()
    {
        var loggedInUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        if (loggedInUser is null)
            throw new ApiException("User doesn't exists or not logged in", StatusCodes.Status400BadRequest);

        return loggedInUser;
    }

    public DateTime? ConvertToDateTime(long? deadline)
    {
        if (deadline is null)
            return null;

        var date = new DateTime(1970, 1, 1, 0, 0, 0, 0); // epoch start
        date = date.AddMilliseconds((double)deadline);
        return date;
    }
}

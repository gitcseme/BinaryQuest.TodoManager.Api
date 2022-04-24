using TodoManager.Shared.Entities;

namespace TodoManager.Shared.Services;

public interface IUtilityService
{
    Task<ApplicationUser> GetLoggedInUserAsync();

    DateTime? ConvertToDateTime(long? deadline);
}

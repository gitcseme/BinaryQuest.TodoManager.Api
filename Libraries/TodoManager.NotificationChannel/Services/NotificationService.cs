using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TodoManager.Membership.Entities;
using TodoManager.NotificationChannel.Entities;
using TodoManager.Shared.CustomExceptions;
using TodoManager.Shared.NotificationDtos;

namespace TodoManager.NotificationChannel.Services;

public class NotificationService : INotificationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationRepositoryManager _repository;

    public NotificationService(
        IHttpContextAccessor httpContextAccessor, 
        UserManager<ApplicationUser> userManager, 
        INotificationRepositoryManager repository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<IEnumerable<NotificationResponseDto>> GetAllAsync(bool trackChanges)
    {
        var user = await GetLoggedInUserAsync();

        IEnumerable<NotificationResponseDto> notifications = await _repository.Notifications
            .Find(n => n.TodoCreatorId.Equals(user.Id), trackChanges)
            .OrderByDescending(n => n.Date)
            .Select(n => PrepareNotificationResponse(n))
            .ToListAsync();

        return notifications;
    }

    private static NotificationResponseDto PrepareNotificationResponse(Notification n)
    {
        return new NotificationResponseDto
        {
            id = n.Id,
            TodoId = n.TodoId,
            TodoCreatorId = n.TodoCreatorId,
            Message = n.Message,
            IsSeen = n.IsSeen,
            Type = n.Type.ToString()
        };
    }

    public async Task MarkSeenAsync(long id)
    {
        var user = await GetLoggedInUserAsync();

        var notificationEntity = await _repository.Notifications
            .Find(n => n.Id.Equals(id) && n.TodoCreatorId.Equals(user.Id), trackChanges: false)
            .FirstOrDefaultAsync();

        if (notificationEntity is null)
            throw new ApiException("Notification not found", StatusCodes.Status404NotFound);

        // Mark seen
        notificationEntity.IsSeen = true;
        
        await _repository.Notifications.Update(notificationEntity);
        await _repository.SaveChanges();
    }

    /* Utility Functions */

    private async Task<ApplicationUser> GetLoggedInUserAsync()
    {
        var loggedInUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
        if (loggedInUser is null)
            throw new ApiException("User doesn't exists or not logged in", StatusCodes.Status400BadRequest);

        return loggedInUser;
    }
}

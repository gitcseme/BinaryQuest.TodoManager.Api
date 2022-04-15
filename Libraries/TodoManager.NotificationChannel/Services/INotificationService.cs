using TodoManager.Shared.NotificationDtos;

namespace TodoManager.NotificationChannel.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationResponseDto>> GetAllAsync();
    Task MarkSeenAsync(long id);
}

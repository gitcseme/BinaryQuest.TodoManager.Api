using TodoManager.Shared.NotificationDtos;

namespace TodoManager.NotificationChannel.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationResponseDto>> GetAllAsync(bool trackChanges);
    Task MarkSeenAsync(long id);
}

using TodoManager.NotificationChannel.Repositories;

namespace TodoManager.NotificationChannel;

public interface INotificationRepositoryManager
{
    INotificationRepository Notifications { get; }

    Task SaveChanges();
}

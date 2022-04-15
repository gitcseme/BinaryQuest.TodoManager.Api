using TodoManager.NotificationChannel.Repositories;

namespace TodoManager.NotificationChannel;

public interface INotificatoinRepositoryManager
{
    INotificationRepository Notifications { get; }

    Task SaveChanges();
}

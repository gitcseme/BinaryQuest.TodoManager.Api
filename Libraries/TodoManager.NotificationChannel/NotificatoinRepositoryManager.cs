using TodoManager.NotificationChannel.Repositories;

namespace TodoManager.NotificationChannel;

public class NotificatoinRepositoryManager : INotificatoinRepositoryManager
{
    private readonly NotificationContext _notificationContext;

    public NotificatoinRepositoryManager(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
        Notifications = new NotificationRepository(_notificationContext);
    }

    public async Task SaveChanges() => await _notificationContext.SaveChangesAsync();

    public INotificationRepository Notifications { get; set; }
}

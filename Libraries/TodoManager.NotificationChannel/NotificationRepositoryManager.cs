using TodoManager.NotificationChannel.Repositories;

namespace TodoManager.NotificationChannel;

public class NotificationRepositoryManager : INotificationRepositoryManager
{
    private readonly NotificationContext _notificationContext;

    public NotificationRepositoryManager(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
        Notifications = new NotificationRepository(_notificationContext);
    }

    public async Task SaveChanges() => await _notificationContext.SaveChangesAsync();

    public INotificationRepository Notifications { get; set; }
}

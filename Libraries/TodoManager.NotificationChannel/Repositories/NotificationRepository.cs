using TodoManager.Core;
using TodoManager.NotificationChannel.Entities;

namespace TodoManager.NotificationChannel.Repositories;

public class NotificationRepository
    : RepositoryBase<NotificationContext, Notification, long>, INotificationRepository
{
    public NotificationRepository(NotificationContext context) : base(context)
    {
    }
}

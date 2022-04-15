using TodoManager.Core;
using TodoManager.NotificationChannel.Entities;

namespace TodoManager.NotificationChannel.Repositories;

public interface INotificationRepository : IRepositoryBase<Notification, long>
{
}

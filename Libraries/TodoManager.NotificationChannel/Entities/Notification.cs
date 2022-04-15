using TodoManager.Core;
using TodoManager.NotificationChannel.Enums;

namespace TodoManager.NotificationChannel.Entities;

public class Notification : EntityBase<long>
{
    public long? TodoId { get; set; }
    public string Message { get; set; }
    public bool IsSeen { get; set; }
    public NotificationType Type { get; set; }
    public DateTime Date { get; set; }
}

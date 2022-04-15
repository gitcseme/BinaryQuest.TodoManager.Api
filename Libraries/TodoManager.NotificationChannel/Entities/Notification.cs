using TodoManager.Core;

namespace TodoManager.NotificationChannel.Entities;

public class Notification : EntityBase<long>
{
    public long UserId { get; set; }
    public string Message { get; set; }
    public bool IsSeen { get; set; }
    public DateTime Date { get; set; }
}

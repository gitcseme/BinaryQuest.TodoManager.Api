using TodoManager.Shared.Enums;

namespace TodoManager.Shared.NotificationDtos;

public class NotificationResponseDto
{
    public long? TodoId { get; set; }
    public long? TodoCreatorId { get; set; }
    public string Message { get; set; }
    public bool IsSeen { get; set; }
    public NotificationType Type { get; set; }
}

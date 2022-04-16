using TodoManager.Shared.Enums;

namespace TodoManager.Shared.NotificationDtos;

public class NotificationResponseDto
{
    public long id { get; set; }
    public long? TodoId { get; set; }
    public long? TodoCreatorId { get; set; }
    public string Message { get; set; }
    public bool IsSeen { get; set; }
    public string Type { get; set; }
}

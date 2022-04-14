using TodoManager.Core;

namespace TodoManager.Data.Entities;

public class Todo : EntityBase<long>
{
    public string Description { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public DateTime? Deadline { get; set; }

    public bool IsDone { get; set; }
    public long CreatorId { get; set; } // foreign key user-id
}

using System.ComponentModel.DataAnnotations;

namespace TodoManager.Shared.TodoDtos;

public class TodoUpdateDto
{
    public string Description { get; set; }

    public bool IsDone { get; set; }

    public long? Deadline { get; set; }
}
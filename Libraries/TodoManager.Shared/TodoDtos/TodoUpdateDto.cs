using System.ComponentModel.DataAnnotations;

namespace TodoManager.Shared.TodoDtos;

public class TodoUpdateDto
{
    [Required, StringLength(255)]
    public string Description { get; set; }

    [Required]
    public bool IsDone { get; set; }

    public long? Deadline { get; set; }
}
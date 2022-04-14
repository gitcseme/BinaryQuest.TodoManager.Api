using System.ComponentModel.DataAnnotations;

namespace TodoManager.Shared.TodoDtos;

public class TodoCreateDto
{
    [Required, StringLength(255)]
    public string Description { get; set; }
}
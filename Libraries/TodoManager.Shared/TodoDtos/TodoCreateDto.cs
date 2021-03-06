using System.ComponentModel.DataAnnotations;

namespace TodoManager.Shared.TodoDtos;

public class TodoCreateDto
{
    [Required, StringLength(255, ErrorMessage = "Description must have less than 255 characters")]
    public string Description { get; set; }
}
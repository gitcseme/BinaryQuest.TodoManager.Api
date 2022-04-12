namespace TodoManager.Shared.TodoDtos;

public record TodoResponseDto(long Id, string Description, DateTime CreatedOn, DateTime UpdatedOn, bool IsDone);

namespace TodoManager.Shared.TodoDtos;

public class TodoResponseDto 
{
    public long Id { get; set; }
    public string Description { get; set; }
    public string CreatedOn { get; set; }
    public string UpdatedOn { get; set; }
    public bool IsDone { get; set; }
}

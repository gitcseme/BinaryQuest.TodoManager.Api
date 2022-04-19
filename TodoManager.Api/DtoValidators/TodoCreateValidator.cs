using FluentValidation;
using TodoManager.Shared.TodoDtos;

namespace TodoManager.Api.DtoValidators;

public class TodoCreateValidator : AbstractValidator<TodoCreateDto>
{
    public TodoCreateValidator()
    {
        RuleFor(todo => todo.Description)
            .NotNull()
            .MaximumLength(255);
    }
}

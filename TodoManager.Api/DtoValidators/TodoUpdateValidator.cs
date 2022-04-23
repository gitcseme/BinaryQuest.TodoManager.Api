using FluentValidation;
using TodoManager.Shared.TodoDtos;

namespace TodoManager.Api.DtoValidators;

public class TodoUpdateValidator : AbstractValidator<TodoUpdateDto>
{
    public TodoUpdateValidator()
    {
        RuleFor(model => model.Description)
            .NotNull()
            .MaximumLength(255);

        RuleFor(model => model.IsDone)
            .NotNull();
    }
}

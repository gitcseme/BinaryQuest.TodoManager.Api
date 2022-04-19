using FluentValidation;
using TodoManager.Membership.AuthModels;

namespace TodoManager.Api.DtoValidators;

public class SigninModelValidator : AbstractValidator<SigninModel>
{
    public SigninModelValidator()
    {
        RuleFor(model => model.Email)
            .EmailAddress()
            .NotNull();

        RuleFor(model => model.Password)
            .MinimumLength(4)
            .Must(p => p.Contains('$'))
            .WithMessage("Must contain '$' in password")
            .NotNull();

        RuleFor(model => model.RememberMe).NotNull();
    }
}

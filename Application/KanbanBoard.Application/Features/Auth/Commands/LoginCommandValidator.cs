using FluentValidation;

namespace KanbanBoard.Application.Features.Auth.Commands;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.LogInDto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.LogInDto.Password).NotEmpty();
    }
}
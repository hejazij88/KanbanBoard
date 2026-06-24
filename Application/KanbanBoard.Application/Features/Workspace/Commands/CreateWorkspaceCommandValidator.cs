using FluentValidation;

namespace KanbanBoard.Application.Features.Workspace.Commands;

public class CreateWorkspaceCommandValidator: AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(a => a.WorkspaceDto.Name).NotEmpty().MaximumLength(100);
    }
}
using FluentValidation;

namespace KanbanBoard.Application.Features.Board.Commands;

public class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardCommandValidator()
    {
        RuleFor(b => b.BoardDto.Title).NotEmpty().MaximumLength(30);
    }
}
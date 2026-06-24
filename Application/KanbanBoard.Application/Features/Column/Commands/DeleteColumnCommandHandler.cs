using MediatR;

namespace KanbanBoard.Application.Features.Column.Commands;

public class DeleteColumnCommandHandler:IRequestHandler<DeleteColumnCommand,bool>
{
    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
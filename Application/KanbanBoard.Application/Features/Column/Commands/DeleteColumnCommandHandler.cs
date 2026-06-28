using KanbanBoard.Application.Features.Workspace.Commands;
using KanbanBoard.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace KanbanBoard.Application.Features.Column.Commands;

public class DeleteColumnCommandHandler:IRequestHandler<DeleteColumnCommand,bool>
{
    private readonly IColumnRepository _columnRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteColumnCommandHandler(IColumnRepository columnRepository, IHttpContextAccessor httpContextAccessor)
    {
        _columnRepository = columnRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteColumnCommand request, CancellationToken cancellationToken)
    {
        var column = await _columnRepository.GetByIdAsync(request.ColumnId);
        if (column == null)
            throw new Exception("column not found.");

        var userId = GetCurrentUserId();
        if (column.Board.Workspace.OwnerId != userId)
            throw new UnauthorizedAccessException("Only the owner can delete the column.");

        _columnRepository.Delete(column);
        await _columnRepository.SaveChangesAsync();
        return true;
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}
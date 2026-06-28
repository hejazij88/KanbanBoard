using AutoMapper;
using KanbanBoard.Application.DTOs.Column;
using KanbanBoard.Application.DTOs.Workspace;
using KanbanBoard.Application.Features.Workspace.Commands;
using KanbanBoard.Application.Interfaces.Repositories;
using KanbanBoard.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using KanbanBoard.Application.Features.Board.Commands;

namespace KanbanBoard.Application.Features.Column.Commands;

public class UpdateColumnCommnadHandler:IRequestHandler<UpdateColumnCommnad,ColumnDto>
{
    private readonly IColumnRepository _columnRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdateColumnCommnadHandler(IColumnRepository columnRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _columnRepository = columnRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ColumnDto> Handle(UpdateColumnCommnad request, CancellationToken cancellationToken)
    {
        var column = await _columnRepository.GetByIdAsync(request.ColumnId);
        if (column == null)
            throw new Exception("Column not found.");

        // بررسی دسترسی (فقط Owner یا Admin)
        var userId = GetCurrentUserId();
        if (column.Board.Workspace.OwnerId != userId && !column.Board.Workspace.Members.Any(m => m.UserId == userId && m.Role == WorkspaceRole.Admin))
            throw new UnauthorizedAccessException("You don't have permission to update this Column.");

        _mapper.Map(request.UpdateColumnDto, column);
        _columnRepository.Update(column);
        await _columnRepository.SaveChangesAsync();

        return _mapper.Map<ColumnDto>(column);
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User not authenticated.");
        return Guid.Parse(userIdClaim);
    }
}
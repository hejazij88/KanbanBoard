using AutoMapper;
using KanbanBoard.Application.DTOs.Attachment;
using KanbanBoard.Application.DTOs.Auth;
using KanbanBoard.Application.DTOs.Board;
using KanbanBoard.Application.DTOs.Column;
using KanbanBoard.Application.DTOs.Comment;
using KanbanBoard.Application.DTOs.Task;
using KanbanBoard.Application.DTOs.Workspace;
using System.Data.Common;
using System.Net.Mail;
using System.Xml.Linq;
using KanbanBoard.Domain.Entities;
using Attachment = KanbanBoard.Domain.Entities.Attachment;

namespace KanbanBoard.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Auth
        CreateMap<RegisterDto, User>()
            .ConstructUsing(src => new User(src.UserName, src.Email, string.Empty))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        // Workspace
        CreateMap<CreateWorkspaceDto, Workspace>()
            .ConstructUsing(src => new Workspace(src.Name, Guid.Empty))
            .ForMember(dest => dest.OwnerId, opt => opt.Ignore());

        CreateMap<UpdateWorkspaceDto, Workspace>();
        CreateMap<Workspace, WorkspaceDto>()
            .ForMember(dest => dest.OwnerUsername, opt => opt.MapFrom(src => src.Owner.Username))
            .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.BoardCount, opt => opt.MapFrom(src => src.Boards.Count));

        CreateMap<WorkspaceMember, WorkspaceMemberDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        // Board
        CreateMap<CreateBoardDto, Board>()
            .ConstructUsing(src => new Board(src.Title, src.WorkspaceId, src.Description));

        CreateMap<UpdateBoardDto, Board>();
        CreateMap<Board, BoardDto>()
            .ForMember(dest => dest.WorkspaceName, opt => opt.MapFrom(src => src.Workspace.Name))
            .ForMember(dest => dest.ColumnCount, opt => opt.MapFrom(src => src.Columns.Count))
            .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Columns.Sum(c => c.TaskItems.Count)));

        // Column
        CreateMap<CreateColumnDto, BoardColumn>()
            .ConstructUsing(src => new BoardColumn(src.Title, Guid.Empty, src.Order ?? 0))
            .ForMember(dest => dest.BoardId, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order ?? 0));

        CreateMap<UpdateColumnDto, BoardColumn>();
        CreateMap<BoardColumn, ColumnDto>()
            .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.TaskItems.Count));

        // Task
        CreateMap<CreateTaskDto, TaskItem>()
            .ConstructUsing(src => new TaskItem(src.Title, src.Description ?? string.Empty,
                src.Priority, Guid.Empty, 0))
            .ForMember(dest => dest.ColumnId, opt => opt.Ignore())
            .ForMember(dest => dest.Order, opt => opt.Ignore())
            .ForMember(dest => dest.DueDate, opt => opt.MapFrom(src => src.DueDate))
            .ForMember(dest => dest.AssignedUserId, opt => opt.MapFrom(src => src.AssignedUserId));

        CreateMap<UpdateTaskDto, TaskItem>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.ColumnTitle, opt => opt.MapFrom(src => src.Column.Title))
            .ForMember(dest => dest.AssignedUsername, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.Username : null));

        // Comment
        CreateMap<CreateCommentDto, Comment>()
            .ConstructUsing(src => new Comment(src.Content, Guid.Empty, Guid.Empty))
            .ForMember(dest => dest.TaskId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore());

        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));

        // Attachment
        CreateMap<Attachment, AttachmentDto>();
    }
}
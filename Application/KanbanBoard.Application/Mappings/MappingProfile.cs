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

namespace KanbanBoard.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, User>()
    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // هش در Handler انجام می‌شود

        // Workspace
        CreateMap<CreateWorkspaceDto, Workspace>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateWorkspaceDto, Workspace>();
        CreateMap<Workspace, WorkspaceDto>()
            .ForMember(dest => dest.OwnerUsername, opt => opt.MapFrom(src => src.Owner.FullName))
            .ForMember(dest => dest.MemberCount, opt => opt.MapFrom(src => src.Members.Count))
            .ForMember(dest => dest.BoardCount, opt => opt.MapFrom(src => src.Boards.Count));

        CreateMap<WorkspaceMember, WorkspaceMemberDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

        // Board
        CreateMap<CreateBoardDto, Board>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateBoardDto, Board>();
        CreateMap<Board, BoardDto>()
            .ForMember(dest => dest.WorkspaceName, opt => opt.MapFrom(src => src.Workspace.Name))
            .ForMember(dest => dest.ColumnCount, opt => opt.MapFrom(src => src.Columns.Count))
            .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Columns.Sum(c => c.Tasks.Count)));

        // Column
        CreateMap<CreateColumnDto, BoardColumn>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<UpdateColumnDto, BoardColumn>();
        CreateMap<BoardColumn, ColumnDto>()
            .ForMember(dest => dest.TaskCount, opt => opt.MapFrom(src => src.Tasks.Count));

        // Task
        CreateMap<CreateTaskDto, TaskItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => 0)) // در Handler مقداردهی می‌شود
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<UpdateTaskDto, TaskItem>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.ColumnTitle, opt => opt.MapFrom(src => src.Column.Name))
            .ForMember(dest => dest.AssignedUsername, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.Email : null))
            .ForMember(dest => dest.AssignedUserEmail, opt => opt.MapFrom(src => src.AssignedUser != null ? src.AssignedUser.Email : null));

        // Comment
        CreateMap<CreateCommentDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Email));

        // Attachment
        CreateMap<Attachment, AttachmentDto>();
    }
}
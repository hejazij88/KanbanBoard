using AngleSharp.Css;
using KanbanBoard.Domain.Entities;
using Attachment = System.Net.Mail.Attachment;

namespace KanbanBoard.Tests.Unit.Helper;

public static class TestDataFactory
{
    public static User CreateUser(string username = "testuser", string email = "test@email.com")
    {
        return new User(username, email, "hashedpassword");
    }

    public static Workspace CreateWorkspace(string name = "Test Workspace", User? owner = null)
    {
        owner ??= CreateUser();
        return new Workspace(name, owner.Id);
    }

    public static Board CreateBoard(string title = "Test Board", Workspace? workspace = null)
    {
        workspace ??= CreateWorkspace();
        return new Board(title, workspace.Id);
    }

    public static BoardColumn CreateColumn(string title = "Todo", Board? board = null, int order = 1)
    {
        board ??= CreateBoard();
        return new BoardColumn(title, board.Id, order);
    }

    public static TaskItem CreateTask(string title = "Test Task", BoardColumn? column = null)
    {
        column ??= CreateColumn();
        return new TaskItem(title, "Description", KanbanBoard.Domain.Enums.Priority.Medium, column.Id, 1);
    }

    public static Comment CreateComment(TaskItem? task = null, User? user = null)
    {
        task ??= CreateTask();
        user ??= CreateUser();
        return new Comment("Test comment", task.Id, user.Id);
    }

    public static KanbanBoard.Domain.Entities.Attachment CreateAttachment(TaskItem? task = null)
    {
        task ??= CreateTask();
        return new KanbanBoard.Domain.Entities.Attachment("file.pdf", "/uploads/file.pdf", 1024, "application/pdf", task.Id);
    }
}
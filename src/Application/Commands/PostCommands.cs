namespace Application.Commands;

public record CreatePostCommand(string Title, string Content);

public record UpdatePostCommand(Guid Id, string Title, string Content);

public record DeletePostCommand(Guid Id);

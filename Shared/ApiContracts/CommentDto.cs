namespace ApiContracts;

public record CommentCreateDto(int PostId, int UserId, string Body);

public record CommentUpdateDto(int Id, string Body);

public record CommentDto(int Id, int PostId, int UserId, string Body);
namespace ApiContracts;

public record PostCreateDto(string Title, string Body, int UserId);

public record PostUpdateDto(int Id, string Title, string Body);

public record PostDto(int Id, string Title, string Body, int UserId, IEnumerable<CommentDto> Comments);
using ApiContracts;
namespace BlazorApp.Services;

public interface ICommentService
{
    Task<CommentDto> AddCommentAsync(CommentCreateDto request);
    Task UpdateCommentAsync(int id, CommentUpdateDto request);
    Task<CommentDto> GetSingleAsync(int id);
    Task<IEnumerable<CommentDto>> GetAllAsync();
    Task DeleteCommentAsync(int id);
    
}
using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(PostCreateDto request);
    Task UpdatePostAsync(int id, PostUpdateDto request);
    Task<PostDto> GetSingleAsync(int id);
    Task<IEnumerable<PostDto>> GetAllAsync();
    Task DeletePostAsync(int id);
    
}
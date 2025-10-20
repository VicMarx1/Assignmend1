using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

[ApiController]
[Route("api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public PostsController(IPostRepository posts, ICommentRepository comments)
    {
        postRepository = posts;
        commentRepository = comments;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Create(PostCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest("Title required");
        var post = await postRepository.AddAsync(new Post
        {
            Title = dto.Title,
            Body = dto.Body,
            UserId = dto.UserId
        });

        var result = new PostDto(post.Id, post.Title, post.Body, post.UserId, null);
        return CreatedAtAction(nameof(GetSingle), new { id = post.Id }, result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle(int id, [FromQuery] bool includeComments = false)
    {
        try
        {
            var post = await postRepository.GetSingleAsync(id);
            var dto = new PostDto(post.Id, post.Title, post.Body, post.UserId, null);

            if (!includeComments) return dto;

            var comments = commentRepository.GetAll()
                .Where(c => new CommentDto(c.Id, c.PostId, c.UserId, c.Body) != null)
                .ToList();

            return Ok(new
            {
                Post = dto,
                Comments = comments
            });
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
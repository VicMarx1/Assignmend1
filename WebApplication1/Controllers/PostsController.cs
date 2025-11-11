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
    private readonly IUserRepository userRepository;

    public PostsController(IPostRepository posts, ICommentRepository comments, IUserRepository username)
    {
        postRepository = posts;
        commentRepository = comments;
        userRepository = username;
    }

    [HttpPost]
    public async Task<ActionResult<PostDto>> Create([FromBody] PostCreateDto dto)
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

            if (!includeComments) return Ok(dto);

            var comments = commentRepository.GetAll()
                .Where(c => c.PostId == id)
                .Select(c => new CommentDto(c.Id, c.PostId, c.UserId, c.Body))
                .ToList();

            return Ok(new PostDto(post.Id, post.Title, post.Body, post.UserId, comments));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetAll(
        [FromQuery] string? title = null,
        [FromQuery] int? authorId = null,
        [FromQuery] string? authorName = null,
        [FromQuery] bool includeComments = false)
    {
        var postsQuery = postRepository.GetAll().AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
            postsQuery = postsQuery.Where(p => p.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

        if (authorId is not null)
            postsQuery = postsQuery.Where(p => p.UserId == authorId);

        if (!string.IsNullOrWhiteSpace(authorName) && userRepository is not null)
        {
            var ids = userRepository.GetAll()
                .Where(u => u.Username.Contains(authorName, StringComparison.OrdinalIgnoreCase))
                .Select(u => u.Id)
                .ToList();
            postsQuery = postsQuery.Where(p => ids.Contains(p.UserId));
        }

        var list = postsQuery
            .Select(p => new PostDto(p.Id, p.Title, p.Body, p.UserId, null))
            .ToList();

        if (!includeComments)
        {
            var listNoComments = postsQuery
                .Select(p => new PostDto(p.Id, p.Title, p.Body, p.UserId, Array.Empty<CommentDto>()))
                .ToList();
            return Ok(listNoComments);
        }

        var allComments = commentRepository.GetAll().ToList();
        var listWithComments = postsQuery
            .Select(p => new PostDto(
                p.Id, p.Title, p.Body, p.UserId,
                allComments.Where(c => c.PostId == p.Id)
                    .Select(c => new CommentDto(c.Id, c.PostId, c.UserId, c.Body))
                    .ToList()
            ))
            .ToList();

        return Ok(listWithComments);
    }
}
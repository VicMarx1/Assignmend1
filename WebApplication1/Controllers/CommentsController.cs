using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository commentRepository;

    public CommentController(ICommentRepository comments)
    {
        commentRepository = comments;
    }

    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(CommentCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Body)) return BadRequest("Body required");
        var created = await commentRepository.AddAsync(new Comment
        {
            PostId = dto.PostId,
            UserId = dto.UserId,
            Body = dto.Body
        });

        var result = new CommentDto(created.Id, created.PostId, created.UserId, created.Body);
        return CreatedAtAction(nameof(GetSingle), new { id = created.Id }, result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        try
        {
            var c = await commentRepository.GetSingleAsync(id);
            var dto = new CommentDto(c.Id, c.PostId, c.UserId, c.Body);
            return Ok(dto);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CommentUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch");
        try
        {
            var existing = await commentRepository.GetSingleAsync(id);
            existing.Body = dto.Body;
            await commentRepository.UpdateAsync(existing);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await commentRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}
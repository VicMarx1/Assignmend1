using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using Entities;
using RepositoryContracts;

[ApiController]
[Route("api/[controller]")]

public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(UserCreateDto dto)
    {
        if ( String.IsNullOrWhiteSpace(dto.UserName)) return BadRequest("UserName required");
        
        var exists = userRepository.GetAll().Any(u => u.Username == dto.UserName);
        if (exists) return Conflict("UserName already exists");
        
        var user = await userRepository.AddAsync(new User{Username = dto.UserName});
        return CreatedAtAction(nameof(GetSingle), new {id = user.Id}, new UserDto(user.Id, user.Username));

    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        try
        {
            var user = await userRepository.GetSingleAsync(id);
            return Ok(new UserDto(user.Id, user.Username));
        }
        catch (InvalidOperationException )
        {
            return NotFound();
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetAll([FromQuery] string? name, [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = userRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(u => u.Username.Contains(name));
        
        var total = query.Count();
        var items = query
            .OrderBy(u => u.Id)
            .Skip((page -1)*pageSize)
            .Take(pageSize)
            .Select(u => new UserDto(u.Id, u.Username))
            .ToList();
        
        Response.Headers.Add("X-Total-Count", total.ToString());
        return Ok(items);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch");

        try
        {
            await userRepository.UpdateAsync(new User { Id = dto.Id, Username = dto.UserName });
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
            await userRepository.DeleteAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}    
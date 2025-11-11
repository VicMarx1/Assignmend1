using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication1.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository users;

    public AuthController(IUserRepository users)
    {
        this.users = users;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.UserName) || string.IsNullOrWhiteSpace(req.Password))
            return Unauthorized("Missing username or password.");

        var all = users.GetAll();
        var user = all.FirstOrDefault(u =>
            string.Equals(u.Username, req.UserName, StringComparison.OrdinalIgnoreCase)
            && u.Password == req.Password
        );

        if (user is null) return Unauthorized("Invalid username or password.");

        var dto = new UserDto(user.Id, user.Password);
        return Ok(dto);
    }

    public sealed record LoginRequest(string UserName, string Password);
}
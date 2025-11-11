using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using ApiContracts;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Auth;

public class SimpleAuthProvider : AuthenticationStateProvider
{
    private readonly HttpClient http;
    private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public SimpleAuthProvider(HttpClient http)
    {
        this.http = http;
    }

    public async Task Login(string userName, string password)
    {
        var response = await http.PostAsJsonAsync("auth/login", new LoginRequest(userName, password));
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode) throw new Exception(content);

        var userDto = JsonSerializer.Deserialize<UserDto>(content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, userDto.UserName),
            new Claim("UserId", userDto.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, "SimpleAuth");
        currentUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(new AuthenticationState(currentUser));

    // ← returner Task, så du kan 'await'e i UI
    public Task Logout()
    {
        currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(currentUser)));
        return Task.CompletedTask;
    }

    public record LoginRequest(string UserName, string Password);
}
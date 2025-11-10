using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService

{
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient client;
    private IUserService userServiceImplementation;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(UserCreateDto request)
    {
        var resp = await client.PostAsJsonAsync("api/Users", request);
        var body = await resp.Content.ReadAsStringAsync();
        if (!resp.IsSuccessStatusCode) throw new Exception(body);
        return JsonSerializer.Deserialize<UserDto>(body, JsonOpts)!;
    }

    public async Task UpdateUserAsync(int id, UserUpdateDto request)
    {
        var resp = await client.PutAsJsonAsync($"api/Users/{id}", request);
        if (!resp.IsSuccessStatusCode)
            throw new Exception(await resp.Content.ReadAsStringAsync());
    }

    public async Task<UserDto> GetSingleAsync(int id)
    {
        return await client.GetFromJsonAsync<UserDto>($"api/Users/{id}")!;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(string? userNameContains = null)
    {
        var url = "api/Users";
        if (!string.IsNullOrWhiteSpace(userNameContains))
            url += $"?userNameContains={Uri.EscapeDataString(userNameContains)}";

        var data = await client.GetFromJsonAsync<IEnumerable<UserDto>>(url);
        return data ?? Enumerable.Empty<UserDto>();
    }

    public async Task DeleteUserAsync(int id)
    {
        var resp = await client.DeleteAsync($"api/Users/{id}");
        if (!resp.IsSuccessStatusCode)
            throw new Exception(await resp.Content.ReadAsStringAsync());
    }


    public async Task<IEnumerable<UserDto>> GetManyAsync(string? userNameContains = null)
    {
        var url = "api/Users";
        if (!string.IsNullOrWhiteSpace(userNameContains))
            url += $"?userNameContains={Uri.EscapeDataString(userNameContains)}";

        var data = await client.GetFromJsonAsync<IEnumerable<UserDto>>(url);
        return data ?? Enumerable.Empty<UserDto>();
    }
}
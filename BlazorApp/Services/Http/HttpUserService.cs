using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpUserService : IUserService
{
    private readonly HttpClient client;

    public HttpUserService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<UserDto> AddUserAsync(UserCreateDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("users", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<UserDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateUserAsync(int id, UserUpdateDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"users/{id}", request);
        if (!httpResponse.IsSuccessStatusCode)
        {
            string msg = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }

    public async Task<UserDto> GetSingleAsync(int id)
    {
        var result = await client.GetFromJsonAsync<UserDto>($"Users/{id}");
        return result;
    }

    public Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var result = client.GetFromJsonAsync<IEnumerable<UserDto>>("users");
        return result!;
    }

    public async Task DeleteUserAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"Users/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string msg = await response.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }
}

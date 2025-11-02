using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<PostDto> AddPostAsync(PostCreateDto request)
    {
        var httpResponse = await client.PostAsJsonAsync("api/Posts", request);
        var response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdatePostAsync(int id, PostUpdateDto request)
    {
        var httpResponse = await client.PutAsJsonAsync($"api/Posts/{id}", request);
        if (!httpResponse.IsSuccessStatusCode)
        {
            var msg = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }

    public async Task<PostDto> GetSingleAsync(int id)
    {
        var result = await client.GetFromJsonAsync<PostDto>($"api/Posts/{id}");
        return result;
    }

    public Task<IEnumerable<PostDto>> GetAllAsync()
    {
        var result = client.GetFromJsonAsync<IEnumerable<PostDto>>("api/Posts");
        return result!;
    }

    public async Task DeletePostAsync(int id)
    {
        var response = await client.DeleteAsync($"api/Posts/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var msg = await response.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }
}
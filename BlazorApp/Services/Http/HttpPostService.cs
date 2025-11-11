using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

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

        return JsonSerializer.Deserialize<PostDto>(response, JsonOpts)!;
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

    public async Task<PostDto> GetSingleAsync(int id, bool includeComments)
    {
        var url = $"api/Posts/{id}?includeComments={(includeComments ? "true" : "false")}";
        var response = await client.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(json);

        return JsonSerializer.Deserialize<PostDto>(json, JsonOpts)!;
    }

    public async Task<IEnumerable<PostDto>> GetAllAsync()
    {
        return await client.GetFromJsonAsync<IEnumerable<PostDto>>("api/Posts", JsonOpts)
               ?? Enumerable.Empty<PostDto>();
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
using System.Net.Http.Json;
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
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("Post", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<PostDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdatePostAsync(int id, PostUpdateDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"Post/{id}", request);
        if (!httpResponse.IsSuccessStatusCode)
        {
            string msg = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }

    public async Task<PostDto> GetSingleAsync(int id)
    {
        var result = await client.GetFromJsonAsync<PostDto>($"Post/{id}");
        return result;
    }

    public Task<IEnumerable<PostDto>> GetAllAsync()
    {
        var result = client.GetFromJsonAsync<IEnumerable<PostDto>>("Post");
        return result!;
    }

    public async Task DeletePostAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"Post/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string msg = await response.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }
}
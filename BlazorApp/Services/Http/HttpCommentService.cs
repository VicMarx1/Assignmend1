
using System.Net.Http.Json;
using System.Text.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<CommentDto> AddCommentAsync(CommentCreateDto request)
    {
        HttpResponseMessage httpResponse = await client.PostAsJsonAsync("Comment", request);
        string response = await httpResponse.Content.ReadAsStringAsync();

        if (!httpResponse.IsSuccessStatusCode)
            throw new Exception(response);

        return JsonSerializer.Deserialize<CommentDto>(response, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task UpdateCommentAsync(int id, CommentUpdateDto request)
    {
        HttpResponseMessage httpResponse = await client.PutAsJsonAsync($"Comment/{id}", request);
        if (!httpResponse.IsSuccessStatusCode)
        {
            string msg = await httpResponse.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }

    public async Task<CommentDto> GetSingleAsync(int id)
    {
        var result = await client.GetFromJsonAsync<CommentDto>($"Comment/{id}");
        return result;
    }

    public Task<IEnumerable<CommentDto>> GetAllAsync()
    {
        var result = client.GetFromJsonAsync<IEnumerable<CommentDto>>("Comment");
        return result!;
    }

    public async Task DeleteCommentAsync(int id)
    {
        HttpResponseMessage response = await client.DeleteAsync($"Comment/{id}");
        if (!response.IsSuccessStatusCode)
        {
            string msg = await response.Content.ReadAsStringAsync();
            throw new Exception(msg);
        }
    }
}
    

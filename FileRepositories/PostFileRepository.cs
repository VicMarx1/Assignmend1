using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new();

        int nextId = posts.Any() ? posts.Max(c => c.Id) + 1 : 1;
        post.Id = nextId;
        
        posts.Add(post);

        string updatedJson = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, updatedJson);

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new();
        
        var existing = posts.SingleOrDefault(p => p.Id == post.Id)
            ?? throw new InvalidOperationException($"Post with id: {post.Id} not found");
        posts.Remove(existing);
        posts.Add(post);

        string updatedJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task DeleteAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new();
        
        var toRemove = posts.SingleOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException($"Post with id: {id} not found");
        posts.Remove(toRemove);

        string updatedJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new();
        
        var post = posts.SingleOrDefault(p => p.Id == id)
            ?? throw new InvalidOperationException($"Post with id: {id} not found");
        return post;
    }
    
    public IQueryable<Post> GetAll()
    {
        string json = File.ReadAllText(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new();
        return posts.AsQueryable();
    }
}
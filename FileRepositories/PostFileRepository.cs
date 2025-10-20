using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "[]");
    }

    public async Task<Post> AddAsync(Post post)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        var nextId = posts.Any() ? posts.Max(c => c.Id) + 1 : 1;
        post.Id = nextId;

        posts.Add(post);

        var updatedJson = JsonSerializer.Serialize(posts);

        await File.WriteAllTextAsync(filePath, updatedJson);

        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        var existing = posts.SingleOrDefault(p => p.Id == post.Id)
                       ?? throw new InvalidOperationException($"Post with id: {post.Id} not found");
        posts.Remove(existing);
        posts.Add(post);

        var updatedJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task DeleteAsync(int id)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        var toRemove = posts.SingleOrDefault(p => p.Id == id)
                       ?? throw new InvalidOperationException($"Post with id: {id} not found");
        posts.Remove(toRemove);

        var updatedJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();

        var post = posts.SingleOrDefault(p => p.Id == id)
                   ?? throw new InvalidOperationException($"Post with id: {id} not found");
        return post;
    }

    public IQueryable<Post> GetAll()
    {
        var json = File.ReadAllText(filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(json) ?? new List<Post>();
        return posts.AsQueryable();
    }
}
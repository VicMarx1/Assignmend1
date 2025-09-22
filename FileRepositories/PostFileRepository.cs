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
        var postsAsJson = await File.ReadAllTextAsync(filePath);
        var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;

        var maxId = posts.Count > 0 ? posts.Max(p => p.Id) : 0;
        post.Id = maxId + 1;

        posts.Add(post);

        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);

        return post;
    }

    public IQueryable<Post> GetAll()
    {
        var postsAsJson = File.ReadAllTextAsync(filePath).Result;
        var posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson)!;
        return posts.AsQueryable();
    }

    public async Task<Post> GetSingleAsync(int id)
    {
        return GetAll().FirstOrDefault(p => p.Id == id)!;
    }

    public async Task UpdateAsync(Post post)
    {
        var posts = GetAll().ToList();
        var existingPost = posts.FirstOrDefault(p => p.Id == post.Id);
        if (existingPost == null)
            throw new InvalidOperationException($"Post with id: {post.Id} not found");

        posts.Remove(existingPost);
        posts.Add(post);

        var json = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task DeleteAsync(int id)
    {
        var posts = GetAll().ToList();
        var postToRemove = posts.FirstOrDefault(p => p.Id == id);
        if (postToRemove == null)
            throw new InvalidOperationException($"Post with Id: {id} not found");

        posts.Remove(postToRemove);

        var json = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, json);
    }
}
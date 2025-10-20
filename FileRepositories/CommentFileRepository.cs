using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "[]");
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        var nextId = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
        comment.Id = nextId;

        comments.Add(comment);

        var updatedJson = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, updatedJson);

        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        var existing = comments.SingleOrDefault(p => p.Id == comment.Id)
                       ?? throw new InvalidOperationException($"Comment with id: {comment.Id} not found");
        comments.Remove(existing);
        comments.Add(comment);

        var updatedJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task DeleteAsync(int id)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        var toRemove = comments.SingleOrDefault(p => p.Id == id)
                       ?? throw new InvalidOperationException($"Comment with id: {id} not found");
        comments.Remove(toRemove);

        var updatedJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();

        var comment = comments.SingleOrDefault(p => p.Id == id)
                      ?? throw new InvalidOperationException($"Comment with id: {id} not found");
        return comment;
    }

    public IQueryable<Comment> GetAll()
    {
        var json = File.ReadAllText(filePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new List<Comment>();
        return comments.AsQueryable();
    }
}
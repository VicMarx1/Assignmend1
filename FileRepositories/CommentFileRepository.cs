using System.Text.Json;
using Entities;
using RepositoryContracts;
namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new();

        int nextId = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
        comment.Id = nextId;
        
        comments.Add(comment);

        string updatedJson = JsonSerializer.Serialize(comments);

        await File.WriteAllTextAsync(filePath, updatedJson);

        return comment;
    }
    
    public async Task UpdateAsync(Comment comment)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new();
        
        var existing = comments.SingleOrDefault(p => p.Id == comment.Id)
                       ?? throw new InvalidOperationException($"Comment with id: {comment.Id} not found");
        comments.Remove(existing);
        comments.Add(comment);

        string updatedJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task DeleteAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new();
        
        var toRemove = comments.SingleOrDefault(p => p.Id == id)
                       ?? throw new InvalidOperationException($"Comment with id: {id} not found");
        comments.Remove(toRemove);

        string updatedJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new();
        
        var comment = comments.SingleOrDefault(p => p.Id == id)
                   ?? throw new InvalidOperationException($"Comment with id: {id} not found");
        return comment;
    }
    
    public IQueryable<Comment> GetAll()
    {
        string json = File.ReadAllText(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(json) ?? new();
        return comments.AsQueryable();
    }

}
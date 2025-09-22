using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string filePath = "comments.Json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "[]");
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        var commentAsJson = await File.ReadAllTextAsync(filePath);
        var comments = JsonSerializer.Deserialize<List<Comment>>(commentAsJson);

        var maxID = comments.Count > 0 ? comments.Max(c => c.Id) : 0;
        comment.Id = maxID + 1;

        comments.Add(comment);

        commentAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentAsJson);

        return comment;
    }

    public IQueryable<Comment> GetAll()
    {
        var commentsAsJson = File.ReadAllTextAsync(filePath).Result;
        var comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        return comments.AsQueryable();
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        return GetAll().FirstOrDefault(c => c.Id == id)!;
    }

    public async Task UpdateAsync(Comment comment)
    {
        var comments = GetAll().ToList();
        var existingComment = comments.FirstOrDefault(c => c.Id == comment.Id);
        if (existingComment == null)
            throw new InvalidOperationException($"Comment with id: {comment.Id} not found");

        comments.Remove(existingComment);
        comments.Add(comment);

        var json = JsonSerializer.Serialize(comment);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task DeleteAsync(int id)
    {
        var comments = GetAll().ToList();
        var commentToRemove = comments.FirstOrDefault(c => c.Id == id);
        if (commentToRemove == null)
            throw new InvalidOperationException($"Comment with id: {id} not found");

        comments.Remove(commentToRemove);

        var json = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, json);
    }
}
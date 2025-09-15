using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    public List<Comment> comments;

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.id = comments.Any()
            ? comments.Max(c => c.id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        var existingComment = comments.SingleOrDefault(c => c.id == comment.id);
        if (existingComment is null) throw new InvalidOperationException($"Comment with id: {comment.id} not found");

        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var commentToRemove = comments.SingleOrDefault(c => c.id == id);
        if (commentToRemove is null) throw new InvalidOperationException($"Comment with id: {id} not found");
        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }


    public Task<Comment> GetSingleAsync(int id)
    {
        var comment = comments.SingleOrDefault(c => c.id == id);
        if (comment is null) throw new InvalidOperationException($"Comment with id: {id} not found");

        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetAll()
    {
        return comments.AsQueryable();
    }
}
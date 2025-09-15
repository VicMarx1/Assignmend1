using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    public List<Comment> comments;

    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(c => c.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        var existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment is null) throw new InvalidOperationException($"Comment with id: {comment.Id} not found");

        comments.Remove(existingComment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove is null) throw new InvalidOperationException($"Comment with id: {id} not found");
        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }


    public Task<Comment> GetSingleAsync(int id)
    {
        var comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment is null) throw new InvalidOperationException($"Comment with id: {id} not found");

        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetAll()
    {
        return comments.AsQueryable();
    }
}
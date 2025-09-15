using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    public List<Post> posts;

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        var existingPost = posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost is null) throw new InvalidOperationException($"Post with id: {post.Id} not found");

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var postToRemove = posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove is null) throw new InvalidOperationException($"Post with id: {id} not found");
        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        var post = posts.SingleOrDefault(p => p.Id == id);
        if (post == null) throw new InvalidOperationException($"Post with id: {id} not found");
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetAll()
    {
        return posts.AsQueryable();
    }
}
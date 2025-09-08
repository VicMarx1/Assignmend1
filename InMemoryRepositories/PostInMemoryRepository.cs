using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    public List<Post> posts;

    public Task<Post> AddAsync(Post post)
    {
        post.id = posts.Any()
            ? posts.Max(p => p.id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p => p.id == post.id);
        if (existingPost is null)
        {
            throw new InvalidOperationException($"Post with id: {post.id} not found");
        }

        posts.Remove(existingPost);
        posts.Add(post);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.id == id);
        if (postToRemove is null)
        {
            throw new InvalidOperationException($"Post with id: {id} not found");
        }
        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        Post? post = posts.SingleOrDefault(p => p.id == id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post with id: {id} not found");
        }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetAll()
    {
        return posts.AsQueryable();
    }
}
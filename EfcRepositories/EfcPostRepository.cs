using System.Globalization;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;
using RepositoryContracts.Exceptions;

namespace EfcRepositories;

public class EfcPostRepository : IPostRepository
{
    private readonly AppContext ctx;

    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddAsync(Post post)
    {
        EntityEntry<Post> entityEntry = await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public IQueryable<Post> GetAll()
    {
        return ctx.Posts
            .Include(p => p.User)
            .Include(p => p.Comments);
    }
    public async Task<Post?> GetSingleAsync(int id)
    {
        return await ctx.Posts
            .Include(p => p.Comments)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateAsync(Post post)
    {
        if (!(await ctx.Posts.AnyAsync(p => p.Id == post.Id)))
        {
            throw new NotFoundException($"Post with id {post.Id} not found");
        }

        ctx.Posts.Update(post);
        await ctx.SaveChangesAsync();
    }

    public Task DeleteAsync(int id)
    {
        Post? existing = ctx.Posts.SingleOrDefault(p => p.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"Post with id: {id} not found");
        }

        ctx.Posts.Remove(existing);
        return ctx.SaveChangesAsync();
    }
}
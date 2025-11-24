using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using RepositoryContracts;
using RepositoryContracts.Exceptions;

namespace EfcRepositories;

public class EfcCommentRepository : ICommentRepository
{
    private readonly AppContext ctx;
    
    public EfcCommentRepository (AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        EntityEntry<Comment> entry = await ctx.Comments.AddAsync(comment);
        await ctx.SaveChangesAsync();
        return entry.Entity;
    }
    
    public IQueryable<Comment> GetAll()
    {
        return ctx.Comments
            .Include(c => c.User)
            .Include(c => c.Post);
    }
    
    public async Task<Comment?> GetSingleAsync(int id)
    {
        return await ctx.Comments
            .Include(c => c.User)
            .Include(c => c.Post)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Comment comment)
    {
        if (!(await ctx.Comments.AnyAsync(p => p.Id == comment.Id)))
        {
            throw new NotFoundException($"Post with id {comment.Id} not found");
        }

        ctx.Comments.Update(comment);
        await ctx.SaveChangesAsync();
    }
    
    public Task DeleteAsync(int id)
    {
        Comment? existing = ctx.Comments.SingleOrDefault(p => p.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"Comment with id: {id} not found");
        }

        ctx.Comments.Remove(existing);
        return ctx.SaveChangesAsync();
    }
}
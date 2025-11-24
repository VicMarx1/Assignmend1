using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using RepositoryContracts;
using RepositoryContracts.Exceptions;

namespace EfcRepositories;

public class EfcUserRepository : IUserRepository
{
    private readonly AppContext ctx;
    
    public EfcUserRepository (AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<User> AddAsync(User user)
    {
        EntityEntry<User> entry = await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return entry.Entity;
    }
    
    public IQueryable<User> GetAll()
    {
        return ctx.Users
            .Include(u => u.Posts)
            .Include(u => u.Comments);
    }
    
    public async Task<User?> GetSingleAsync(int id)
    {
        return await ctx.Users
            .Include(u => u.Posts)
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
        if (!(await ctx.Users.AnyAsync(p => p.Id == user.Id)))
        {
            throw new NotFoundException($"Post with id {user.Id} not found");
        }

        ctx.Users.Update(user);
        await ctx.SaveChangesAsync();
    }
    
    public Task DeleteAsync(int id)
    {
        User? existing = ctx.Users.SingleOrDefault(u => u.Id == id);
        if (existing == null)
        {
            throw new NotFoundException($"User with id: {id} not found");
        }

        ctx.Users.Remove(existing);
        return ctx.SaveChangesAsync();
    }
}

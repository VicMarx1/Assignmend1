using Entities;
using RepositoryContracts;
namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    public List<User> users;
    
    public Task<User> AddAsync(User user)
    {
        user.id = users.Any()
            ? users.Max(u => u.id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        User? existingUser = users.SingleOrDefault(u => u.id == user.id);
        if (existingUser is null)
        {
            throw new InvalidOperationException($"User with id: {user.id} not found");
        }
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        User? userToRemove = users.SingleOrDefault(u => u.id == id);
        if (userToRemove is null)
        {
            throw new InvalidOperationException($"User with id: {id} not dound");
        }
        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        User? user = users.SingleOrDefault(u => u.id == id);
        if (user is null)
        {
            throw new InvalidOperationException($"User with id: {id} not found");
        }

        return Task.FromResult(user);
    }

    public IQueryable<User> GetAll()
    {
        return users.AsQueryable();
    }

}
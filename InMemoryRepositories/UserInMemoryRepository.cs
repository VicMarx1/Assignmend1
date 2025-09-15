using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    public List<User> users;

    public Task<User> AddAsync(User user)
    {
        user.Id = users.Any()
            ? users.Max(u => u.Id) + 1
            : 1;
        users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        var existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser is null) throw new InvalidOperationException($"User with id: {user.Id} not found");
        users.Remove(existingUser);
        users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        var userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove is null) throw new InvalidOperationException($"User with id: {id} not dound");
        users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        var user = users.SingleOrDefault(u => u.Id == id);
        if (user is null) throw new InvalidOperationException($"User with id: {id} not found");

        return Task.FromResult(user);
    }

    public IQueryable<User> GetAll()
    {
        return users.AsQueryable();
    }
}
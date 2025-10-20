using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "[]");
    }

    public async Task<User> AddAsync(User user)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        var nextId = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        user.Id = nextId;
        users.Add(user);
        var updatedJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, updatedJson);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        var existing = users.SingleOrDefault(u => u.Id == user.Id)
                       ?? throw new InvalidOperationException($"User with id {user.Id} not found");
        users.Remove(existing);
        users.Add(user);

        var updatedJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task DeleteAsync(int id)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        var toRemove = users.SingleOrDefault(u => u.Id == id)
                       ?? throw new InvalidOperationException($"User with id {id} not found");
        users.Remove(toRemove);

        var updatedJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var json = await File.ReadAllTextAsync(filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();

        var user = users.SingleOrDefault(u => u.Id == id)
                   ?? throw new InvalidOperationException($"User with id {id} not found");
        return user;
    }

    public IQueryable<User> GetAll()
    {
        var json = File.ReadAllText(filePath);
        var users = JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        return users.AsQueryable();
    }
}
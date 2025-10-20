using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? new();
        
        int nextId = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        user.Id = nextId;
        users.Add(user);
        string updatedJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, updatedJson);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? new();
        
        var existing = users.SingleOrDefault(u => u.Id == user.Id)
            ?? throw new InvalidOperationException($"User with id {user.Id} not found");
        users.Remove(existing);
        users.Add(user);

        string updatedJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task DeleteAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? new();
        
        var toRemove = users.SingleOrDefault(u => u.Id == id)
            ?? throw new InvalidOperationException($"User with id {id} not found");
        users.Remove(toRemove);

        string updatedJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, updatedJson);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        string json = await File.ReadAllTextAsync(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? new();
        
        var user = users.SingleOrDefault(u => u.Id == id)
            ?? throw new InvalidOperationException($"User with id {id} not found");
        return user;
    }

    public IQueryable<User> GetAll()
    {
        string json = File.ReadAllText(filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(json) ?? new();
        return users.AsQueryable();
    }

}
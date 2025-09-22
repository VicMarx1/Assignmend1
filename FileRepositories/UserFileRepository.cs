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
        string usersAsJson = await File.ReadAllTextAsync(filePath);
        var users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        int maxId = users.Count > 0 ? users.Max(u => u.Id) : 0;
        user.Id = maxId + 1;
        
        users.Add(user);
        
        usersAsJson = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, usersAsJson);
        
        return user;
    }
    

    public IQueryable<User> GetAll()
    {
        string usersAsJson = File.ReadAllTextAsync(filePath).Result;
        var users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        return users.AsQueryable();
    }

    public async Task<User> GetSingleAsync(int id)
    {
        return GetAll().FirstOrDefault(u => u.Id == id)!;
    }
    public Task UpdateAsync(User user)
    {
        var users = GetAll().ToList();
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser == null)
            throw new InvalidOperationException($"User with id: {user.Id} not found");

        users.Remove(existingUser);
        users.Add(user);

        string json = JsonSerializer.Serialize(users);
        return File.WriteAllTextAsync(filePath, json);
    }

    public async Task DeleteAsync(int id)
    {
        var users = GetAll().ToList();
        var userToRemove = users.FirstOrDefault(u => u.Id == id);
        if (userToRemove == null)
            throw new InvalidOperationException($"User with id: {id} not found");

        users.Remove(userToRemove);

        string json = JsonSerializer.Serialize(users);
        await File.WriteAllTextAsync(filePath, json);   
    }
}
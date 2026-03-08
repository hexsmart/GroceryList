using System.Text.Json;
using GroceryList.Models;

namespace GroceryList.Services;

public class UserService
{
    private readonly string _filePath;

    public UserService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "users.json");
    }

    public List<UserProfile> GetAll()
    {
        if (!File.Exists(_filePath)) return new List<UserProfile>();
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<UserProfile>>(json) ?? new List<UserProfile>();
    }

    public UserProfile? FindByEmail(string email)
    {
        return GetAll().FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    public UserProfile Register(string firstName, string lastName, string email)
    {
        var users = GetAll();
        var user = new UserProfile { FirstName = firstName, LastName = lastName, Email = email };
        users.Add(user);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true }));
        return user;
    }
}

using System.Text.Json;
using GroceryList.Models;

namespace GroceryList.Services;

public class GroceryService
{
    private readonly string _contentRoot;

    public GroceryService(IWebHostEnvironment env)
    {
        _contentRoot = env.ContentRootPath;
    }

    private string FilePath(string userId) =>
        Path.Combine(_contentRoot, $"groceries-{userId}.json");

    public List<GroceryItem> GetAll(string userId)
    {
        var path = FilePath(userId);
        if (!File.Exists(path)) return new List<GroceryItem>();
        var json = File.ReadAllText(path);
        var items = JsonSerializer.Deserialize<List<GroceryItem>>(json) ?? new List<GroceryItem>();
        return items.OrderBy(i => i.Name, StringComparer.OrdinalIgnoreCase).ToList();
    }

    public void Save(string userId, List<GroceryItem> items)
    {
        File.WriteAllText(FilePath(userId), JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true }));
    }

    public void AddItems(string userId, string commaSeparated)
    {
        var items = GetAll(userId);
        var existing = items.Select(i => i.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var names = commaSeparated.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var name in names)
        {
            var capitalized = Capitalize(name);
            if (!existing.Contains(capitalized))
            {
                items.Add(new GroceryItem { Name = capitalized });
                existing.Add(capitalized);
            }
        }
        Save(userId, items);
    }

    private static string Capitalize(string s) =>
        string.IsNullOrWhiteSpace(s) ? s : char.ToUpper(s[0]) + s.Substring(1);

    public void RemoveItem(string userId, Guid id)
    {
        var items = GetAll(userId);
        items.RemoveAll(i => i.Id == id);
        Save(userId, items);
    }

    public void ClearAll(string userId)
    {
        Save(userId, new List<GroceryItem>());
    }
}

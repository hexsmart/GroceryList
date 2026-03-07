using System.Text.Json;
using GroceryList.Models;

namespace GroceryList.Services;

public class GroceryService
{
    private readonly string _filePath;

    public GroceryService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "groceries.json");
    }

    public List<GroceryItem> GetAll()
    {
        if (!File.Exists(_filePath)) return new List<GroceryItem>();
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<GroceryItem>>(json) ?? new List<GroceryItem>();
    }

    public void Save(List<GroceryItem> items)
    {
        File.WriteAllText(_filePath, JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true }));
    }

    public void AddItems(string commaSeparated)
    {
        var items = GetAll();
        var names = commaSeparated.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var name in names)
            items.Add(new GroceryItem { Name = name });
        Save(items);
    }

    public void RemoveItem(Guid id)
    {
        var items = GetAll();
        items.RemoveAll(i => i.Id == id);
        Save(items);
    }

    public void ClearAll()
    {
        Save(new List<GroceryItem>());
    }
}

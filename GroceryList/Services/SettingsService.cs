using System.Text.Json;

namespace GroceryList.Services;

public class SettingsService
{
    private readonly string _filePath;

    public SettingsService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "settings.json");
    }

    public List<string> GetCategoryOrder()
    {
        if (!File.Exists(_filePath)) return new List<string>();
        var json = File.ReadAllText(_filePath);
        var settings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
        if (settings != null && settings.TryGetValue("categoryOrder", out var val))
            return val.Deserialize<List<string>>() ?? new List<string>();
        return new List<string>();
    }

    public void SaveCategoryOrder(List<string> order)
    {
        var settings = new Dictionary<string, object> { ["categoryOrder"] = order };
        File.WriteAllText(_filePath, JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
    }
}

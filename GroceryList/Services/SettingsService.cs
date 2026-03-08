using System.Text.Json;

namespace GroceryList.Services;

public class SettingsService
{
    private readonly string _contentRoot;

    public SettingsService(IWebHostEnvironment env)
    {
        _contentRoot = env.ContentRootPath;
    }

    private string FilePath(string userId) =>
        Path.Combine(_contentRoot, $"settings-{userId}.json");

    public List<string> GetCategoryOrder(string userId)
    {
        var path = FilePath(userId);
        if (!File.Exists(path)) return new List<string>();
        var json = File.ReadAllText(path);
        var settings = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
        if (settings != null && settings.TryGetValue("categoryOrder", out var val))
            return val.Deserialize<List<string>>() ?? new List<string>();
        return new List<string>();
    }

    public void SaveCategoryOrder(string userId, List<string> order)
    {
        var settings = new Dictionary<string, object> { ["categoryOrder"] = order };
        File.WriteAllText(FilePath(userId), JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true }));
    }
}

using GroceryList.Helpers;

namespace GroceryList.Models;

public class StoreViewModel
{
    public List<StoreItem> StoreItems { get; set; } = new();
    public HashSet<string> ExistingItems { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

namespace GroceryList.Models;

public class StoreViewModel
{
    public List<string> StoreItems { get; set; } = new();
    public HashSet<string> ExistingItems { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}

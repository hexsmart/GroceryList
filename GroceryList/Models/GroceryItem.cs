namespace GroceryList.Models;

public class GroceryItem
{
    private string _name = string.Empty;
    private string _category = "Staple";

    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name
    {
        get => _name;
        set => _name = value ?? string.Empty;
    }
    
    public string Category
    {
        get => _category;
        set => _category = value ?? "Staple";
    }
}

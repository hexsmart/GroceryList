namespace GroceryList.Models;

public class SharedList
{
    private string _name = string.Empty;

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name
    {
        get => _name;
        set => _name = value ?? string.Empty;
    }
    public Guid OwnerId { get; set; }
    public List<Guid> MemberIds { get; set; } = new();
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}

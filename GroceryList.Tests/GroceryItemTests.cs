using GroceryList.Models;

namespace GroceryList.Tests;

public class GroceryItemTests
{
    [Fact]
    public void NewGroceryItem_HasNonEmptyGuid()
    {
        Assert.NotEqual(Guid.Empty, new GroceryItem().Id);
    }

    [Fact]
    public void NewGroceryItem_DefaultsToStapleCategory()
    {
        Assert.Equal("Staple", new GroceryItem().Category);
    }

    [Fact]
    public void NewGroceryItem_DefaultsToEmptyName()
    {
        Assert.Equal(string.Empty, new GroceryItem().Name);
    }
}

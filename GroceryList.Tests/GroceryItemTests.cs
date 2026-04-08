using GroceryList.Models;

namespace GroceryList.Tests;

[TestClass]
public class GroceryItemTests
{
    [TestMethod]
    public void NewGroceryItem_HasNonEmptyGuid()
    {
        Assert.AreNotEqual(Guid.Empty, new GroceryItem().Id);
    }

    [TestMethod]
    public void NewGroceryItem_DefaultsToStapleCategory()
    {
        Assert.AreEqual("Staple", new GroceryItem().Category);
    }

    [TestMethod]
    public void NewGroceryItem_DefaultsToEmptyName()
    {
        Assert.AreEqual(string.Empty, new GroceryItem().Name);
    }
}

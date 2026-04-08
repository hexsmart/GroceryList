using GroceryList.Models;

namespace GroceryList.Tests;

[TestClass]
public class GroceryItemTests
{
    [TestMethod]
    public void GroceryItem_Category_CanSetValue()
    {
        // Arrange
        var item = new GroceryItem();
        var expectedCategory = "Produce";

        // Act
        item.Category = expectedCategory;

        // Assert
        Assert.AreEqual(expectedCategory, item.Category);
    }

    [TestMethod]
    public void GroceryItem_Category_ConvertsNullToDefault()
    {
        // Arrange
        var item = new GroceryItem { Category = "Produce" };

        // Act
        item.Category = null!;

        // Assert
        Assert.AreEqual("Staple", item.Category);
    }

    [TestMethod]
    public void GroceryItem_Category_DefaultsToStaple()
    {
        // Arrange & Act
        var item = new GroceryItem();

        // Assert
        Assert.AreEqual("Staple", item.Category);
    }

    [TestMethod]
    public void GroceryItem_Id_CanSetValue()
    {
        // Arrange
        var item = new GroceryItem();
        var expectedId = Guid.NewGuid();

        // Act
        item.Id = expectedId;

        // Assert
        Assert.AreEqual(expectedId, item.Id);
    }

    [TestMethod]
    public void GroceryItem_Id_IsNonEmptyGuid()
    {
        // Arrange & Act
        var item = new GroceryItem();

        // Assert
        Assert.AreNotEqual(Guid.Empty, item.Id);
    }

    [TestMethod]
    public void GroceryItem_Id_IsUniquePerInstance()
    {
        // Arrange & Act
        var item1 = new GroceryItem();
        var item2 = new GroceryItem();

        // Assert
        Assert.AreNotEqual(item1.Id, item2.Id);
    }

    [TestMethod]
    public void GroceryItem_Name_CanSetValue()
    {
        // Arrange
        var item = new GroceryItem();
        var expectedName = "Milk";

        // Act
        item.Name = expectedName;

        // Assert
        Assert.AreEqual(expectedName, item.Name);
    }

    [TestMethod]
    public void GroceryItem_Name_ConvertsNullToEmpty()
    {
        // Arrange
        var item = new GroceryItem { Name = "Milk" };

        // Act
        item.Name = null!;

        // Assert
        Assert.AreEqual(string.Empty, item.Name);
    }

    [TestMethod]
    public void GroceryItem_Name_DefaultsToEmpty()
    {
        // Arrange & Act
        var item = new GroceryItem();

        // Assert
        Assert.AreEqual(string.Empty, item.Name);
    }

    [TestMethod]
    public void GroceryItem_ObjectInitializer_SetsAllProperties()
    {
        // Arrange
        var expectedName = "Bread";
        var expectedCategory = "Bakery";

        // Act
        var item = new GroceryItem
        {
            Name = expectedName,
            Category = expectedCategory
        };

        // Assert
        Assert.AreEqual(expectedName, item.Name);
        Assert.AreEqual(expectedCategory, item.Category);
        Assert.AreNotEqual(Guid.Empty, item.Id);
    }
}

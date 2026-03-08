using GroceryList.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace GroceryList.Tests;

public class GroceryServiceTests : IDisposable
{
    private readonly string _tempFile;
    private readonly GroceryService _service;

    public GroceryServiceTests()
    {
        var dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(dir);
        _tempFile = Path.Combine(dir, "groceries.json");
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(dir);
        _service = new GroceryService(env.Object);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFile)) File.Delete(_tempFile);
    }

    [Fact]
    public void GetAll_ReturnsEmpty_WhenNoFile()
    {
        var items = _service.GetAll();
        Assert.Empty(items);
    }

    [Fact]
    public void AddItems_AddsASingleItem()
    {
        _service.AddItems("Milk");
        Assert.Single(_service.GetAll());
        Assert.Equal("Milk", _service.GetAll()[0].Name);
    }

    [Fact]
    public void AddItems_CapitalizesFirstLetter()
    {
        _service.AddItems("milk");
        Assert.Equal("Milk", _service.GetAll()[0].Name);
    }

    [Fact]
    public void AddItems_ParsesCommaSeparatedItems()
    {
        _service.AddItems("Milk, Eggs, Bread");
        Assert.Equal(3, _service.GetAll().Count);
    }

    [Fact]
    public void AddItems_PreventsDuplicates()
    {
        _service.AddItems("Milk");
        _service.AddItems("Milk");
        Assert.Single(_service.GetAll());
    }

    [Fact]
    public void AddItems_PreventsDuplicates_CaseInsensitive()
    {
        _service.AddItems("Milk");
        _service.AddItems("milk");
        Assert.Single(_service.GetAll());
    }

    [Fact]
    public void AddItems_ReturnsAlphabetizedList()
    {
        _service.AddItems("Zebra, Apple, Mango");
        var items = _service.GetAll();
        Assert.Equal("Apple", items[0].Name);
        Assert.Equal("Mango", items[1].Name);
        Assert.Equal("Zebra", items[2].Name);
    }

    [Fact]
    public void RemoveItem_RemovesCorrectItem()
    {
        _service.AddItems("Milk, Eggs");
        var milkId = _service.GetAll().First(i => i.Name == "Milk").Id;
        _service.RemoveItem(milkId);
        var remaining = _service.GetAll();
        Assert.Single(remaining);
        Assert.Equal("Eggs", remaining[0].Name);
    }

    [Fact]
    public void RemoveItem_DoesNothing_WhenIdNotFound()
    {
        _service.AddItems("Milk");
        _service.RemoveItem(Guid.NewGuid());
        Assert.Single(_service.GetAll());
    }

    [Fact]
    public void ClearAll_RemovesAllItems()
    {
        _service.AddItems("Milk, Eggs, Bread");
        _service.ClearAll();
        Assert.Empty(_service.GetAll());
    }

    [Fact]
    public void NewItem_DefaultsToStapleCategory()
    {
        _service.AddItems("Milk");
        Assert.Equal("Staple", _service.GetAll()[0].Category);
    }

    [Fact]
    public void Save_PersistsUpdatedCategory()
    {
        _service.AddItems("Milk");
        var items = _service.GetAll();
        items[0].Category = "Other";
        _service.Save(items);
        Assert.Equal("Other", _service.GetAll()[0].Category);
    }
}

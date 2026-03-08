using GroceryList.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace GroceryList.Tests;

public class GroceryServiceTests : IDisposable
{
    private readonly string _dir;
    private readonly GroceryService _service;
    private const string UserId = "test-user";

    public GroceryServiceTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_dir);
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(_dir);
        _service = new GroceryService(env.Object);
    }

    public void Dispose() => Directory.Delete(_dir, recursive: true);

    [Fact]
    public void GetAll_ReturnsEmpty_WhenNoFile()
    {
        Assert.Empty(_service.GetAll(UserId));
    }

    [Fact]
    public void AddItems_AddsASingleItem()
    {
        _service.AddItems(UserId, "Milk");
        Assert.Single(_service.GetAll(UserId));
        Assert.Equal("Milk", _service.GetAll(UserId)[0].Name);
    }

    [Fact]
    public void AddItems_CapitalizesFirstLetter()
    {
        _service.AddItems(UserId, "milk");
        Assert.Equal("Milk", _service.GetAll(UserId)[0].Name);
    }

    [Fact]
    public void AddItems_ParsesCommaSeparatedItems()
    {
        _service.AddItems(UserId, "Milk, Eggs, Bread");
        Assert.Equal(3, _service.GetAll(UserId).Count);
    }

    [Fact]
    public void AddItems_PreventsDuplicates()
    {
        _service.AddItems(UserId, "Milk");
        _service.AddItems(UserId, "Milk");
        Assert.Single(_service.GetAll(UserId));
    }

    [Fact]
    public void AddItems_PreventsDuplicates_CaseInsensitive()
    {
        _service.AddItems(UserId, "Milk");
        _service.AddItems(UserId, "milk");
        Assert.Single(_service.GetAll(UserId));
    }

    [Fact]
    public void AddItems_ReturnsAlphabetizedList()
    {
        _service.AddItems(UserId, "Zebra, Apple, Mango");
        var items = _service.GetAll(UserId);
        Assert.Equal("Apple", items[0].Name);
        Assert.Equal("Mango", items[1].Name);
        Assert.Equal("Zebra", items[2].Name);
    }

    [Fact]
    public void RemoveItem_RemovesCorrectItem()
    {
        _service.AddItems(UserId, "Milk, Eggs");
        var milkId = _service.GetAll(UserId).First(i => i.Name == "Milk").Id;
        _service.RemoveItem(UserId, milkId);
        var remaining = _service.GetAll(UserId);
        Assert.Single(remaining);
        Assert.Equal("Eggs", remaining[0].Name);
    }

    [Fact]
    public void RemoveItem_DoesNothing_WhenIdNotFound()
    {
        _service.AddItems(UserId, "Milk");
        _service.RemoveItem(UserId, Guid.NewGuid());
        Assert.Single(_service.GetAll(UserId));
    }

    [Fact]
    public void ClearAll_RemovesAllItems()
    {
        _service.AddItems(UserId, "Milk, Eggs, Bread");
        _service.ClearAll(UserId);
        Assert.Empty(_service.GetAll(UserId));
    }

    [Fact]
    public void NewItem_DefaultsToStapleCategory()
    {
        _service.AddItems(UserId, "Milk");
        Assert.Equal("Staple", _service.GetAll(UserId)[0].Category);
    }

    [Fact]
    public void Save_PersistsUpdatedCategory()
    {
        _service.AddItems(UserId, "Milk");
        var items = _service.GetAll(UserId);
        items[0].Category = "Other";
        _service.Save(UserId, items);
        Assert.Equal("Other", _service.GetAll(UserId)[0].Category);
    }

    [Fact]
    public void DifferentUsers_HaveSeparateLists()
    {
        _service.AddItems("user-a", "Milk");
        _service.AddItems("user-b", "Eggs");
        Assert.Equal("Milk", _service.GetAll("user-a")[0].Name);
        Assert.Equal("Eggs", _service.GetAll("user-b")[0].Name);
        Assert.Single(_service.GetAll("user-a"));
        Assert.Single(_service.GetAll("user-b"));
    }
}

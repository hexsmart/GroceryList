using GroceryList.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace GroceryList.Tests;

[TestClass]
public class GroceryServiceTests
{
    private string _dir = null!;
    private GroceryService _service = null!;
    private const string UserId = "test-user";

    [TestInitialize]
    public void Initialize()
    {
        _dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_dir);
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(_dir);
        _service = new GroceryService(env.Object);
    }

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dir, recursive: true);

    [TestMethod]
    public void GroceryService_AddItems_AddsASingleItem()
    {
        _service.AddItems(UserId, "Milk");
        Assert.AreEqual(1, _service.GetAll(UserId).Count);
        Assert.AreEqual("Milk", _service.GetAll(UserId)[0].Name);
    }

    [TestMethod]
    public void GroceryService_AddItems_CapitalizesFirstLetter()
    {
        _service.AddItems(UserId, "milk");
        Assert.AreEqual("Milk", _service.GetAll(UserId)[0].Name);
    }

    [TestMethod]
    public void GroceryService_AddItems_ParsesCommaSeparatedItems()
    {
        _service.AddItems(UserId, "Milk, Eggs, Bread");
        Assert.AreEqual(3, _service.GetAll(UserId).Count);
    }

    [TestMethod]
    public void GroceryService_AddItems_PreventsDuplicates()
    {
        _service.AddItems(UserId, "Milk");
        _service.AddItems(UserId, "Milk");
        Assert.AreEqual(1, _service.GetAll(UserId).Count);
    }

    [TestMethod]
    public void GroceryService_AddItems_PreventsDuplicatesCaseInsensitive()
    {
        _service.AddItems(UserId, "Milk");
        _service.AddItems(UserId, "milk");
        Assert.AreEqual(1, _service.GetAll(UserId).Count);
    }

    [TestMethod]
    public void GroceryService_AddItems_ReturnsAlphabetizedList()
    {
        _service.AddItems(UserId, "Zebra, Apple, Mango");
        var items = _service.GetAll(UserId);
        Assert.AreEqual("Apple", items[0].Name);
        Assert.AreEqual("Mango", items[1].Name);
        Assert.AreEqual("Zebra", items[2].Name);
    }

    [TestMethod]
    public void GroceryService_ClearAll_RemovesAllItems()
    {
        _service.AddItems(UserId, "Milk, Eggs, Bread");
        _service.ClearAll(UserId);
        Assert.AreEqual(0, _service.GetAll(UserId).Count);
    }

    [TestMethod]
    public void GroceryService_DifferentUsers_HaveSeparateLists()
    {
        _service.AddItems("user-a", "Milk");
        _service.AddItems("user-b", "Eggs");
        Assert.AreEqual("Milk", _service.GetAll("user-a")[0].Name);
        Assert.AreEqual("Eggs", _service.GetAll("user-b")[0].Name);
        Assert.AreEqual(1, _service.GetAll("user-a").Count);
        Assert.AreEqual(1, _service.GetAll("user-b").Count);
    }

    [TestMethod]
    public void GroceryService_GetAll_ReturnsEmptyWhenNoFile()
    {
        Assert.AreEqual(0, _service.GetAll(UserId).Count);
    }

    [TestMethod]
    public void GroceryService_NewItem_DefaultsToStapleCategory()
    {
        _service.AddItems(UserId, "Milk");
        Assert.AreEqual("Staple", _service.GetAll(UserId)[0].Category);
    }

    [TestMethod]
    public void GroceryService_RemoveItem_DoesNothingWhenIdNotFound()
    {
        _service.AddItems(UserId, "Milk");
        _service.RemoveItem(UserId, Guid.NewGuid());
        Assert.AreEqual(1, _service.GetAll(UserId).Count);
    }

    [TestMethod]
    public void GroceryService_RemoveItem_RemovesCorrectItem()
    {
        _service.AddItems(UserId, "Milk, Eggs");
        var milkId = _service.GetAll(UserId).First(i => i.Name == "Milk").Id;
        _service.RemoveItem(UserId, milkId);
        var remaining = _service.GetAll(UserId);
        Assert.AreEqual(1, remaining.Count);
        Assert.AreEqual("Eggs", remaining[0].Name);
    }

    [TestMethod]
    public void GroceryService_Save_PersistsUpdatedCategory()
    {
        _service.AddItems(UserId, "Milk");
        var items = _service.GetAll(UserId);
        items[0].Category = "Other";
        _service.Save(UserId, items);
        Assert.AreEqual("Other", _service.GetAll(UserId)[0].Category);
    }
}

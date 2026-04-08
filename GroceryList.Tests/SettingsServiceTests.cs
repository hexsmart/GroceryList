using GroceryList.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace GroceryList.Tests;

[TestClass]
public class SettingsServiceTests
{
    private string _dir = null!;
    private SettingsService _service = null!;
    private const string UserId = "test-user";

    [TestInitialize]
    public void Initialize()
    {
        _dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_dir);
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(_dir);
        _service = new SettingsService(env.Object);
    }

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dir, recursive: true);

    [TestMethod]
    public void SettingsService_DifferentUsers_HaveSeparateSettings()
    {
        _service.SaveCategoryOrder("user-a", new List<string> { "Produce", "Dairy" });
        _service.SaveCategoryOrder("user-b", new List<string> { "Beverages", "Frozen" });

        var orderA = _service.GetCategoryOrder("user-a");
        var orderB = _service.GetCategoryOrder("user-b");

        Assert.AreEqual("Produce", orderA[0]);
        Assert.AreEqual("Beverages", orderB[0]);
    }

    [TestMethod]
    public void SettingsService_GetCategoryOrder_ReturnsEmptyForUnknownUser()
    {
        _service.SaveCategoryOrder("other-user", new List<string> { "Produce" });
        var result = _service.GetCategoryOrder("unknown-user");
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SettingsService_GetCategoryOrder_ReturnsEmptyWhenNoFile()
    {
        var order = _service.GetCategoryOrder(UserId);
        Assert.AreEqual(0, order.Count);
    }

    [TestMethod]
    public void SettingsService_SaveAndGet_RoundTripsCategoryOrder()
    {
        var order = new List<string> { "Produce", "Dairy", "Beverages" };
        _service.SaveCategoryOrder(UserId, order);
        var result = _service.GetCategoryOrder(UserId);
        CollectionAssert.AreEqual(order, result);
    }

    [TestMethod]
    public void SettingsService_SaveCategoryOrder_CreatesFilePerUser()
    {
        _service.SaveCategoryOrder("user-x", new List<string> { "Dairy" });
        Assert.IsTrue(File.Exists(Path.Combine(_dir, "settings-user-x.json")));
    }

    [TestMethod]
    public void SettingsService_SaveCategoryOrder_OverwritesPreviousOrder()
    {
        _service.SaveCategoryOrder(UserId, new List<string> { "Produce", "Dairy" });
        _service.SaveCategoryOrder(UserId, new List<string> { "Dairy", "Produce" });
        var result = _service.GetCategoryOrder(UserId);
        Assert.AreEqual("Dairy", result[0]);
        Assert.AreEqual("Produce", result[1]);
    }
}

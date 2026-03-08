using GroceryList.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace GroceryList.Tests;

public class SettingsServiceTests : IDisposable
{
    private readonly string _dir;
    private readonly SettingsService _service;
    private const string UserId = "test-user";

    public SettingsServiceTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_dir);
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(_dir);
        _service = new SettingsService(env.Object);
    }

    public void Dispose() => Directory.Delete(_dir, recursive: true);

    [Fact]
    public void GetCategoryOrder_ReturnsEmpty_WhenNoFile()
    {
        var order = _service.GetCategoryOrder(UserId);
        Assert.Empty(order);
    }

    [Fact]
    public void SaveAndGet_RoundTrips_CategoryOrder()
    {
        var order = new List<string> { "Produce", "Dairy", "Beverages" };
        _service.SaveCategoryOrder(UserId, order);
        var result = _service.GetCategoryOrder(UserId);
        Assert.Equal(order, result);
    }

    [Fact]
    public void SaveCategoryOrder_OverwritesPreviousOrder()
    {
        _service.SaveCategoryOrder(UserId, new List<string> { "Produce", "Dairy" });
        _service.SaveCategoryOrder(UserId, new List<string> { "Dairy", "Produce" });
        var result = _service.GetCategoryOrder(UserId);
        Assert.Equal("Dairy", result[0]);
        Assert.Equal("Produce", result[1]);
    }

    [Fact]
    public void DifferentUsers_HaveSeparateSettings()
    {
        _service.SaveCategoryOrder("user-a", new List<string> { "Produce", "Dairy" });
        _service.SaveCategoryOrder("user-b", new List<string> { "Beverages", "Frozen" });

        var orderA = _service.GetCategoryOrder("user-a");
        var orderB = _service.GetCategoryOrder("user-b");

        Assert.Equal("Produce", orderA[0]);
        Assert.Equal("Beverages", orderB[0]);
    }

    [Fact]
    public void GetCategoryOrder_ReturnsEmpty_ForUnknownUser()
    {
        _service.SaveCategoryOrder("other-user", new List<string> { "Produce" });
        var result = _service.GetCategoryOrder("unknown-user");
        Assert.Empty(result);
    }

    [Fact]
    public void SaveCategoryOrder_CreatesFile_PerUser()
    {
        _service.SaveCategoryOrder("user-x", new List<string> { "Dairy" });
        Assert.True(File.Exists(Path.Combine(_dir, "settings-user-x.json")));
    }
}

using GroceryList.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace GroceryList.Tests;

public class UserServiceTests : IDisposable
{
    private readonly string _dir;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_dir);
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(_dir);
        _service = new UserService(env.Object);
    }

    public void Dispose() => Directory.Delete(_dir, recursive: true);

    [Fact]
    public void GetAll_ReturnsEmpty_WhenNoFile()
    {
        Assert.Empty(_service.GetAll());
    }

    [Fact]
    public void Register_AddsUser()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        Assert.Single(_service.GetAll());
    }

    [Fact]
    public void Register_PersistsAllFields()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        var user = _service.GetAll()[0];
        Assert.Equal("Jane", user.FirstName);
        Assert.Equal("Doe", user.LastName);
        Assert.Equal("jane@example.com", user.Email);
    }

    [Fact]
    public void Register_AssignsUniqueId()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        _service.Register("John", "Smith", "john@example.com");
        var users = _service.GetAll();
        Assert.NotEqual(users[0].Id, users[1].Id);
    }

    [Fact]
    public void FindByEmail_ReturnsCorrectUser()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        var user = _service.FindByEmail("jane@example.com");
        Assert.NotNull(user);
        Assert.Equal("Jane", user!.FirstName);
    }

    [Fact]
    public void FindByEmail_IsCaseInsensitive()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        var user = _service.FindByEmail("JANE@EXAMPLE.COM");
        Assert.NotNull(user);
    }

    [Fact]
    public void FindByEmail_ReturnsNull_WhenNotFound()
    {
        var user = _service.FindByEmail("nobody@example.com");
        Assert.Null(user);
    }

    [Fact]
    public void Register_MultipleUsers_AllPersisted()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        _service.Register("John", "Smith", "john@example.com");
        Assert.Equal(2, _service.GetAll().Count);
    }
}

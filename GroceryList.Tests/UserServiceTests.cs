using GroceryList.Services;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace GroceryList.Tests;

[TestClass]
public class UserServiceTests
{
    private string _dir = null!;
    private UserService _service = null!;

    [TestInitialize]
    public void Initialize()
    {
        _dir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_dir);
        var env = new Mock<IWebHostEnvironment>();
        env.Setup(e => e.ContentRootPath).Returns(_dir);
        _service = new UserService(env.Object);
    }

    [TestCleanup]
    public void Cleanup() => Directory.Delete(_dir, recursive: true);

    [TestMethod]
    public void GetAll_ReturnsEmpty_WhenNoFile()
    {
        Assert.AreEqual(0, _service.GetAll().Count);
    }

    [TestMethod]
    public void Register_AddsUser()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        Assert.AreEqual(1, _service.GetAll().Count);
    }

    [TestMethod]
    public void Register_PersistsAllFields()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        var user = _service.GetAll()[0];
        Assert.AreEqual("Jane", user.FirstName);
        Assert.AreEqual("Doe", user.LastName);
        Assert.AreEqual("jane@example.com", user.Email);
    }

    [TestMethod]
    public void Register_AssignsUniqueId()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        _service.Register("John", "Smith", "john@example.com");
        var users = _service.GetAll();
        Assert.AreNotEqual(users[0].Id, users[1].Id);
    }

    [TestMethod]
    public void FindByEmail_ReturnsCorrectUser()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        var user = _service.FindByEmail("jane@example.com");
        Assert.IsNotNull(user);
        Assert.AreEqual("Jane", user!.FirstName);
    }

    [TestMethod]
    public void FindByEmail_IsCaseInsensitive()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        var user = _service.FindByEmail("JANE@EXAMPLE.COM");
        Assert.IsNotNull(user);
    }

    [TestMethod]
    public void FindByEmail_ReturnsNull_WhenNotFound()
    {
        var user = _service.FindByEmail("nobody@example.com");
        Assert.IsNull(user);
    }

    [TestMethod]
    public void Register_MultipleUsers_AllPersisted()
    {
        _service.Register("Jane", "Doe", "jane@example.com");
        _service.Register("John", "Smith", "john@example.com");
        Assert.AreEqual(2, _service.GetAll().Count);
    }
}

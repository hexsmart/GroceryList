using Microsoft.VisualStudio.TestTools.UnitTesting;
using GroceryList.Services;
using GroceryList.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace GroceryList.Tests;

[TestClass]
public class SharedListServiceTests
{
    private SharedListService _service = null!;
    private UserService _userService = null!;
    private string _testContentRoot = null!;

    [TestInitialize]
    public void Setup()
    {
        _testContentRoot = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testContentRoot);
        var env = new TestWebHostEnvironment { ContentRootPath = _testContentRoot };
        _service = new SharedListService(env);
        _userService = new UserService(env);
    }

    [TestCleanup]
    public void Cleanup()
    {
        if (Directory.Exists(_testContentRoot))
        {
            Directory.Delete(_testContentRoot, true);
        }
    }

    [TestMethod]
    public void SharedListService_AddMember_AddsUserSuccessfully()
    {
        var owner = Guid.NewGuid().ToString();
        var memberEmail = "member@test.com";
        _userService.Register("Member", "User", memberEmail);

        var list = _service.CreateSharedList(owner, "Test List");
        var result = _service.AddMember(list.Id, owner, memberEmail, _userService);

        Assert.IsTrue(result);
        var updatedList = _service.GetSharedList(list.Id);
        Assert.AreEqual(1, updatedList!.MemberIds.Count);
    }

    [TestMethod]
    public void SharedListService_AddMember_FailsForNonExistentUser()
    {
        var owner = Guid.NewGuid().ToString();
        var list = _service.CreateSharedList(owner, "Test List");

        var result = _service.AddMember(list.Id, owner, "nonexistent@test.com", _userService);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SharedListService_AddMember_FailsForNonOwner()
    {
        var owner = Guid.NewGuid().ToString();
        var nonOwner = Guid.NewGuid().ToString();
        var memberEmail = "member@test.com";
        _userService.Register("Member", "User", memberEmail);

        var list = _service.CreateSharedList(owner, "Test List");
        var result = _service.AddMember(list.Id, nonOwner, memberEmail, _userService);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SharedListService_AddMember_PreventsDuplicateMembers()
    {
        var owner = Guid.NewGuid().ToString();
        var memberEmail = "member@test.com";
        _userService.Register("Member", "User", memberEmail);

        var list = _service.CreateSharedList(owner, "Test List");
        _service.AddMember(list.Id, owner, memberEmail, _userService);
        var result = _service.AddMember(list.Id, owner, memberEmail, _userService);

        Assert.IsFalse(result);
        var updatedList = _service.GetSharedList(list.Id);
        Assert.AreEqual(1, updatedList!.MemberIds.Count);
    }

    [TestMethod]
    public void SharedListService_CreateSharedList_CreatesListSuccessfully()
    {
        var userId = Guid.NewGuid().ToString();
        var listName = "Family Groceries";

        var result = _service.CreateSharedList(userId, listName);

        Assert.IsNotNull(result);
        Assert.AreEqual(listName, result.Name);
        Assert.AreEqual(Guid.Parse(userId), result.OwnerId);
        Assert.AreEqual(0, result.MemberIds.Count);
    }

    [TestMethod]
    public void SharedListService_DeleteSharedList_DeletesSuccessfully()
    {
        var owner = Guid.NewGuid().ToString();
        var list = _service.CreateSharedList(owner, "Test List");

        var result = _service.DeleteSharedList(list.Id, owner);

        Assert.IsTrue(result);
        var deletedList = _service.GetSharedList(list.Id);
        Assert.IsNull(deletedList);
    }

    [TestMethod]
    public void SharedListService_DeleteSharedList_FailsForNonOwner()
    {
        var owner = Guid.NewGuid().ToString();
        var nonOwner = Guid.NewGuid().ToString();
        var list = _service.CreateSharedList(owner, "Test List");

        var result = _service.DeleteSharedList(list.Id, nonOwner);

        Assert.IsFalse(result);
        var stillExists = _service.GetSharedList(list.Id);
        Assert.IsNotNull(stillExists);
    }

    [TestMethod]
    public void SharedListService_GetAllSharedLists_ReturnsEmptyListInitially()
    {
        var result = _service.GetAllSharedLists();

        Assert.IsNotNull(result);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void SharedListService_GetSharedList_ReturnsCorrectList()
    {
        var userId = Guid.NewGuid().ToString();
        var listName = "Test List";
        var created = _service.CreateSharedList(userId, listName);

        var result = _service.GetSharedList(created.Id);

        Assert.IsNotNull(result);
        Assert.AreEqual(created.Id, result.Id);
        Assert.AreEqual(listName, result.Name);
    }

    [TestMethod]
    public void SharedListService_GetSharedList_ReturnsNullForNonExistentList()
    {
        var result = _service.GetSharedList(Guid.NewGuid());

        Assert.IsNull(result);
    }

    [TestMethod]
    public void SharedListService_GetUserSharedLists_ReturnsOnlyAccessibleLists()
    {
        var owner = Guid.NewGuid().ToString();
        var nonMember = Guid.NewGuid().ToString();

        var list = _service.CreateSharedList(owner, "Shared List");
        var member = _userService.Register("Member", "User", "member@test.com");
        _service.AddMember(list.Id, owner, "member@test.com", _userService);

        var ownerLists = _service.GetUserSharedLists(owner);
        var memberLists = _service.GetUserSharedLists(member.Id.ToString());
        var nonMemberLists = _service.GetUserSharedLists(nonMember);

        Assert.AreEqual(1, ownerLists.Count);
        Assert.AreEqual(1, memberLists.Count);
        Assert.AreEqual(0, nonMemberLists.Count);
    }

    [TestMethod]
    public void SharedListService_HasAccess_ReturnsFalseForNonMember()
    {
        var owner = Guid.NewGuid().ToString();
        var nonMember = Guid.NewGuid().ToString();
        var list = _service.CreateSharedList(owner, "Test List");

        var result = _service.HasAccess(list.Id, nonMember);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SharedListService_HasAccess_ReturnsTrueForMember()
    {
        var owner = Guid.NewGuid().ToString();
        var memberEmail = "member@test.com";
        var member = _userService.Register("Member", "User", memberEmail);

        var list = _service.CreateSharedList(owner, "Test List");
        _service.AddMember(list.Id, owner, memberEmail, _userService);

        var result = _service.HasAccess(list.Id, member.Id.ToString());

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void SharedListService_HasAccess_ReturnsTrueForOwner()
    {
        var owner = Guid.NewGuid().ToString();
        var list = _service.CreateSharedList(owner, "Test List");

        var result = _service.HasAccess(list.Id, owner);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void SharedListService_LeaveSharedList_FailsForOwner()
    {
        var owner = Guid.NewGuid().ToString();
        var list = _service.CreateSharedList(owner, "Test List");

        var result = _service.LeaveSharedList(list.Id, owner);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SharedListService_LeaveSharedList_RemovesMemberSuccessfully()
    {
        var owner = Guid.NewGuid().ToString();
        var memberEmail = "member@test.com";
        var member = _userService.Register("Member", "User", memberEmail);

        var list = _service.CreateSharedList(owner, "Test List");
        _service.AddMember(list.Id, owner, memberEmail, _userService);

        var result = _service.LeaveSharedList(list.Id, member.Id.ToString());

        Assert.IsTrue(result);
        var updatedList = _service.GetSharedList(list.Id);
        Assert.AreEqual(0, updatedList!.MemberIds.Count);
    }

    [TestMethod]
    public void SharedListService_RemoveMember_FailsForNonOwner()
    {
        var owner = Guid.NewGuid().ToString();
        var nonOwner = Guid.NewGuid().ToString();
        var memberEmail = "member@test.com";
        var member = _userService.Register("Member", "User", memberEmail);

        var list = _service.CreateSharedList(owner, "Test List");
        _service.AddMember(list.Id, owner, memberEmail, _userService);

        var result = _service.RemoveMember(list.Id, nonOwner, member.Id);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SharedListService_RemoveMember_RemovesUserSuccessfully()
    {
        var owner = Guid.NewGuid().ToString();
        var memberEmail = "member@test.com";
        var member = _userService.Register("Member", "User", memberEmail);

        var list = _service.CreateSharedList(owner, "Test List");
        _service.AddMember(list.Id, owner, memberEmail, _userService);

        var result = _service.RemoveMember(list.Id, owner, member.Id);

        Assert.IsTrue(result);
        var updatedList = _service.GetSharedList(list.Id);
        Assert.AreEqual(0, updatedList!.MemberIds.Count);
    }

    [TestMethod]
    public void SharedListService_RenameSharedList_FailsForNonOwner()
    {
        var owner = Guid.NewGuid().ToString();
        var nonOwner = Guid.NewGuid().ToString();
        var list = _service.CreateSharedList(owner, "Original");

        var result = _service.RenameSharedList(list.Id, nonOwner, "New Name");

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void SharedListService_RenameSharedList_RenamesSuccessfully()
    {
        var owner = Guid.NewGuid().ToString();
        var list = _service.CreateSharedList(owner, "Old Name");
        var newName = "New Name";

        var result = _service.RenameSharedList(list.Id, owner, newName);

        Assert.IsTrue(result);
        var updatedList = _service.GetSharedList(list.Id);
        Assert.AreEqual(newName, updatedList!.Name);
    }

    private class TestWebHostEnvironment : IWebHostEnvironment
    {
        public string WebRootPath { get; set; } = string.Empty;
        public string ContentRootPath { get; set; } = string.Empty;
        public string ApplicationName { get; set; } = string.Empty;
        public string EnvironmentName { get; set; } = string.Empty;
        public IFileProvider WebRootFileProvider { get; set; } = null!;
        public IFileProvider ContentRootFileProvider { get; set; } = null!;
    }
}
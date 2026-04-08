using Microsoft.AspNetCore.Mvc;
using GroceryList.Services;

namespace GroceryList.Controllers;

public class SharedListController : Controller
{
    private readonly SharedListService _sharedListService;
    private readonly UserService _userService;

    public SharedListController(SharedListService sharedListService, UserService userService)
    {
        _sharedListService = sharedListService;
        _userService = userService;
    }

    private string? UserId => HttpContext.Session.GetString("UserId");

    [HttpGet]
    public IActionResult Manage()
    {
        if (string.IsNullOrEmpty(UserId))
            return RedirectToAction("Login", "Account");

        var sharedLists = _sharedListService.GetUserSharedLists(UserId);
        return View(sharedLists);
    }

    [HttpPost]
    public IActionResult Create(string listName)
    {
        if (string.IsNullOrEmpty(UserId))
            return RedirectToAction("Login", "Account");

        if (string.IsNullOrWhiteSpace(listName))
        {
            TempData["Error"] = "List name cannot be empty.";
            return RedirectToAction("Manage");
        }

        _sharedListService.CreateSharedList(UserId, listName);
        TempData["Success"] = $"Shared list '{listName}' created successfully!";
        return RedirectToAction("Manage");
    }

    [HttpPost]
    public IActionResult AddMember(Guid listId, string memberEmail)
    {
        if (string.IsNullOrEmpty(UserId))
            return RedirectToAction("Login", "Account");

        if (string.IsNullOrWhiteSpace(memberEmail))
        {
            TempData["Error"] = "Email cannot be empty.";
            return RedirectToAction("Manage");
        }

        var success = _sharedListService.AddMember(listId, UserId, memberEmail, _userService);
        if (success)
        {
            TempData["Success"] = $"User '{memberEmail}' added to the list successfully!";
        }
        else
        {
            TempData["Error"] = "Failed to add member. Make sure the email is registered and not already a member.";
        }

        return RedirectToAction("Manage");
    }

    [HttpPost]
    public IActionResult RemoveMember(Guid listId, Guid memberId)
    {
        if (string.IsNullOrEmpty(UserId))
            return RedirectToAction("Login", "Account");

        var success = _sharedListService.RemoveMember(listId, UserId, memberId);
        if (success)
        {
            TempData["Success"] = "Member removed successfully!";
        }
        else
        {
            TempData["Error"] = "Failed to remove member.";
        }

        return RedirectToAction("Manage");
    }

    [HttpPost]
    public IActionResult Leave(Guid listId)
    {
        if (string.IsNullOrEmpty(UserId))
            return RedirectToAction("Login", "Account");

        var success = _sharedListService.LeaveSharedList(listId, UserId);
        if (success)
        {
            TempData["Success"] = "You have left the shared list.";
        }
        else
        {
            TempData["Error"] = "Failed to leave the list.";
        }

        return RedirectToAction("Manage");
    }

    [HttpPost]
    public IActionResult Delete(Guid listId)
    {
        if (string.IsNullOrEmpty(UserId))
            return RedirectToAction("Login", "Account");

        var success = _sharedListService.DeleteSharedList(listId, UserId);
        if (success)
        {
            TempData["Success"] = "Shared list deleted successfully!";
        }
        else
        {
            TempData["Error"] = "Failed to delete the list. Only the owner can delete a list.";
        }

        return RedirectToAction("Manage");
    }

    [HttpPost]
    public IActionResult Rename(Guid listId, string newName)
    {
        if (string.IsNullOrEmpty(UserId))
            return RedirectToAction("Login", "Account");

        if (string.IsNullOrWhiteSpace(newName))
        {
            TempData["Error"] = "List name cannot be empty.";
            return RedirectToAction("Manage");
        }

        var success = _sharedListService.RenameSharedList(listId, UserId, newName);
        if (success)
        {
            TempData["Success"] = "List renamed successfully!";
        }
        else
        {
            TempData["Error"] = "Failed to rename the list. Only the owner can rename a list.";
        }

        return RedirectToAction("Manage");
    }
}

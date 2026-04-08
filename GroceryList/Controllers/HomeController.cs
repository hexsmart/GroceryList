using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GroceryList.Models;
using GroceryList.Services;

namespace GroceryList.Controllers;

public class HomeController : Controller
{
    private readonly GroceryService _groceryService;
    private readonly SettingsService _settingsService;
    private readonly SharedListService _sharedListService;

    public HomeController(GroceryService groceryService, SettingsService settingsService, SharedListService sharedListService)
    {
        _groceryService = groceryService;
        _settingsService = settingsService;
        _sharedListService = sharedListService;
    }

    private string? UserId => HttpContext.Session.GetString("UserId");

    // Get the current list ID (either personal userId or shared list ID)
    private string? CurrentListId
    {
        get
        {
            var listId = HttpContext.Session.GetString("CurrentListId");
            // If not set, default to user's personal list
            if (string.IsNullOrEmpty(listId) && !string.IsNullOrEmpty(UserId))
            {
                HttpContext.Session.SetString("CurrentListId", UserId);
                return UserId;
            }
            return listId;
        }
    }

    // Check if current list is a shared list
    private bool IsSharedList => CurrentListId != UserId;

    private IActionResult RequireLogin()
    {
        if (UserId == null) return RedirectToAction("Login", "Account");
        return null!;
    }

    public IActionResult Index()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;

        // Verify shared list access if not personal list
        if (IsSharedList && CurrentListId != null)
        {
            var listIdGuid = Guid.Parse(CurrentListId);
            if (!_sharedListService.HasAccess(listIdGuid, UserId!))
            {
                TempData["Error"] = "You don't have access to this shared list.";
                HttpContext.Session.SetString("CurrentListId", UserId!);
                return RedirectToAction(nameof(Index));
            }
        }

        var items = _groceryService.GetAll(CurrentListId!);
        ViewBag.CategoryOrder = _settingsService.GetCategoryOrder(UserId!);
        ViewBag.CurrentListId = CurrentListId;
        ViewBag.IsSharedList = IsSharedList;

        // Get list name for display
        if (IsSharedList && CurrentListId != null)
        {
            var sharedList = _sharedListService.GetSharedList(Guid.Parse(CurrentListId));
            ViewBag.CurrentListName = sharedList?.Name ?? "Shared List";
        }
        else
        {
            ViewBag.CurrentListName = "My Grocery List";
        }

        // Get available lists for switcher
        ViewBag.SharedLists = _sharedListService.GetUserSharedLists(UserId!);

        return View(items);
    }

    public IActionResult Store()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        var existing = _groceryService.GetAll(CurrentListId!).Select(i => i.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var model = new StoreViewModel
        {
            StoreItems = GroceryList.Helpers.EmojiHelper.GetAllItems(),
            ExistingItems = existing
        };
        ViewBag.CategoryOrder = _settingsService.GetCategoryOrder(UserId!);
        ViewBag.CurrentListId = CurrentListId;
        ViewBag.IsSharedList = IsSharedList;

        // Get list name for display
        if (IsSharedList && CurrentListId != null)
        {
            var sharedList = _sharedListService.GetSharedList(Guid.Parse(CurrentListId));
            ViewBag.CurrentListName = sharedList?.Name ?? "Shared List";
        }
        else
        {
            ViewBag.CurrentListName = "My Grocery List";
        }

        // Get available lists for switcher
        ViewBag.SharedLists = _sharedListService.GetUserSharedLists(UserId!);

        return View(model);
    }

    public IActionResult Shop()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        ViewBag.CategoryOrder = _settingsService.GetCategoryOrder(UserId!);
        ViewBag.CurrentListId = CurrentListId;
        ViewBag.IsSharedList = IsSharedList;

        // Get list name for display
        if (IsSharedList && CurrentListId != null)
        {
            var sharedList = _sharedListService.GetSharedList(Guid.Parse(CurrentListId));
            ViewBag.CurrentListName = sharedList?.Name ?? "Shared List";
        }
        else
        {
            ViewBag.CurrentListName = "My Grocery List";
        }

        // Get available lists for switcher
        ViewBag.SharedLists = _sharedListService.GetUserSharedLists(UserId!);

        return View();
    }

    [HttpPost]
    public IActionResult SwitchList(string listId)
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;

        // If switching to personal list
        if (listId == UserId)
        {
            HttpContext.Session.SetString("CurrentListId", UserId!);
            return RedirectToAction(nameof(Index));
        }

        // Verify access to shared list
        var listIdGuid = Guid.Parse(listId);
        if (_sharedListService.HasAccess(listIdGuid, UserId!))
        {
            HttpContext.Session.SetString("CurrentListId", listId);
        }
        else
        {
            TempData["Error"] = "You don't have access to this shared list.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult SaveCategoryOrder([FromBody] List<string> order)
    {
        var redirect = RequireLogin();
        if (redirect != null) return Unauthorized();
        _settingsService.SaveCategoryOrder(UserId!, order);
        return Ok();
    }

    [HttpPost]
    public IActionResult AddItem([FromBody] string item)
    {
        var redirect = RequireLogin();
        if (redirect != null) return Unauthorized();
        if (!string.IsNullOrWhiteSpace(item))
            _groceryService.AddItems(CurrentListId!, item);
        return Ok();
    }

    [HttpPost]
    public IActionResult Add(string items)
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        if (!string.IsNullOrWhiteSpace(items))
            _groceryService.AddItems(CurrentListId!, items);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult UpdateCategory(Guid id, string category)
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        var items = _groceryService.GetAll(CurrentListId!);
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            item.Category = category ?? string.Empty;
            _groceryService.Save(CurrentListId!, items);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Remove(Guid id)
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        _groceryService.RemoveItem(CurrentListId!, id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Clear()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        _groceryService.ClearAll(CurrentListId!);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

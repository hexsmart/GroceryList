using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GroceryList.Models;
using GroceryList.Services;

namespace GroceryList.Controllers;

public class HomeController : Controller
{
    private readonly GroceryService _groceryService;
    private readonly SettingsService _settingsService;

    public HomeController(GroceryService groceryService, SettingsService settingsService)
    {
        _groceryService = groceryService;
        _settingsService = settingsService;
    }

    private string? UserId => HttpContext.Session.GetString("UserId");

    private IActionResult RequireLogin()
    {
        if (UserId == null) return RedirectToAction("Login", "Account");
        return null!;
    }

    public IActionResult Index()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        var items = _groceryService.GetAll(UserId!);
        ViewBag.CategoryOrder = _settingsService.GetCategoryOrder();
        return View(items);
    }

    public IActionResult Store()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        var existing = _groceryService.GetAll(UserId!).Select(i => i.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var model = new StoreViewModel
        {
            StoreItems = GroceryList.Helpers.EmojiHelper.GetAllItems(),
            ExistingItems = existing
        };
        return View(model);
    }

    public IActionResult Shop()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        ViewBag.CategoryOrder = _settingsService.GetCategoryOrder();
        return View();
    }

    [HttpPost]
    public IActionResult SaveCategoryOrder([FromBody] List<string> order)
    {
        _settingsService.SaveCategoryOrder(order);
        return Ok();
    }

    [HttpPost]
    public IActionResult AddItem([FromBody] string item)
    {
        var redirect = RequireLogin();
        if (redirect != null) return Unauthorized();
        if (!string.IsNullOrWhiteSpace(item))
            _groceryService.AddItems(UserId!, item);
        return Ok();
    }

    [HttpPost]
    public IActionResult Add(string items)
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        if (!string.IsNullOrWhiteSpace(items))
            _groceryService.AddItems(UserId!, items);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult UpdateCategory(Guid id, string category)
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        var items = _groceryService.GetAll(UserId!);
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            item.Category = category ?? string.Empty;
            _groceryService.Save(UserId!, items);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Remove(Guid id)
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        _groceryService.RemoveItem(UserId!, id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Clear()
    {
        var redirect = RequireLogin();
        if (redirect != null) return redirect;
        _groceryService.ClearAll(UserId!);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

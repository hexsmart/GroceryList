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

    public IActionResult Index()
    {
        var items = _groceryService.GetAll();
        ViewBag.CategoryOrder = _settingsService.GetCategoryOrder();
        return View(items);
    }

    public IActionResult Store()
    {
        var existing = _groceryService.GetAll().Select(i => i.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var model = new StoreViewModel
        {
            StoreItems = GroceryList.Helpers.EmojiHelper.GetAllItems(),
            ExistingItems = existing
        };
        return View(model);
    }

    public IActionResult Shop()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SaveCategoryOrder([FromBody] List<string> order)
    {
        _settingsService.SaveCategoryOrder(order);
        return Ok();
    }

    [HttpPost]
    public IActionResult Add(string items)
    {
        if (!string.IsNullOrWhiteSpace(items))
            _groceryService.AddItems(items);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult UpdateCategory(Guid id, string category)
    {
        var items = _groceryService.GetAll();
        var item = items.FirstOrDefault(i => i.Id == id);
        if (item != null)
        {
            item.Category = category ?? string.Empty;
            _groceryService.Save(items);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Remove(Guid id)
    {
        _groceryService.RemoveItem(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Clear()
    {
        _groceryService.ClearAll();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GroceryList.Models;
using GroceryList.Services;

namespace GroceryList.Controllers;

public class HomeController : Controller
{
    private readonly GroceryService _groceryService;

    public HomeController(GroceryService groceryService)
    {
        _groceryService = groceryService;
    }

    public IActionResult Index()
    {
        var items = _groceryService.GetAll();
        return View(items);
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
            item.Category = category ?? "Staple";
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

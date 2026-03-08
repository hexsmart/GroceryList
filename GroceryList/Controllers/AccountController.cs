using Microsoft.AspNetCore.Mvc;
using GroceryList.Services;

namespace GroceryList.Controllers;

public class AccountController : Controller
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string email)
    {
        var user = _userService.FindByEmail(email);
        if (user == null)
        {
            TempData["Email"] = email;
            TempData["Error"] = "No account found for that email.";
            return View();
        }
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserName", user.FirstName);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        ViewBag.Email = TempData["Email"];
        return View();
    }

    [HttpPost]
    public IActionResult Register(string firstName, string lastName, string email)
    {
        if (_userService.FindByEmail(email) != null)
        {
            ViewBag.Error = "An account with that email already exists.";
            ViewBag.Email = email;
            return View();
        }
        var user = _userService.Register(firstName, lastName, email);
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserName", user.FirstName);
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}

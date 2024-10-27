using Microsoft.AspNetCore.Mvc;
using StackWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace StackWebApp.Controllers
{
   public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                ModelState.AddModelError(string.Empty, "Email is required.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password ?? string.Empty, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }
}
}
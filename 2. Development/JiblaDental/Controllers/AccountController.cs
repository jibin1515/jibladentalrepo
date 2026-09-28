using Application.ViewModels;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    #region Constructor

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userManager = userManager;
    }

    #endregion

    //[AllowAnonymous]
    [HttpGet("/account/login")]
    public async Task<IActionResult> Index()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
        return View();
    }

    //[AllowAnonymous]
    //[ValidateAntiForgeryToken]
    [HttpPost("/account/login")]
    public async Task<IActionResult> Index(LoginViewModel loginViewModel, [FromQuery] string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View();

        var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

        try
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName, loginViewModel.Password,
                loginViewModel.RememberMe, true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in");
                if (returnUrl != null) return LocalRedirect(returnUrl);
                return RedirectToAction("Index", "Admin");
            }

            ModelState.AddModelError("error", result.ToString());
        }
        catch (Exception)
        {
            ModelState.AddModelError("error", "The provided username or password is incorrect.");
        }

        return View();
    }

    [Authorize]
    [HttpGet("/account/check")]
    public async Task<string> Check()
    {
        return (await _userManager.GetUserAsync(User)).Id.ToString();
    }

    [HttpGet("/account/logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
        return RedirectToAction("index");
    }
}
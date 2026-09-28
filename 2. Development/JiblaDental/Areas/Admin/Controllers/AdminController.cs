using Application.ViewModels;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JiblaDental.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }


    [HttpGet("/admin")]
    public IActionResult Index()
    {
        return View();
    }


    [HttpGet("/admin/account")]
    public async Task<IActionResult> Get()
    {
        var model = new AccountSettingsViewModel();
        var user = await _userManager.GetUserAsync(User);
        //model.UserId = user.Id;
        model.Email = user.Email;
        return View(model);
    }

    [HttpPost("/admin/account")]
    public async Task<IActionResult> Post(AccountSettingsViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);

        if (string.IsNullOrEmpty(model.Email))
        {
            ModelState.AddModelError("invalid", "Please provide an email address.");
            return View("Get", model);
        }

        user.Email = model.Email;
        await _userManager.UpdateAsync(user);

        if (!string.IsNullOrWhiteSpace(model.OldPassword) && !string.IsNullOrWhiteSpace(model.Password) &&
            !string.IsNullOrWhiteSpace(model.CPassword))
        {
            if (model.Password != model.CPassword)
            {
                ModelState.AddModelError("invalid", "Passwords do not match.");
            }
            else
            {
                if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                    ModelState.AddModelError("invalid", "Password changed successfully.");
                }
                else
                {
                    ModelState.AddModelError("invalid", "Current password is incorrect. Please try again.");
                }
            }
        }
        else
        {
            ModelState.AddModelError("invalid", "Please enter data to update.");
        }

        var entity = new AccountSettingsViewModel
        {
            Email = user.Email,
            OldPassword = string.Empty,
            Password = string.Empty,
            CPassword = string.Empty
        };
        return View("Get", entity);
    }
}
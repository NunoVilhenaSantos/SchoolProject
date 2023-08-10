// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class ResetAuthenticatorModel : PageModel
{
    private readonly ILogger<ResetAuthenticatorModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public ResetAuthenticatorModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<ResetAuthenticatorModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        await _userManager.SetTwoFactorEnabledAsync(user: user, enabled: false);
        await _userManager.ResetAuthenticatorKeyAsync(user: user);
        var userId = await _userManager.GetUserIdAsync(user: user);
        _logger.LogInformation(
            message: "User with ID '{UserId}' has reset their authentication app key.",
            args: user.Id);

        await _signInManager.RefreshSignInAsync(user: user);
        StatusMessage =
            "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

        return RedirectToPage(pageName: "./EnableAuthenticator");
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class GenerateRecoveryCodesModel : PageModel
{
    private readonly ILogger<GenerateRecoveryCodesModel> _logger;
    private readonly UserManager<User> _userManager;

    public GenerateRecoveryCodesModel(
        UserManager<User> userManager,
        ILogger<GenerateRecoveryCodesModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string[] RecoveryCodes { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        var isTwoFactorEnabled =
            await _userManager.GetTwoFactorEnabledAsync(user: user);
        if (!isTwoFactorEnabled)
            throw new InvalidOperationException(
                message: "Cannot generate recovery codes for user because they do not have 2FA enabled.");

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        var isTwoFactorEnabled =
            await _userManager.GetTwoFactorEnabledAsync(user: user);
        var userId = await _userManager.GetUserIdAsync(user: user);
        if (!isTwoFactorEnabled)
            throw new InvalidOperationException(
                message: "Cannot generate recovery codes for user as they do not have 2FA enabled.");

        var recoveryCodes =
            await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user: user, number: 10);
        RecoveryCodes = recoveryCodes.ToArray();

        _logger.LogInformation(
            message: "User with ID '{UserId}' has generated new 2FA recovery codes.",
            args: userId);
        StatusMessage = "You have generated new recovery codes.";
        return RedirectToPage(pageName: "./ShowRecoveryCodes");
    }
}
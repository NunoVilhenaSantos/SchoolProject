// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class TwoFactorAuthenticationModel : PageModel
{
    private readonly ILogger<TwoFactorAuthenticationModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public TwoFactorAuthenticationModel(
        UserManager<User> userManager, SignInManager<User> signInManager,
        ILogger<TwoFactorAuthenticationModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool HasAuthenticator { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public int RecoveryCodesLeft { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public bool Is2faEnabled { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool IsMachineRemembered { get; set; }

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

        HasAuthenticator =
            await _userManager.GetAuthenticatorKeyAsync(user: user) != null;
        Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user: user);
        IsMachineRemembered =
            await _signInManager.IsTwoFactorClientRememberedAsync(user: user);
        RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user: user);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        await _signInManager.ForgetTwoFactorClientAsync();
        StatusMessage =
            "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
        return RedirectToPage();
    }
}
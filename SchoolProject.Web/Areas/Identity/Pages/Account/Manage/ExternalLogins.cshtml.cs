// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class ExternalLoginsModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;

    public ExternalLoginsModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IUserStore<User> userStore)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userStore = userStore;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<UserLoginInfo> CurrentLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme> OtherLogins { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool ShowRemoveButton { get; set; }

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

        CurrentLogins = await _userManager.GetLoginsAsync(user: user);
        OtherLogins =
            (await _signInManager.GetExternalAuthenticationSchemesAsync())
            .Where(predicate: auth =>
                CurrentLogins.All(predicate: ul => auth.Name != ul.LoginProvider))
            .ToList();

        string passwordHash = null;
        if (_userStore is IUserPasswordStore<User> userPasswordStore)
            passwordHash =
                await userPasswordStore.GetPasswordHashAsync(user: user,
                    cancellationToken: HttpContext.RequestAborted);

        ShowRemoveButton = passwordHash != null || CurrentLogins.Count > 1;
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveLoginAsync(
        string loginProvider, string providerKey)
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        var result =
            await _userManager.RemoveLoginAsync(user: user, loginProvider: loginProvider,
                providerKey: providerKey);
        if (!result.Succeeded)
        {
            StatusMessage = "The external login was not removed.";
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync(user: user);
        StatusMessage = "The external login was removed.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostLinkLoginAsync(string provider)
    {
        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(scheme: IdentityConstants.ExternalScheme);

        // Request a redirect to the external login provider to link a login for the current user
        var redirectUrl = Url.Page(pageName: "./ExternalLogins", pageHandler: "LinkLoginCallback");
        var properties =
            _signInManager.ConfigureExternalAuthenticationProperties(provider: provider,
                redirectUrl: redirectUrl, userId: _userManager.GetUserId(principal: User));
        return new ChallengeResult(authenticationScheme: provider, properties: properties);
    }

    public async Task<IActionResult> OnGetLinkLoginCallbackAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        var userId = await _userManager.GetUserIdAsync(user: user);
        var info = await _signInManager.GetExternalLoginInfoAsync(expectedXsrf: userId);
        if (info == null)
            throw new InvalidOperationException(
                message: "Unexpected error occurred loading external login info.");

        var result = await _userManager.AddLoginAsync(user: user, login: info);
        if (!result.Succeeded)
        {
            StatusMessage =
                "The external login was not added. External logins can only be associated with one account.";
            return RedirectToPage();
        }

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(scheme: IdentityConstants.ExternalScheme);

        StatusMessage = "The external login was added.";
        return RedirectToPage();
    }
}
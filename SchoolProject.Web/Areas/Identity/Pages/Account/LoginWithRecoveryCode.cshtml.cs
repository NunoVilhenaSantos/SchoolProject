// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Areas.Identity.Pages.Account;

public class LoginWithRecoveryCodeModel : PageModel
{
    private readonly ILogger<LoginWithRecoveryCodeModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public LoginWithRecoveryCodeModel(
        ILogger<LoginWithRecoveryCodeModel> logger,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager
    )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(string returnUrl = null)
    {
        // Ensure the appUser has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync() ??
                   throw new InvalidOperationException(
                       "Unable to load two-factor authentication appUser.");
        ReturnUrl = returnUrl;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync() ??
                   throw new InvalidOperationException(
                       "Unable to load two-factor authentication appUser.");

        var recoveryCode =
            Input.RecoveryCode.Replace(" ", string.Empty);

        var result =
            await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

        var userId = await _userManager.GetUserIdAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation(
                "AppUser with ID '{UserId}' logged in with a recovery code.",
                user.Id);
            return LocalRedirect(returnUrl ?? Url.Content("~/"));
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("AppUser account locked out.");
            return RedirectToPage("./Lockout");
        }

        _logger.LogWarning(
            "Invalid recovery code entered for appUser with ID '{UserId}' ",
            user.Id);
        ModelState.AddModelError(string.Empty,
            "Invalid recovery code entered.");
        return Page();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure
        ///     and is not intended to be used directly from your code.
        ///     This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; }
    }
}
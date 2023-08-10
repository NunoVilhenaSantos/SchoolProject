// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account;

public class LoginWithRecoveryCodeModel : PageModel
{
    private readonly ILogger<LoginWithRecoveryCodeModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public LoginWithRecoveryCodeModel(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ILogger<LoginWithRecoveryCodeModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(string returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
            throw new InvalidOperationException(
                message: "Unable to load two-factor authentication user.");

        ReturnUrl = returnUrl;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
            throw new InvalidOperationException(
                message: "Unable to load two-factor authentication user.");

        var recoveryCode = Input.RecoveryCode.Replace(oldValue: " ", newValue: string.Empty);

        var result =
            await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode: recoveryCode);

        var userId = await _userManager.GetUserIdAsync(user: user);

        if (result.Succeeded)
        {
            _logger.LogInformation(
                message: "User with ID '{UserId}' logged in with a recovery code.",
                args: user.Id);
            return LocalRedirect(localUrl: returnUrl ?? Url.Content(contentPath: "~/"));
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning(message: "User account locked out.");
            return RedirectToPage(pageName: "./Lockout");
        }

        _logger.LogWarning(
            message: "Invalid recovery code entered for user with ID '{UserId}' ",
            args: user.Id);
        ModelState.AddModelError(key: string.Empty,
            errorMessage: "Invalid recovery code entered.");
        return Page();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        [Required]
        [DataType(dataType: DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; }
    }
}
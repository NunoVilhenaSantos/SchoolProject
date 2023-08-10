// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account;

public class LoginWith2faModel : PageModel
{
    private readonly ILogger<LoginWith2faModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public LoginWith2faModel(
        SignInManager<User> signInManager,
        UserManager<User> userManager,
        ILogger<LoginWith2faModel> logger)
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
    public bool RememberMe { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string ReturnUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(bool rememberMe,
        string returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null)
            throw new InvalidOperationException(
                message: "Unable to load two-factor authentication user.");

        ReturnUrl = returnUrl;
        RememberMe = rememberMe;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(bool rememberMe,
        string returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();

        returnUrl = returnUrl ?? Url.Content(contentPath: "~/");

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null)
            throw new InvalidOperationException(
                message: "Unable to load two-factor authentication user.");

        var authenticatorCode = Input.TwoFactorCode.Replace(oldValue: " ", newValue: string.Empty)
            .Replace(oldValue: "-", newValue: string.Empty);

        var result =
            await _signInManager.TwoFactorAuthenticatorSignInAsync(
                code: authenticatorCode, isPersistent: rememberMe, rememberClient: Input.RememberMachine);

        var userId = await _userManager.GetUserIdAsync(user: user);

        if (result.Succeeded)
        {
            _logger.LogInformation(
                message: "User with ID '{UserId}' logged in with 2fa.", args: user.Id);
            return LocalRedirect(localUrl: returnUrl);
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning(message: "User with ID '{UserId}' account locked out.",
                args: user.Id);
            return RedirectToPage(pageName: "./Lockout");
        }

        _logger.LogWarning(
            message: "Invalid authenticator code entered for user with ID '{UserId}'.",
            args: user.Id);
        ModelState.AddModelError(key: string.Empty, errorMessage: "Invalid authenticator code.");
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
        [Required]
        [StringLength(maximumLength: 7,
            ErrorMessage =
                "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(dataType: DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class EnableAuthenticatorModel : PageModel
{
    private const string AuthenticatorUriFormat =
        "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

    private readonly ILogger<EnableAuthenticatorModel> _logger;
    private readonly UrlEncoder _urlEncoder;
    private readonly UserManager<User> _userManager;

    public EnableAuthenticatorModel(
        UserManager<User> userManager,
        ILogger<EnableAuthenticatorModel> logger,
        UrlEncoder urlEncoder)
    {
        _userManager = userManager;
        _logger = logger;
        _urlEncoder = urlEncoder;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string SharedKey { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string AuthenticatorUri { get; set; }

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

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        await LoadSharedKeyAndQrCodeUriAsync(user: user);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadSharedKeyAndQrCodeUriAsync(user: user);
            return Page();
        }

        // Strip spaces and hyphens
        var verificationCode = Input.Code.Replace(oldValue: " ", newValue: string.Empty)
            .Replace(oldValue: "-", newValue: string.Empty);

        var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(
            user: user, tokenProvider: _userManager.Options.Tokens.AuthenticatorTokenProvider,
            token: verificationCode);

        if (!is2faTokenValid)
        {
            ModelState.AddModelError(key: "Input.Code",
                errorMessage: "Verification code is invalid.");
            await LoadSharedKeyAndQrCodeUriAsync(user: user);
            return Page();
        }

        await _userManager.SetTwoFactorEnabledAsync(user: user, enabled: true);
        var userId = await _userManager.GetUserIdAsync(user: user);
        _logger.LogInformation(
            message: "User with ID '{UserId}' has enabled 2FA with an authenticator app.",
            args: userId);

        StatusMessage = "Your authenticator app has been verified.";

        if (await _userManager.CountRecoveryCodesAsync(user: user) == 0)
        {
            var recoveryCodes =
                await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user: user,
                    number: 10);
            RecoveryCodes = recoveryCodes.ToArray();
            return RedirectToPage(pageName: "./ShowRecoveryCodes");
        }

        return RedirectToPage(pageName: "./TwoFactorAuthentication");
    }

    private async Task LoadSharedKeyAndQrCodeUriAsync(User user)
    {
        // Load the authenticator key & QR code URI to display on the form
        var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user: user);
        if (string.IsNullOrEmpty(value: unformattedKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user: user);
            unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user: user);
        }

        SharedKey = FormatKey(unformattedKey: unformattedKey);

        var email = await _userManager.GetEmailAsync(user: user);
        AuthenticatorUri = GenerateQrCodeUri(email: email, unformattedKey: unformattedKey);
    }

    private string FormatKey(string unformattedKey)
    {
        var result = new StringBuilder();
        var currentPosition = 0;
        while (currentPosition + 4 < unformattedKey.Length)
        {
            result.Append(value: unformattedKey.AsSpan(start: currentPosition, length: 4))
                .Append(value: ' ');
            currentPosition += 4;
        }

        if (currentPosition < unformattedKey.Length)
            result.Append(value: unformattedKey.AsSpan(start: currentPosition));

        return result.ToString().ToLowerInvariant();
    }

    private string GenerateQrCodeUri(string email, string unformattedKey)
    {
        return string.Format(
            provider: CultureInfo.InvariantCulture,
            format: AuthenticatorUriFormat,
            arg0: _urlEncoder.Encode(value: "Microsoft.AspNetCore.Identity.UI"),
            arg1: _urlEncoder.Encode(value: email),
            arg2: unformattedKey);
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
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class EmailModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public EmailModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public bool IsEmailConfirmed { get; set; }

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

    private async Task LoadAsync(User user)
    {
        var email = await _userManager.GetEmailAsync(user: user);
        Email = email;

        Input = new InputModel
        {
            NewEmail = email
        };

        IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user: user);
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        await LoadAsync(user: user);
        return Page();
    }

    public async Task<IActionResult> OnPostChangeEmailAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user: user);
            return Page();
        }

        var email = await _userManager.GetEmailAsync(user: user);
        if (Input.NewEmail != email)
        {
            var userId = await _userManager.GetUserIdAsync(user: user);
            var code =
                await _userManager.GenerateChangeEmailTokenAsync(user: user,
                    newEmail: Input.NewEmail);
            code = WebEncoders.Base64UrlEncode(input: Encoding.UTF8.GetBytes(s: code));
            var callbackUrl = Url.Page(
                pageName: "/Account/ConfirmEmailChange",
                pageHandler: null,
                values: new {area = "Identity", userId, email = Input.NewEmail, code},
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email: Input.NewEmail,
                subject: "Confirm your email",
                htmlMessage: $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(value: callbackUrl)}'>clicking here</a>.");

            StatusMessage =
                "Confirmation link to change email sent. Please check your email.";
            return RedirectToPage();
        }

        StatusMessage = "Your email is unchanged.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user: user);
            return Page();
        }

        var userId = await _userManager.GetUserIdAsync(user: user);
        var email = await _userManager.GetEmailAsync(user: user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user: user);
        code = WebEncoders.Base64UrlEncode(input: Encoding.UTF8.GetBytes(s: code));
        var callbackUrl = Url.Page(
            pageName: "/Account/ConfirmEmail",
            pageHandler: null,
            values: new {area = "Identity", userId, code},
            protocol: Request.Scheme);
        await _emailSender.SendEmailAsync(
            email: email,
            subject: "Confirm your email",
            htmlMessage: $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(value: callbackUrl)}'>clicking here</a>.");

        StatusMessage = "Verification email sent. Please check your email.";
        return RedirectToPage();
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
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ResendEmailConfirmationModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<User> _userManager;

    public ResendEmailConfirmationModel(
        UserManager<User> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.FindByEmailAsync(email: Input.Email);
        if (user == null)
        {
            ModelState.AddModelError(key: string.Empty,
                errorMessage: "Verification email sent. Please check your email.");
            return Page();
        }

        var userId = await _userManager.GetUserIdAsync(user: user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user: user);
        code = WebEncoders.Base64UrlEncode(input: Encoding.UTF8.GetBytes(s: code));
        var callbackUrl = Url.Page(
            pageName: "/Account/ConfirmEmail",
            pageHandler: null,
            values: new {userId, code},
            protocol: Request.Scheme);
        await _emailSender.SendEmailAsync(
            email: Input.Email,
            subject: "Confirm your email",
            htmlMessage: $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(value: callbackUrl)}'>clicking here</a>.");

        ModelState.AddModelError(key: string.Empty,
            errorMessage: "Verification email sent. Please check your email.");
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
        [EmailAddress]
        public string Email { get; set; }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    private readonly IEmailSender _sender;
    private readonly UserManager<User> _userManager;

    public RegisterConfirmationModel(UserManager<User> userManager,
        IEmailSender sender)
    {
        _userManager = userManager;
        _sender = sender;
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
    public bool DisplayConfirmAccountLink { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string EmailConfirmationUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(string email,
        string returnUrl = null)
    {
        if (email == null) return RedirectToPage(pageName: "/Index");
        returnUrl = returnUrl ?? Url.Content(contentPath: "~/");

        var user = await _userManager.FindByEmailAsync(email: email);
        if (user == null)
            return NotFound(value: $"Unable to load user with email '{email}'.");

        Email = email;
        // Once you add a real email sender, you should remove this code that lets you confirm the account
        DisplayConfirmAccountLink = true;
        if (DisplayConfirmAccountLink)
        {
            var userId = await _userManager.GetUserIdAsync(user: user);
            var code =
                await _userManager.GenerateEmailConfirmationTokenAsync(user: user);
            code = WebEncoders.Base64UrlEncode(input: Encoding.UTF8.GetBytes(s: code));
            EmailConfirmationUrl = Url.Page(
                pageName: "/Account/ConfirmEmail",
                pageHandler: null,
                values: new {area = "Identity", userId, code, returnUrl},
                protocol: Request.Scheme);
        }

        return Page();
    }
}
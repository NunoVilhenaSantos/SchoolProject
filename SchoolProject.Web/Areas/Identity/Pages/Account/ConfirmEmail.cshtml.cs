// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account;

public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<User> _userManager;

    public ConfirmEmailModel(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        if (userId == null || code == null) return RedirectToPage(pageName: "/Index");

        var user = await _userManager.FindByIdAsync(userId: userId);
        if (user == null)
            return NotFound(value: $"Unable to load user with ID '{userId}'.");

        code = Encoding.UTF8.GetString(bytes: WebEncoders.Base64UrlDecode(input: code));
        var result = await _userManager.ConfirmEmailAsync(user: user, token: code);
        StatusMessage = result.Succeeded
            ? "Thank you for confirming your email."
            : "Error confirming your email.";
        return Page();
    }
}
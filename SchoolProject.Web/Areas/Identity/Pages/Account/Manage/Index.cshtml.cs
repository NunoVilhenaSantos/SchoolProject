// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public IndexModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string Username { get; set; }

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
        var userName = await _userManager.GetUserNameAsync(user: user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user: user);

        Username = userName;

        Input = new InputModel
        {
            PhoneNumber = phoneNumber
        };
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

    public async Task<IActionResult> OnPostAsync()
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

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user: user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult =
                await _userManager.SetPhoneNumberAsync(user: user, phoneNumber: Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage =
                    "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        await _signInManager.RefreshSignInAsync(user: user);
        StatusMessage = "Your profile has been updated";
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
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
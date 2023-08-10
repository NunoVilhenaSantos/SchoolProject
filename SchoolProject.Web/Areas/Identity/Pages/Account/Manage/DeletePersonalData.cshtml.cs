// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class DeletePersonalDataModel : PageModel
{
    private readonly ILogger<DeletePersonalDataModel> _logger;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public DeletePersonalDataModel(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<DeletePersonalDataModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
    public bool RequirePassword { get; set; }

    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        RequirePassword = await _userManager.HasPasswordAsync(user: user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        RequirePassword = await _userManager.HasPasswordAsync(user: user);
        if (RequirePassword)
            if (!await _userManager.CheckPasswordAsync(user: user, password: Input.Password))
            {
                ModelState.AddModelError(key: string.Empty, errorMessage: "Incorrect password.");
                return Page();
            }

        var result = await _userManager.DeleteAsync(user: user);
        var userId = await _userManager.GetUserIdAsync(user: user);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                message: "Unexpected error occurred deleting user.");

        await _signInManager.SignOutAsync();

        _logger.LogInformation(message: "User with ID '{UserId}' deleted themselves.",
            args: userId);

        return Redirect(url: "~/");
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
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }
    }
}
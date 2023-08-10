// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account;

public class ResetPasswordModel : PageModel
{
    private readonly UserManager<User> _userManager;

    public ResetPasswordModel(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    public IActionResult OnGet(string code = null)
    {
        if (code == null)
            return BadRequest(error: "A code must be supplied for password reset.");

        Input = new InputModel
        {
            Code = Encoding.UTF8.GetString(bytes: WebEncoders.Base64UrlDecode(input: code))
        };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.FindByEmailAsync(email: Input.Email);
        if (user == null)
            // Don't reveal that the user does not exist
            return RedirectToPage(pageName: "./ResetPasswordConfirmation");

        var result =
            await _userManager.ResetPasswordAsync(user: user, token: Input.Code,
                newPassword: Input.Password);
        if (result.Succeeded)
            return RedirectToPage(pageName: "./ResetPasswordConfirmation");

        foreach (var error in result.Errors)
            ModelState.AddModelError(key: string.Empty, errorMessage: error.Description);
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

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        [StringLength(maximumLength: 100,
            ErrorMessage =
                "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(dataType: DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(dataType: DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare(otherProperty: "Password",
            ErrorMessage =
                "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required]
        public string Code { get; set; }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

public class DownloadPersonalDataModel : PageModel
{
    private readonly ILogger<DownloadPersonalDataModel> _logger;
    private readonly UserManager<User> _userManager;

    public DownloadPersonalDataModel(
        UserManager<User> userManager,
        ILogger<DownloadPersonalDataModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        return NotFound();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(principal: User);
        if (user == null)
            return NotFound(
                value: $"Unable to load user with ID '{_userManager.GetUserId(principal: User)}'.");

        _logger.LogInformation(
            message: "User with ID '{UserId}' asked for their personal data.",
            args: _userManager.GetUserId(principal: User));

        // Only include personal data for download
        var personalData = new Dictionary<string, string>();
        var personalDataProps = typeof(User).GetProperties().Where(
            predicate: prop => Attribute.IsDefined(element: prop, attributeType: typeof(PersonalDataAttribute)));
        foreach (var p in personalDataProps)
            personalData.Add(key: p.Name, value: p.GetValue(obj: user)?.ToString() ?? "null");

        var logins = await _userManager.GetLoginsAsync(user: user);
        foreach (var l in logins)
            personalData.Add(key: $"{l.LoginProvider} external login provider key",
                value: l.ProviderKey);

        personalData.Add(key: "Authenticator Key",
            value: await _userManager.GetAuthenticatorKeyAsync(user: user));

        Response.Headers.Add(key: "Content-Disposition",
            value: "attachment; filename=PersonalData.json");
        return new FileContentResult(
            fileContents: JsonSerializer.SerializeToUtf8Bytes(value: personalData),
            contentType: "application/json");
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolProject.Web.Areas.Identity.Pages.Account.Manage;

/// <summary>
///     This API supports the ASP.NET Core Identity default UI infrastructure
///     and is not intended to be used directly from your code.
///     This API may change or be removed in future releases.
/// </summary>
public static class ManageNavPages
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string Index => "Index";


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string Email => "Email";


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string ChangePassword => "ChangePassword";


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string DownloadPersonalData => "DownloadPersonalData";


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string DeletePersonalData => "DeletePersonalData";


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string ExternalLogins => "ExternalLogins";


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string PersonalData => "PersonalData";


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string TwoFactorAuthentication => "TwoFactorAuthentication";


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string IndexNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext: viewContext, page: Index);
    }


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string EmailNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext: viewContext, page: Email);
    }


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string ChangePasswordNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext: viewContext, page: ChangePassword);
    }


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string DownloadPersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext: viewContext, page: DownloadPersonalData);
    }


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string DeletePersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext: viewContext, page: DeletePersonalData);
    }


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string ExternalLoginsNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext: viewContext, page: ExternalLogins);
    }


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string PersonalDataNavClass(ViewContext viewContext)
    {
        return PageNavClass(viewContext: viewContext, page: PersonalData);
    }


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string TwoFactorAuthenticationNavClass(
        ViewContext viewContext)
    {
        return PageNavClass(viewContext: viewContext, page: TwoFactorAuthentication);
    }


    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure
    ///     and is not intended to be used directly from your code.
    ///     This API may change or be removed in future releases.
    /// </summary>
    public static string PageNavClass(ViewContext viewContext, string page)
    {
        var activePage =
            viewContext.ViewData[index: "ActivePage"] as string ??
            Path.GetFileNameWithoutExtension(path: viewContext
                .ActionDescriptor.DisplayName);

        return string.Equals(a: activePage, b: page,
            comparisonType: StringComparison.OrdinalIgnoreCase)
            ? "active"
            : null;
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Account;

/// <summary>
///     view model class to change appUser data
/// </summary>
public class ChangeAppUserViewModel : AppUser
{
    /// <summary>
    ///     The list of countries for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? CountriesListItems { get; set; }


    /// <summary>
    ///     The list of cities for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? CitiesListItems { get; set; }
}
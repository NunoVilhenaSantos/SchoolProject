using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Users;

public class ChangeUserViewModel : User
{
    /// <summary>
    ///     The list of countries for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? Countries { get; set; }


    /// <summary>
    ///     The country of the user.
    /// </summary>
    [Display(Name = "Country")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country...")]
    public required int CountryId { get; set; }


    /// <summary>
    ///     The list of cities for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? Cities { get; set; }


    /// <summary>
    ///     The city of the user.
    /// </summary>
    [Display(Name = "City")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a city...")]
    public required int CityId { get; set; }
}
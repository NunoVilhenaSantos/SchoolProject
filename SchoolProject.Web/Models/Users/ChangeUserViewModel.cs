using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Users;

public class ChangeUserViewModel
{
    /// <summary>
    ///     User class to be used for the view.
    ///     It is used to get the user's data to create a new user.
    /// </summary>
    public User user { get; set; }


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


    /// <summary>
    ///     The nationality of the user.
    /// </summary>
    //[Display(Name = "Nationality")]
    //[Range(1, int.MaxValue, ErrorMessage = "You must select a country...")]
    //public required int NationalityId { get; set; }


    /// <summary>
    ///     The list of 1 nationality for selection.
    /// </summary>
    //public IEnumerable<SelectListItem> Nationalities { get; set; }
}
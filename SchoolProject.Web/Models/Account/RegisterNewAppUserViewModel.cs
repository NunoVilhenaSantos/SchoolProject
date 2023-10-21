using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Account;

/// <summary>
///     view model class to register a new appUser using the the main class appUser
/// </summary>
public class RegisterNewAppUserViewModel : AppUser
{
    /// <summary>
    ///     The country of the appUser.
    /// </summary>
    [Required]
    [Display(Name = "Country")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country...")]
    public required int CountryId { get; set; }


    /// <summary>
    ///     The list of countries for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? Countries { get; set; }


    /// <summary>
    ///     The city of the appUser.
    /// </summary>
    [Required]
    [Display(Name = "City")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a city...")]
    public required int CityId { get; set; }


    /// <summary>
    ///     The list of cities for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? Cities { get; set; }


    /// <summary>
    ///     The password of the appUser.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public required string Password { get; set; }


    /// <summary>
    ///     The confirm password of the appUser.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [MinLength(6)]
    public required string ConfirmPassword { get; set; }
}
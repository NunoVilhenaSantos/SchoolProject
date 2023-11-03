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
    ///     The list of countries for selection.
    /// </summary>
    public required IEnumerable<SelectListItem>? CountriesList { get; set; }

    public required int? CountryId { get; set; }


    /// <summary>
    ///     The list of cities for selection.
    /// </summary>
    public required IEnumerable<SelectListItem>? CitiesList { get; set; }

    public required int? CityId { get; set; }


    /// <summary>
    ///     The list of countries for selection.
    /// </summary>
    public required IEnumerable<SelectListItem>? RolesList { get; set; }

    public required string? RoleId { get; set; }


    /// <summary>
    ///     The list of cities for selection.
    /// </summary>
    public required IEnumerable<SelectListItem>? GendersList { get; set; }

    public required int? GenderId { get; set; }


    /// <summary>
    ///     The password of the appUser.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    // [MinLength(6)]
    [MinLength(6,
        ErrorMessage = "The field {0} only can contain {1} characters length.")]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*\W).*$",
        ErrorMessage =
            "A password must contain at least one number, one uppercase letter, " +
            "one lowercase letter, and one special character.")]
    public required string Password { get; set; }


    /// <summary>
    ///     The confirm password of the appUser.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password")]
    [MinLength(6)]
    public required string ConfirmPassword { get; set; }


    /// <summary>
    /// </summary>
    [EmailAddress]
    [Required]
    public required string Email { get; set; }
}
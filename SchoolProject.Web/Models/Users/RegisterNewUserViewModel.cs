using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Users;

public class RegisterNewUserViewModel
{
    /// <summary>
    ///     The first name of the user.
    /// </summary>
    [Required]
    [DisplayName("First Name")]
    public required string FirstName { get; set; }


    /// <summary>
    ///     The last name of the user.
    /// </summary>
    [Required]
    [DisplayName("Last Name")]
    public required string LastName { get; set; }


    /// <summary>
    ///     The username for the user.
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Username { get; set; }


    /// <summary>
    ///     The address of the user.
    /// </summary>
    [MaxLength(100,
        ErrorMessage =
            "The field {0} can only contain {1} characters in lenght.")]
    public string? Address { get; set; }


    /// <summary>
    ///     The phone number of the user.
    /// </summary>
    [MaxLength(20,
        ErrorMessage =
            "The field {0} can only contain {1} characters in lenght.")]
    public string? PhoneNumber { get; set; }


    /// <summary>
    ///     The city of the user.
    /// </summary>
    [Display(Name = "City")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
    public int CityId { get; set; }


    /// <summary>
    ///     The list of cities for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? Cities { get; set; }


    /// <summary>
    ///     The country of the user.
    /// </summary>
    [Display(Name = "Country")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
    public int CountryId { get; set; }


    /// <summary>
    ///     The list of countries for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? Countries { get; set; }


    /// <summary>
    ///     The password of the user.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public required string Password { get; set; }


    /// <summary>
    ///     The confirm password of the user.
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [MinLength(6)]
    public required string ConfirmPassword { get; set; }
}
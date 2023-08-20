using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Models.Users;

public class RegisterNewUserViewModel : User
{
    /// <summary>
    ///  User class to be used for the view.
    ///  It is used to get the user's data to create a new user.
    /// </summary>
    //public User user { get; set; }


    /// <summary>
    ///     The first name of the user.
    /// </summary>
    //[Required]
    //[DisplayName("First Name")]
    //public required string FirstName { get; set; }


    /// <summary>
    ///     The last name of the user.
    /// </summary>
    //[Required]
    //[DisplayName("Last Name")]
    //public required string LastName { get; set; }


    /// <summary>
    /// </summary>
    //[Required]
    //[DataType(DataType.EmailAddress)]
    //public required string UserName { get; set; }


    /// <summary>
    ///     The address of the user.
    /// </summary>
    //[MaxLength(100,
    //    ErrorMessage =
    //        "The field {0} can only contain {1} characters in length.")]
    //public string? Address { get; set; }


    /// <summary>
    ///     The phone number of the user.
    /// </summary>
    //[MaxLength(20,
    //    ErrorMessage =
    //        "The field {0} can only contain {1} characters in length.")]
    //public string? PhoneNumber { get; set; }


    /// <summary>
    ///     The country of the user.
    /// </summary>
    [Display(Name = "Country")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country...")]
    public required int CountryId { get; set; }


    /// <summary>
    ///     The list of countries for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? Countries { get; set; }


    /// <summary>
    ///     The city of the user.
    /// </summary>
    [Display(Name = "City")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a city...")]
    public required int CityId { get; set; }


    /// <summary>
    ///     The list of cities for selection.
    /// </summary>
    public IEnumerable<SelectListItem>? Cities { get; set; }


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
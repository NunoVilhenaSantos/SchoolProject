using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolProject.Web.Models.Users;

public class RegisterNewUserViewModel
{
    [Required] [DisplayName("First Name")] public string FirstName { get; set; }


    [Required] [DisplayName("Last Name")] public string LastName { get; set; }


    [Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }


    [MaxLength(100,
        ErrorMessage =
            "The field {0} can only contain {1} characters in lenght.")]
    public string Address { get; set; }


    [MaxLength(20,
        ErrorMessage =
            "The field {0} can only contain {1} characters in lenght.")]
    public string PhoneNumber { get; set; }


    [Display(Name = "City")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
    public int CityId { get; set; }


    public IEnumerable<SelectListItem> Cities { get; set; }


    [Display(Name = "Country")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
    public int CountryId { get; set; }


    public IEnumerable<SelectListItem> Countries { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [MinLength(6)]
    public string Password { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    [MinLength(6)]
    public string ConfirmPassword { get; set; }
}
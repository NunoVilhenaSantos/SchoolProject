using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolProject.Web.Models.Users;

public class ChangeUserViewModel
{
    [Microsoft.Build.Framework.Required]
    [DisplayName(displayName: "First Name")]
    public string FirstName { get; set; }


    [Microsoft.Build.Framework.Required]
    [DisplayName(displayName: "Last Name")]
    public string LastName { get; set; }


    [Microsoft.Build.Framework.Required]
    [DataType(dataType: DataType.EmailAddress)]
    public string Username { get; set; }


    [MaxLength(length: 100,
        ErrorMessage =
            "The field {0} can only contain {1} characters in lenght.")]
    public string Address { get; set; }


    [MaxLength(length: 20,
        ErrorMessage =
            "The field {0} can only contain {1} characters in lenght.")]
    public string PhoneNumber { get; set; }


    [DisplayName(displayName: "City")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "You must select a city.")]
    public int CityId { get; set; }


    public IEnumerable<SelectListItem> Cities { get; set; }


    [DisplayName(displayName: "Country")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "You must select a country")]
    public int CountryId { get; set; }


    public IEnumerable<SelectListItem> Countries { get; set; }
}
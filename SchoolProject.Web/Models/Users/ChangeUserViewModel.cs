using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Users;

public class ChangeUserViewModel
{
    [Microsoft.Build.Framework.Required]
    [DisplayName("First Name")]
    public string FirstName { get; set; }


    [Microsoft.Build.Framework.Required]
    [DisplayName("Last Name")]
    public string LastName { get; set; }


    [Microsoft.Build.Framework.Required]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; }


    [MaxLength(100,
        ErrorMessage =
            "The field {0} can only contain {1} characters in lenght.")]
    public string Address { get; set; }


    [MaxLength(20,
        ErrorMessage =
            "The field {0} can only contain {1} characters in lenght.")]
    public string? PhoneNumber { get; set; }


    [DisplayName("City")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
    public int CityId { get; set; }


    public IEnumerable<SelectListItem> Cities { get; set; }


    [DisplayName("Country")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country")]
    public int CountryId { get; set; }


    public IEnumerable<SelectListItem> Countries { get; set; }


    [DisplayName("Nationality")]
    public int NationalityId => Convert.ToInt32(
        Countries.FirstOrDefault(c => c.Selected)?.Value);
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Countries;

public class CityViewModel
{
    public int CountryId { get; set; }

    public string CountryName { get; set; }


    public int CityId { get; set; }


    [Required]
    [DisplayName(displayName: "City")]
    [MaxLength(length: 50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public string Name { get; set; }
}
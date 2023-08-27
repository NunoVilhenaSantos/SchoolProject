using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Countries;

public class CityViewModel
{
    [Required] public required int CountryId { get; set; }

    public string CountryName { get; set; }


    [Required] public required int CityId { get; set; }


    [Required]
    [DisplayName("City")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }

    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}
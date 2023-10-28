using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolProject.Web.Data.Entities.Countries;

namespace SchoolProject.Web.Models.Countries;

/// <summary>
///
/// </summary>
public class CountryViewModel : Country
{
    /// <summary>
    ///     stores the country id though the country name.
    /// </summary>
    [Required]
    [DisplayName("Country")]
    public required int CountryId { get; set; }

    /// <summary>
    ///     stores the country name though the country id.
    /// </summary>
    [DisplayName("Country Name")]
    public required string CountryName { get; set; }


    /// <summary>
    ///     The nationality id to be altered.
    /// </summary>
    [Required]
    [DisplayName("Nationality Name")]
    public required int NationalityId { get; set; }

    /// <summary>
    ///     stores the name of the nationality.
    /// </summary>
    [Required]
    [DisplayName("Nationality Name")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string NationalityName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }
}
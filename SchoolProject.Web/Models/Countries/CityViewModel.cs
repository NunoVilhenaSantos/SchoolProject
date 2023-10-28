using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Countries;

/// <summary>
///
/// </summary>
public class CityViewModel
{
    /// <summary>
    ///
    /// </summary>
    [Required]
    public required int CountryId { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required string CountryName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public required int CityId { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    [DisplayName("City")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string CityName { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }
}
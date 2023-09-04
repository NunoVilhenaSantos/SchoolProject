using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Countries;

/// <summary>
///     view model for the nationality class, to change or add a new nationality.
/// </summary>
public class NationalityViewModel
{
    /// <summary>
    ///     stores the country id though the country name.
    /// </summary>
    [Required]
    public required int CountryId { get; set; }

    /// <summary>
    ///     stores the country name though the country id.
    /// </summary>
    public required string CountryName { get; set; }


    /// <summary>
    ///     The nationality id to be altered.
    /// </summary>
    [Required]
    public required int NationalityId { get; set; }


    /// <summary>
    ///     stores the name of the nationality.
    /// </summary>
    [Required]
    [DisplayName("Nationality Name")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }
}
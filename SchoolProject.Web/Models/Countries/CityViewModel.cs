using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;

namespace SchoolProject.Web.Models.Countries;

/// <summary>
/// </summary>
public class CityViewModel
{
    /// <summary>
    /// </summary>
    [Required]
    public required int CountryId { get; set; }

    /// <summary>
    /// </summary>
    public required string CountryName { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public required int CityId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("City")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string CityName { get; set; }

    /// <summary>
    /// </summary>
    [Ignore]
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    [NotMapped]
    public IFormFile? ImageFile { get; set; }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Countries;

/// <summary>
///     Nationality class
/// </summary>
public class Nationality : IEntity, INotifyPropertyChanged
{
    /// <summary>
    ///     the name of the nationality
    /// </summary>
    [Required]
    [DisplayName("Nationality")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // Dependent (child)


    /// <summary>
    ///
    /// </summary>
    [Required]
    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    public virtual required Country Country { get; set; }


    // public Guid CountryGuidId => Country.IdGuid;


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Number of Cities")]
    public int NumberOfCitiesInCountry => Country.Cities?.Count ?? 0;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <inheritdoc />
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    /// <inheritdoc />
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    /// <inheritdoc />
    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    /// <inheritdoc />
    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    /// <inheritdoc />
    [Required]
    [DisplayName("Created By")]
    public virtual required User CreatedBy { get; set; }


    /// <inheritdoc />
    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


    /// <inheritdoc />
    [DisplayName("Updated By")]
    public virtual User? UpdatedBy { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;


    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }


    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
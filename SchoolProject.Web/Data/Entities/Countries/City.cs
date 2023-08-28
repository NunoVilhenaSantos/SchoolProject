using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;


namespace SchoolProject.Web.Data.Entities.Countries;



// [PrimaryKey(nameof(Id), nameof(IdGuid))]
/// <summary>
///  Class for cities data
/// </summary>
public class City : IEntity, INotifyPropertyChanged
{
    /// <summary>
    ///  Name of the city
    /// </summary>
    [Required]
    [DisplayName("City")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [DisplayName("Created By")]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Updated By")] public virtual User? UpdatedBy { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///   The image of the user file from the form to be inserted in the database.
    /// </summary>
    [NotMapped]
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }


    /// <summary>
    ///    The profile photo of the user.
    /// </summary>
    [DisplayName("Profile Photo")]
    public required Guid ProfilePhotoId { get; set; }

    /// <summary>
    ///    The profile photo of the user in URL format.
    /// </summary>
    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://ca001.blob.core.windows.net/images/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/cities/" +
          ProfilePhotoId;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // --------------------------------------------------------------------- //

    [Required] public required int CountryId { get; set; }


    [Required]
    [ForeignKey(nameof(CountryId))]
    public virtual required Country Country { get; set; }

    [DisplayName("Number of Cities")]
    public virtual int NumberCitiesInCountry => Country?.Cities?.Count ?? 0;

    public Guid CountryGuidId => Country.IdGuid;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
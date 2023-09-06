using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.OtherEntities;

/// <summary>
///
/// </summary>
public class Gender : IEntity, INotifyPropertyChanged
{
    [MaxLength(20,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Name { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     The image of the user file from the form to be inserted in the database.
    /// </summary>
    [NotMapped]
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }


    /// <summary>
    ///     The profile photo of the user.
    /// </summary>
    [DisplayName("Profile Photo")]
    public required Guid ProfilePhotoId { get; set; }

    /// <summary>
    ///     The profile photo of the user in URL format.
    /// </summary>
    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://ca001.blob.core.windows.net/images/noimage.png"
        // : StorageHelper.GcpStoragePublicUrl + "genders/" + ProfilePhotoId;
        : StorageHelper.AzureStoragePublicUrl + "genders/" + ProfilePhotoId;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public required bool WasDeleted { get; set; }


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
    // --------------------------------------------------------------------- //


    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;


    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new(propertyName));
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
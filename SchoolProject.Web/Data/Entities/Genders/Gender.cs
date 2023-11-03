using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Genders;

/// <summary>
/// </summary>
public class Gender : IEntity, INotifyPropertyChanged
{
    /// <summary>
    /// </summary>
    [MaxLength(15,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Name { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     The image of the appUser file from the form to be inserted in the database.
    /// </summary>
    [Ignore]
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    [NotMapped]
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }


    /// <summary>
    ///     The profile photo of the appUser.
    /// </summary>
    [DisplayName("Profile Photo")]
    public required Guid ProfilePhotoId { get; set; } = Guid.Empty;


    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    [DisplayName("Profile Photo")]
    public string ProfilePhotoIdUrl =>
        ProfilePhotoId == Guid.Empty || ProfilePhotoId == null
            ? StorageHelper.NoImageUrl
            : StorageHelper.AzureStoragePublicUrl +
              GendersController.BucketName +
              "/" + ProfilePhotoId;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    /// </summary>
    public IEnumerable<AppUser>? AppUsers { get; set; }

    /// <summary>
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int AppUsersCount => AppUsers?.Count() ?? 0;


    /// <summary>
    /// </summary>
    public IEnumerable<Teacher>? Teachers { get; set; }

    /// <summary>
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int TeachersCount => Teachers?.Count() ?? 0;


    /// <summary>
    /// </summary>
    public IEnumerable<Student>? Students { get; set; }


    /// <summary>
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int StudentsCount => Students?.Count() ?? 0;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Created By AppUser")]
    [ForeignKey(nameof(CreatedBy))]
    public string CreatedById { get; set; }

    /// <summary>
    ///     Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Updated By AppUser")]
    [ForeignKey(nameof(UpdatedBy))]
    public string? UpdatedById { get; set; }


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
    public required bool WasDeleted { get; set; }


    /// <inheritdoc />
    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    /// <inheritdoc />
    [Required]
    [DisplayName("Created By")]
    public virtual required AppUser CreatedBy { get; set; }


    /// <inheritdoc />
    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc />
    [DisplayName("Updated By")]
    public virtual AppUser? UpdatedBy { get; set; }


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
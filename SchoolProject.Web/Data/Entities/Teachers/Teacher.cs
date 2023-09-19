using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration.Attributes;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Teachers;

/// <summary>
///     teacher class for ef
/// </summary>
public class Teacher : IEntity, INotifyPropertyChanged
{
    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("First Name")]
    public required string FirstName { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Last Name")]
    public required string LastName { get; set; }


    /// <summary>
    /// </summary>
    [DisplayName("Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    /// <summary>
    /// </summary>
    [Required]
    public required string Address { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Postal Code")]
    public required string PostalCode { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    public required City City { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public required Country Country { get; set; }


    // public int CountryId => Country.Id;
    // public Guid CountryGuidId => Country.IdGuid;


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Mobile Phone")]
    public required string MobilePhone { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public required bool Active { get; set; } = true;

    /// <summary>
    /// </summary>
    [Required]
    public required Gender Gender { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Date Of Birth")]
    [DataType(DataType.Date)]
    public required DateTime DateOfBirth { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Identification Number")]
    public required string IdentificationNumber { get; set; }

    /// <summary>
    /// </summary>
    public required string IdentificationType { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Expiration Date Identification Number")]
    [DataType(DataType.Date)]
    public required DateTime ExpirationDateIdentificationNumber { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Tax Identification Number")]
    public required string TaxIdentificationNumber { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Country Of Nationality")]
    public required Country CountryOfNationality { get; set; }

    // [Required]
    // public Nationality Nationality => CountryOfNationality.Nationality;

    // [Required] public required Nationality Nationality { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    public required Country Birthplace { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Enroll Date")]
    [DataType(DataType.Date)]
    public required DateTime EnrollDate { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    public required User User { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     The image of the user file from the form to be inserted in the database.
    /// </summary>
    [Ignore]
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
        // : StorageHelper.GcpStoragePublicUrl + "teachers/" + ProfilePhotoId;
        : StorageHelper.AzureStoragePublicUrl + "teachers/" + ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Navigation property for the many-to-many relationship with courses
    /// </summary>
    public virtual HashSet<TeacherCourse>? TeacherCourses { get; set; }
    // = new List<TeacherCourse>();


    /// <summary>
    /// </summary>
    [DisplayName("Disciplines Count")]
    public int CoursesCount => TeacherCourses?.Count ?? 0;

    /// <summary>
    /// </summary>
    [DisplayName("Total Work Hours")]
    public int TotalWorkHours => TeacherCourses?
        .Sum(t => t.Course.Hours) ?? 0;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


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


    // ---------------------------------------------------------------------- //
    // Property Changed Event Handler
    // ---------------------------------------------------------------------- //


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
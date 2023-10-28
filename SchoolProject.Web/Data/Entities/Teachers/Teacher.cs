using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration.Attributes;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Genders;
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
    ///     The country Id.
    /// </summary>
    // [Required]
    [NotMapped]
    [DisplayName("Country")]
    // [ForeignKey(nameof(City))]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country...")]
    public required int CountryId { get; set; }


    /// <summary>
    /// 
    /// </summary>
    // [Required]
    [ForeignKey(nameof(City))]
    public int CityId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public required City City { get; set; }


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
    // [Required]
    [ForeignKey(nameof(Gender))]
    public int GenderId { get; set; }

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
    [DisplayName("Expiration Date of Identification Number")]
    [DataType(DataType.Date)]
    public required DateTime ExpirationDateIdentificationNumber { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Tax Identification Number")]
    public required string TaxIdentificationNumber { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // [Required]
    /// <summary>
    ///
    /// </summary>
    [ForeignKey(nameof(CountryOfNationality))]
    [DisplayName("Country Of Nationality")]
    public int CountryOfNationalityId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Country Of Nationality")]
    public required Country CountryOfNationality { get; set; }


    /// <summary>
    ///
    /// </summary>
    [ForeignKey(nameof(Birthplace))]
    public int BirthplaceId { get; set; }

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
    public required DateTime EnrollDate { get; set; } = DateTime.UtcNow;


    /// <summary>
    ///
    /// </summary>
    [ForeignKey(nameof(AppUser))]
    public string UserId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public required AppUser AppUser { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     The image of the appUser file from the form to be inserted in the database.
    /// </summary>
    [Ignore]
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
              TeachersController.BucketName +
              "/" + ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    ///// <summary>
    /////    Navigation property for the many-to-many relationship with courses
    ///// </summary>
    //public IEnumerable<Discipline>? Disciplines { get; set; }


    ///// <summary>
    ///// </summary>
    //[DisplayName("Disciplines Count")]
    //public int DisciplinesCount => Disciplines?.Count() ?? 0;

    ///// <summary>
    ///// </summary>
    //[DisplayName("Total Work Hours")]
    //public int TotalWorkHours => Disciplines?
    //    .Sum(t => t.Hours) ?? 0;

    ///// <summary>
    ///// </summary>
    //[DisplayName("Total Students")]
    //public int TotalStudents => Disciplines?
    //    .Sum(t => t.StudentsCount) ?? 0;


    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship with courses
    /// </summary>
    public virtual HashSet<TeacherDiscipline>? TeacherDisciplines { get; set; }
    // = new List<TeacherDiscipline>();


    /// <summary>
    ///    Returns the disciplines associated with this teacher
    /// </summary>
    [NotMapped]
    public IEnumerable<Discipline>? Disciplines =>
        TeacherDisciplines?.Select(sc => sc.Discipline).Distinct();


    /// <summary>
    /// </summary>
    [DisplayName("Disciplines Count")]
    public int DisciplinesCount => TeacherDisciplines?.Count() ?? 0;

    /// <summary>
    /// </summary>
    [DisplayName("Total Work Hours")]
    public int TotalWorkHours => TeacherDisciplines?
        .Sum(t => t.Discipline?.Hours) ?? 0;

    /// <summary>
    /// </summary>
    [DisplayName("Total Students")]
    public int TotalStudents => TeacherDisciplines?
        .Sum(t => t.Discipline?.StudentsCount) ?? 0;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <inheritdoc />
    //[Key]
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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



    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    /// Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Created By AppUser")]
    [ForeignKey(nameof(CreatedBy))]
    public string CreatedById { get; set; }

    /// <summary>
    /// Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Updated By AppUser")]
    [ForeignKey(nameof(UpdatedBy))]
    public string? UpdatedById { get; set; }


    // --------------------------------------------------------------------- //
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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Genders;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Students;

/// <summary>
///     Student class for ef
/// </summary>
public class Student : IEntity, INotifyPropertyChanged
{
    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("First Name")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string FirstName { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Last Name")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string LastName { get; set; }


    /// <summary>
    /// </summary>
    [DisplayName("Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(150, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Address { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Postal Code")]
    [MaxLength(10, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string PostalCode { get; set; }


    /// <summary>
    ///     The country Id.
    /// </summary>
    // [Required]
    [NotMapped]
    [DisplayName("Country")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country...")]
    public required int CountryId { get; set; }


    /// <summary>
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
    [MaxLength(20, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string MobilePhone { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    [MaxLength(200, ErrorMessage = "The field {0} can contain {1} characters.")]
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
    [MaxLength(20, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string IdentificationNumber { get; set; }

    /// <summary>
    /// </summary>
    [MaxLength(20, ErrorMessage = "The field {0} can contain {1} characters.")]
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
    [MaxLength(20, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string TaxIdentificationNumber { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // [Required]
    /// <summary>
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
    /// </summary>
    [ForeignKey(nameof(Birthplace))]
    public int BirthplaceId { get; set; }

    /// <summary>
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
              StudentsController.BucketName +
              "/" + ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and Courses
    // ---------------------------------------------------------------------- //


    /// <summary>
    /// </summary>
    public virtual HashSet<CourseStudent>? CourseStudents { get; set; }


    /// <summary>
    /// </summary>
    [NotMapped]
    public IEnumerable<Course>? Courses =>
        CourseStudents?.Select(cd => cd.Course).Distinct();

    /// <summary>
    /// </summary>
    [DisplayName("Courses Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int CoursesCount => Courses?.Count() ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Courses Work Load")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int CoursesWorkLoad => Courses?.Sum(e => e.WorkHourLoad) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Courses Credits")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public double CoursesCredits => Courses?.Sum(r => r.CourseCredits) ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and Disciplines
    // ---------------------------------------------------------------------- //


    /// <summary>
    /// </summary>
    public virtual HashSet<Enrollment>? Enrollments { get; set; }


    /// <summary>
    ///     Returns the disciplines associated with this teacher
    /// </summary>
    [NotMapped]
    public IEnumerable<Discipline> Disciplines =>
        Enrollments?.Select(sc => sc.Discipline).Distinct() ??
        Array.Empty<Discipline>();


    /// <summary>
    /// </summary>
    [DisplayName("Disciplines Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int CoursesCountEnrollments =>
        Enrollments?.Where(e => e.Discipline.Id == Id).Count() ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Total Work Hours")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int TotalWorkHoursEnrollments =>
        Enrollments?.Sum(e => e.Discipline.Hours) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Highest Grade")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal HighestGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Max(e => e.Grade) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Average Grade")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal AverageGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Average(e => e.Grade) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Lowest Grade")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal LowestGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Min(e => e.Grade) ?? 0;


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
    public virtual HashSet<StudentDiscipline>? StudentDisciplines { get; set; }
    //= new HashSet<StudentDiscipline>();


    /// <summary>
    ///     Returns the disciplines associated with this teacher
    /// </summary>
    [NotMapped]
    public IEnumerable<Discipline>? SDDisciplines =>
        StudentDisciplines?.Select(sc => sc.Discipline).Distinct();


    /// <summary>
    /// </summary>
    [DisplayName("Disciplines Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int DisciplinesCount => SDDisciplines?.Count() ?? 0;

    /// <summary>
    /// </summary>
    [DisplayName("Total Work Hours")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int TotalWorkHours => SDDisciplines?
        .Sum(t => t.Hours) ?? 0;

    /// <summary>
    /// </summary>
    [DisplayName("Total Students")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int TotalStudents => SDDisciplines?
        .Sum(t => t.StudentsCount) ?? 0;


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
    // ---------------------------------------------------------------------- //


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
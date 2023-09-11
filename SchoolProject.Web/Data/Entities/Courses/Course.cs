using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Courses;

/// <summary>
///
/// </summary>
public class Course : IEntity, INotifyPropertyChanged
{
    /// <summary>
    ///
    /// </summary>
    [DisplayName("Code")]
    [MaxLength(7,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Code { get; init; }


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Discipline Acronym")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Acronym { get; set; }


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Discipline Name")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Name { get; set; }


    /// <summary>
    ///
    /// </summary>
    [DisplayName("QNQ Level")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required byte QnqLevel { get; init; }


    /// <summary>
    ///
    /// </summary>
    [DisplayName("EQF Level")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required byte EqfLevel { get; init; }

    // old version
    // [Required]
    // [DisplayName("Start Date")]
    // [DataType(DataType.Date)]
    // public DateOnly StartDate { get; set; }

    // [Required]
    // [DataType(DataType.Date)]
    // [DisplayName("End Date")]
    // public DateOnly EndDate { get; set; }


    // [Required]
    // [DisplayName("Start Hour")]
    // [DataType(DataType.Time)]
    // public TimeOnly StartHour { get; set; }

    // [Required]
    // [DataType(DataType.Time)]
    // [DisplayName("End Hour")]
    // public TimeOnly EndHour { get; set; }

    // new version
    /// <summary>
    ///
    /// </summary>
    [Required]
    [DisplayName("Start Date")]
    [DataType(DataType.Date)]
    public required DateTime StartDate { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    [DisplayName("End Date")]
    [DataType(DataType.Date)]
    public required DateTime EndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    [DisplayName("Start Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan StartHour { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    [DisplayName("End Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan EndHour { get; set; }


    /// <summary>
    ///
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Area { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    [Precision(10, 2)]
    [DataType(DataType.Currency)]
    [DisplayName("Price for Employed")]
    public required decimal PriceForEmployed { get; set; } = 200;


    /// <summary>
    ///
    /// </summary>
    [Required]
    [Precision(10, 2)]
    [DataType(DataType.Currency)]
    [DisplayName("Price for Unemployed")]
    public required decimal PriceForUnemployed { get; set; }


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
        // : StorageHelper.GcpStoragePublicUrl + "school-classes/" + ProfilePhotoId;
        : StorageHelper.AzureStoragePublicUrl + "course/" +
          ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Navigation property for the many-to-many relationship between Discipline and Discipline
    /// </summary>
    public virtual HashSet<CourseDisciplines>? CourseDisciplines { get; set; }


    ///// <summary>
    /////
    ///// </summary>
    //[DisplayName("Disciplines Count")]
    //public int? CoursesCount => CourseDisciplines?.Count ?? 0;


    ///// <summary>
    /////
    ///// </summary>
    //[DisplayName("Discipline CreditPoints")]
    //public double? CourseCredits =>
    //    CourseDisciplines?.Sum(c => c.Discipline.CreditPoints) ?? 0;


    ///// <summary>
    /////
    ///// </summary>
    //[DisplayName("Work Hour Load")]
    //public int? WorkHourLoad =>
    //    CourseDisciplines?.Sum(c => c.Discipline.Hours) ?? 0;


    //// ---------------------------------------------------------------------- //
    //// Navigation property for the many-to-many relationship
    //// ---------------------------------------------------------------------- //

    ///// <summary>
    /////     Navigation property for the many-to-many relationship between Discipline and Student
    ///// </summary>
    public virtual HashSet<CourseStudents>? CourseStudents { get; set; }


    ///// <summary>
    /////
    ///// </summary>
    //[DisplayName("Students Count")]
    //public int? ScsStudentsCount => Students?.Count ?? 0;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Navigation property for the one-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///  List of Disciplines for this Discipline
    /// </summary>
    public virtual HashSet<Discipline>? Disciplines { get; set; }


    /// <summary>
    ///  List of Students for this Discipline
    /// </summary>
    public virtual HashSet<Student>? Students { get; set; }


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Students Count")]
    public int? StudentsCount => Students?.Count() ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///
    /// </summary>
    [DisplayName("Enrollment")]
    public virtual HashSet<Enrollment>? Enrollment { get; set; }


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Class Average")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? ClassAverage =>
        Enrollment?.Where(e => e.Grade.HasValue)
            .Average(e => e.Grade);


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Highest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? HighestGrade =>
        Enrollment?.Max(e => e.Grade);


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Lowest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? LowestGrade =>
        Enrollment?.Min(e => e.Grade);


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Disciplines Count")]
    public int ECoursesCount =>
        Enrollment?.Select(e => e.Discipline).Distinct().Count() ?? 0;


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Work Hour Load")]
    public int EWorkHourLoad =>
        Enrollment?.Sum(e => e.Discipline.Hours) ?? 0;


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Students Count")]
    public int EStudentsCount =>
        Enrollment?.Select(e => e.Student).Distinct().Count() ?? 0;


    // --------------------------------------------------------------------- //
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
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
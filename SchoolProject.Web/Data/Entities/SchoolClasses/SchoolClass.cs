using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClass : IEntity, INotifyPropertyChanged
{
    [DisplayName("Code")]
    [MaxLength(7,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Code { get; init; }


    [DisplayName("Class Acronym")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Acronym { get; set; }


    [DisplayName("Class Name")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Name { get; set; }


    [DisplayName("QNQ Level")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required byte QnqLevel { get; init; }


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
    [Required]
    [DisplayName("Start Date")]
    [DataType(DataType.Date)]
    public required DateTime StartDate { get; set; }

    [Required]
    [DisplayName("End Date")]
    [DataType(DataType.Date)]
    public required DateTime EndDate { get; set; }

    [Required]
    [DisplayName("Start Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan StartHour { get; set; }

    [Required]
    [DisplayName("End Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan EndHour { get; set; }


    public string? Location { get; set; }

    public string? Type { get; set; }

    public string? Area { get; set; }


    [Required]
    [Precision(10, 2)]
    [DataType(DataType.Currency)]
    [DisplayName("Price for Employed")]
    public required decimal PriceForEmployed { get; set; } = 200;


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
        : "https://storage.googleapis.com/storage-nuno/schoolclasses/" +
          ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Navigation property for the many-to-many relationship between SchoolClass and Course
    /// </summary>
    public virtual ICollection<SchoolClassCourse> SchoolClassCourses
    {
        get;
        set;
    } =
        new List<SchoolClassCourse>();


    [DisplayName("Courses Count")]
    public int? CoursesCount => SchoolClassCourses?.Count ?? 0;


    [DisplayName("SchoolClass CreditPoints")]
    public double? SchoolClassCredits =>
        SchoolClassCourses?.Sum(c => c.Course.CreditPoints) ?? 0;


    [DisplayName("Work Hour Load")]
    public int? WorkHourLoad =>
        SchoolClassCourses?.Sum(c => c.Course.Hours) ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between SchoolClass and Student
    /// </summary>
    public virtual ICollection<Student>? Students { get; set; } =
        new List<Student>();


    [DisplayName("Students Count")]
    public int? StudentsCount => Students?.Count ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between SchoolClass and Student
    /// </summary>
    public virtual ICollection<SchoolClassStudent>? SchoolClassStudents
    {
        get;
        set;
    } =
        new List<SchoolClassStudent>();


    [DisplayName("Students Count")]
    public int? SCSStudentsCount => Students?.Count ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    [DisplayName("Enrollment")]
    public virtual IEnumerable<Enrollment>? Enrollment { get; set; }


    [DisplayName("Class Average")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? ClassAverage =>
        Enrollment?.Where(e => e.Grade.HasValue)
            .Average(e => e.Grade);


    [DisplayName("Highest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? HighestGrade =>
        Enrollment?.Max(e => e.Grade);


    [DisplayName("Lowest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? LowestGrade =>
        Enrollment?.Min(e => e.Grade);


    [DisplayName("Courses Count")]
    public int ECoursesCount =>
        Enrollment?.Select(e => e.Course).Distinct().Count() ?? 0;


    [DisplayName("Work Hour Load")]
    public int EWorkHourLoad =>
        Enrollment?.Sum(e => e.Course.Hours) ?? 0;


    [DisplayName("Students Count")]
    public int EStudentsCount =>
        Enrollment?.Select(e => e.Student).Distinct().Count() ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Navigation property for the many-to-many relationship between SchoolClass and Courses
    ///     This property is used by EF Core to automatically build a join table
    /// </summary>
    public virtual IEnumerable<Course>? Courses { get; init; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    [Key]
    [DatabaseGenerated(
        DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(
        DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(
        DatabaseGeneratedOption.Identity)]
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


    // ---------------------------------------------------------------------- //
    // Property Changed Event Handler
    // ---------------------------------------------------------------------- //


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
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
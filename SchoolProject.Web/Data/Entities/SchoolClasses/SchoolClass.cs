using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClass : IEntity, INotifyPropertyChanged
{
    [DisplayName(displayName: "Code")]
    [MaxLength(length: 7,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Code { get; init; }


    [DisplayName(displayName: "Class Acronym")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Acronym { get; set; }


    [DisplayName(displayName: "Class Name")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Name { get; set; }


    [DisplayName(displayName: "QNQ Level")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required byte QnqLevel { get; init; }


    [DisplayName(displayName: "EQF Level")]
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
    [DisplayName(displayName: "Start Date")]
    [DataType(dataType: DataType.Date)]
    public required DateTime StartDate { get; set; }

    [Required]
    [DisplayName(displayName: "End Date")]
    [DataType(dataType: DataType.Date)]
    public required DateTime EndDate { get; set; }

    [Required]
    [DisplayName(displayName: "Start Hour")]
    [DataType(dataType: DataType.Time)]
    public required TimeSpan StartHour { get; set; }

    [Required]
    [DisplayName(displayName: "End Hour")]
    [DataType(dataType: DataType.Time)]
    public required TimeSpan EndHour { get; set; }


    public string? Location { get; set; }

    public string? Type { get; set; }

    public string? Area { get; set; }


    [Required]
    [Precision(precision: 10, scale: 2)]
    [DataType(dataType: DataType.Currency)]
    [DisplayName(displayName: "Price for Employed")]
    public required decimal PriceForEmployed { get; set; } = 200;


    [Required]
    [Precision(precision: 10, scale: 2)]
    [DataType(dataType: DataType.Currency)]
    [DisplayName(displayName: "Price for Unemployed")]
    public required decimal PriceForUnemployed { get; set; }


    [DisplayName(displayName: "Profile Photo")] public Guid? ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/schoolclasses/" +
          ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Navigation property for the many-to-many relationship between SchoolClass and Course
    /// </summary>
    public ICollection<SchoolClassCourse> SchoolClassCourses { get; set; } =
        new List<SchoolClassCourse>();


    [DisplayName(displayName: "Courses Count")]
    public int? CoursesCount => SchoolClassCourses?.Count ?? 0;


    [DisplayName(displayName: "SchoolClass CreditPoints")]
    public double? SchoolClassCredits =>
        SchoolClassCourses?.Sum(selector: c => c.Course.CreditPoints) ?? 0;


    [DisplayName(displayName: "Work Hour Load")]
    public int? WorkHourLoad =>
        SchoolClassCourses?.Sum(selector: c => c.Course.Hours) ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between SchoolClass and Student
    /// </summary>
    public ICollection<Student>? Students { get; set; } = new List<Student>();


    [DisplayName(displayName: "Students Count")]
    public int? StudentsCount => Students?.Count ?? 0;


    [DisplayName(displayName: "Enrollment")]
    public IEnumerable<Enrollment>? Enrollment { get; set; }


    [DisplayName(displayName: "Class Average")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(precision: 18, scale: 2)]
    public decimal? ClassAverage =>
        Enrollment?.Where(predicate: e => e.Grade.HasValue)
            .Average(selector: e => e.Grade);


    [DisplayName(displayName: "Highest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(precision: 18, scale: 2)]
    public decimal? HighestGrade =>
        Enrollment?.Max(selector: e => e.Grade);


    [DisplayName(displayName: "Lowest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(precision: 18, scale: 2)]
    public decimal? LowestGrade =>
        Enrollment?.Min(selector: e => e.Grade);


    [DisplayName(displayName: "Courses Count")]
    public int ECoursesCount =>
        Enrollment?.Select(selector: e => e.Course).Distinct().Count() ?? 0;


    [DisplayName(displayName: "Work Hour Load")]
    public int EWorkHourLoad =>
        Enrollment?.Sum(selector: e => e.Course.Hours) ?? 0;


    [DisplayName(displayName: "Students Count")]
    public int EStudentsCount =>
        Enrollment?.Select(selector: e => e.Student).Distinct().Count() ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Navigation property for the many-to-many relationship between SchoolClass and Courses
    ///     This property is used by EF Core to automatically build a join table
    /// </summary>
    public List<Course>? Courses { get; init; }

    [Key]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName(displayName: "Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Created At")]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [DisplayName(displayName: "Created By")]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


    [DisplayName(displayName: "Updated By")] public virtual User? UpdatedBy { get; set; }


    // ---------------------------------------------------------------------- //
    // Property Changed Event Handler
    // ---------------------------------------------------------------------- //


    public event PropertyChangedEventHandler? PropertyChanged;


    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(sender: this,
            e: new PropertyChangedEventArgs(propertyName: propertyName));
    }


    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(x: field, y: value)) return false;
        field = value;
        OnPropertyChanged(propertyName: propertyName);
        return true;
    }
}
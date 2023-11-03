using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Courses;

/// <summary>
/// </summary>
public class Course : IEntity, INotifyPropertyChanged
{
    /// <summary>
    /// </summary>
    [DisplayName("Code")]
    [MaxLength(7,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Code { get; init; }


    /// <summary>
    /// </summary>
    [DisplayName("Course Acronym")]
    [MaxLength(20, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Acronym { get; set; }


    /// <summary>
    /// </summary>
    [DisplayName("Course Name")]
    [MaxLength(150, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    /// <summary>
    /// </summary>
    [DisplayName("QNQ Level")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required byte QnqLevel { get; init; }


    /// <summary>
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
    /// </summary>
    [Required]
    [DisplayName("Start Date")]
    [DataType(DataType.Date)]
    public required DateTime StartDate { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("End Date")]
    [DataType(DataType.Date)]
    public required DateTime EndDate { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Start Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan StartHour { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("End Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan EndHour { get; set; }


    /// <summary>
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// </summary>
    public string? Area { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [Precision(10, 2)]
    [DataType(DataType.Currency)]
    [DisplayName("Price for Employed")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public required decimal PriceForEmployed { get; set; } = 200;


    /// <summary>
    /// </summary>
    [Required]
    [Precision(10, 2)]
    [DataType(DataType.Currency)]
    [DisplayName("Price for Unemployed")]
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public required decimal PriceForUnemployed { get; set; }


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
              CoursesController.BucketName +
              "/" + ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Navigation property for the many-to-many relationship between Course and Discipline
    /// </summary>
    public virtual HashSet<CourseDiscipline>? CourseDisciplines { get; set; }


    /// <summary>
    /// </summary>
    [NotMapped]
    public IEnumerable<Discipline>? Disciplines =>
        CourseDisciplines?.Select(cd => cd.Discipline).Distinct();

    /// <summary>
    /// </summary>
    [DisplayName("Disciplines Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int CoursesCount => Disciplines?.Count() ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Course Total Credit Points")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public double CourseCredits => Disciplines?.Sum(c => c?.CreditPoints) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Work Hour Load")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int WorkHourLoad =>
        CourseDisciplines?.Sum(c => c.Discipline?.Hours) ?? 0;


    //// ---------------------------------------------------------------------- //
    //// Navigation property for the many-to-many relationship
    //// ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between Courses and Student
    /// </summary>
    public virtual HashSet<CourseStudent>? CourseStudents { get; set; }


    /// <summary>
    /// </summary>
    [NotMapped]
    public IEnumerable<Student>? Students =>
        CourseStudents?.Select(cd => cd.Student).Distinct();

    /// <summary>
    /// </summary>
    [DisplayName("Course CreditPoints")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int StudentsCount0 => Students?.Count() ?? 0;


    /// <summary>
    ///     This property calculates the number of distinct students associated with the course
    /// </summary>
    [DisplayName("Students Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int StudentsCount =>
        CourseStudents?.Select(e => e.CourseId).Distinct().Count() ?? 0;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Navigation property for the one-to-many relationship
    // ---------------------------------------------------------------------- //


    ///// <summary>
    /////     List of Disciplines
    ///// </summary>
    //public virtual HashSet<Discipline>? Disciplines { get; set; }

    ///// <summary>
    ///// </summary>
    //[DisplayName("Disciplines Count")]
    //public int  DisciplinesCount => Disciplines?.Count() ?? 0;


    ///// <summary>
    /////     List of Students
    ///// </summary>
    //public virtual HashSet<Student>? Students { get; set; }


    ///// <summary>
    ///// </summary>
    //[DisplayName("Students Count")]
    //public int StudentsCount => Students?.Count() ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    /// </summary>
    [DisplayName("Enrollment")]
    public virtual HashSet<Enrollment>? Enrollments { get; set; }

    /// <summary>
    /// </summary>
    [NotMapped]
    public IEnumerable<Student>? EStudentsList =>
        Enrollments?.Select(e => e.Student).Distinct();

    /// <summary>
    /// </summary>
    [NotMapped]
    public IEnumerable<Discipline>? EDisciplinesList =>
        Enrollments?.Select(e => e.Discipline).Distinct();


    /// <summary>
    /// </summary>
    [DisplayName("Class Average")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal? ClassAverage =>
        Enrollments?.Where(e => e.Grade.HasValue)
            .Average(e => e.Grade);


    /// <summary>
    /// </summary>
    [DisplayName("Highest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal? HighestGrade =>
        Enrollments?.Where(e => e.Grade.HasValue)
            .Max(e => e.Grade);


    /// <summary>
    /// </summary>
    [DisplayName("Lowest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal? LowestGrade =>
        Enrollments?.Where(e => e.Grade.HasValue)
            .Min(e => e.Grade);


    /// <summary>
    /// </summary>
    [DisplayName("Disciplines Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int ECoursesCount =>
        Enrollments?.Select(e => e.Discipline).Distinct().Count() ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Work Hour Load")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int EWorkHourLoad =>
        Enrollments?.Sum(e => e.Discipline?.Hours) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Students Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int EStudentsCount =>
        Enrollments?.Select(e => e.Student).Distinct().Count() ?? 0;


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
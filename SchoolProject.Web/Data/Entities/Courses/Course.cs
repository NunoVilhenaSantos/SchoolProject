using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Courses;

public class Course : IEntity, INotifyPropertyChanged
{
    /// <summary>
    ///     The code of the course.
    /// </summary>
    [DisplayName("Code")]
    [MaxLength(7,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Code { get; init; }

    /// <summary>
    ///     The name of the course.
    /// </summary>
    [Required]
    public required string Name { get; set; }


    /// <summary>
    ///     The description of the course.
    /// </summary>
    public string? Description { get; set; }


    /// <summary>
    ///     The number of hours of the course.
    /// </summary>
    [Required]
    public required int Hours { get; set; }


    /// <summary>
    ///     The number of credit points of the course.
    ///     The number of ECTS of the course.
    ///     The number of ECTS credits of the course.
    ///     European Credit Transfer and Accumulation System (ECTS).
    /// </summary>
    [Required]
    public required double CreditPoints { get; set; }


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
        // : StorageHelper.GcpStoragePublicUrl + "courses/" + ProfilePhotoId;
        : StorageHelper.AzureStoragePublicUrl + "courses/" + ProfilePhotoId;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    // ---------------------------------------------------------------------- //
    // SchoolClassCourse relationship
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

    /// <summary>
    ///     Returns the number of the course for this school classes
    /// </summary>
    public int SchoolClassCoursesCount => SchoolClassCourses.Count;


    // ---------------------------------------------------------------------- //
    // Teacher Courses relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between Teacher and Course
    /// </summary>
    public virtual ICollection<TeacherCourse> TeacherCourses { get; set; } =
        new List<TeacherCourse>();

    /// <summary>
    ///     Returns the number of the course for this teachers
    /// </summary>
    public int TeacherCoursesCount => TeacherCourses.Count;

    public int TeachersCount => TeacherCourses
        .Select(tc => tc.Teacher).Distinct().Count();


    // ---------------------------------------------------------------------- //
    // Student Courses relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between Courses and Students
    /// </summary>
    public virtual ICollection<StudentCourse> StudentCourses { get; set; } =
        new List<StudentCourse>();

    public int StudentCoursesCount => StudentCourses.Count;

    public int StudentCount => StudentCourses
        .Select(sc => sc.Student).Distinct().Count();


    // ---------------------------------------------------------------------- //
    // Enrollments relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the one-to-many relationship between Course and Enrollment
    /// </summary>
    public virtual ICollection<Enrollment>? Enrollments { get; set; }


    /// <summary>
    ///     Returns the number of students enrolled in the course
    /// </summary>
    [DisplayName("Enrolled Students")]
    public int? StudentsCount =>
        Enrollments?.Where(e => e.Course.Id == Id).Count() ?? 0;


    /// <summary>
    ///     Returns the highest grade of the course
    /// </summary>
    [DisplayName("Highest Grade")]
    public decimal? HighestGrade => Enrollments?
        .Where(e => e.CourseId == Id)
        .Max(e => e.Grade) ?? null;


    /// <summary>
    ///     Returns the average grade of the course
    /// </summary>
    [DisplayName("Average Grade")]
    public decimal? AveregaGrade => Enrollments?
        .Where(e => e.CourseId == Id)
        .Average(e => e.Grade) ?? null;


    /// <summary>
    ///     Returns the lowest grade of the course
    /// </summary>
    [DisplayName("Lowest Grade")]
    public decimal? LowestGrade => Enrollments?
        .Where(e => e.CourseId == Id)
        .Min(e => e.Grade) ?? null;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     ID of the course.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    /// <summary>
    ///     Guid value of the ID of the course.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    /// <summary>
    ///     Was Deleted?
    ///     Determines whether the course was deleted or not.
    /// </summary>
    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    /// <summary>
    ///     Date and time of the creation of the course.
    /// </summary>
    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     The user who created the course.
    /// </summary>
    [Required]
    [DisplayName("Created By")]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    /// <summary>
    ///     Date and time of the update of the course.
    /// </summary>
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     The user who updated the course.
    /// </summary>
    [DisplayName("Updated By")]
    public virtual User? UpdatedBy { get; set; }


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
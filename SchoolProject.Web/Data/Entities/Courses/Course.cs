using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Courses;

public class Course : IEntity, INotifyPropertyChanged
{
    /// <summary>
    ///     The code of the course.
    /// </summary>
    [DisplayName(displayName: "Code")]
    [MaxLength(length: 7,
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
    public int Description { get; set; }


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


    /// <summary>
    ///     The profile photo of the course.
    ///     Guid value of the profile photo of the course.
    ///     Guid.Empty is the default value of the Guid type.
    /// </summary>
    [DisplayName(displayName: "Profile Photo")]
    public Guid ProfilePhotoId { get; set; }


    /// <summary>
    ///     The profile photo of the course.
    /// </summary>
    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/courses/" +
          ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    // ---------------------------------------------------------------------- //
    // SchoolClassCourse relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between SchoolClass and Course
    /// </summary>
    public ICollection<SchoolClassCourse> SchoolClassCourses { get; set; } =
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
    public ICollection<TeacherCourse> TeacherCourses { get; set; } =
        new List<TeacherCourse>();

    /// <summary>
    ///     Returns the number of the course for this teachers
    /// </summary>
    public int TeacherCoursesCount => TeacherCourses.Count;

    public int TeachersCount => TeacherCourses
        .Select(selector: tc => tc.Teacher).Distinct().Count();


    // ---------------------------------------------------------------------- //
    // Student Courses relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between Courses and Students
    /// </summary>
    public ICollection<StudentCourse> StudentCourses { get; set; } =
        new List<StudentCourse>();

    public int StudentCoursesCount => StudentCourses.Count;

    public int StudentCount => StudentCourses
        .Select(selector: sc => sc.Student).Distinct().Count();


    // ---------------------------------------------------------------------- //
    // Enrollments relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the one-to-many relationship between Course and Enrollment
    /// </summary>
    public ICollection<Enrollment>? Enrollments { get; set; }


    /// <summary>
    ///     Returns the number of students enrolled in the course
    /// </summary>
    [DisplayName(displayName: "Enrolled Students")]
    public int? StudentsCount =>
        Enrollments?.Where(predicate: e => e.Course.Id == Id).Count() ?? 0;


    /// <summary>
    ///     Returns the highest grade of the course
    /// </summary>
    [DisplayName(displayName: "Highest Grade")]
    public decimal? HighestGrade => Enrollments?
        .Where(predicate: e => e.CourseId == Id)
        .Max(selector: e => e.Grade) ?? null;


    /// <summary>
    ///     Returns the average grade of the course
    /// </summary>
    [DisplayName(displayName: "Average Grade")]
    public decimal? AveregaGrade => Enrollments?
        .Where(predicate: e => e.CourseId == Id)
        .Average(selector: e => e.Grade) ?? null;


    /// <summary>
    ///     Returns the lowest grade of the course
    /// </summary>
    [DisplayName(displayName: "Lowest Grade")]
    public decimal? LowestGrade => Enrollments?
        .Where(predicate: e => e.CourseId == Id)
        .Min(selector: e => e.Grade) ?? null;


    /// <summary>
    ///     ID of the course.
    /// </summary>
    [Key]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    /// <summary>
    ///     Guid value of the ID of the course.
    /// </summary>
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    /// <summary>
    ///     Was Deleted?
    ///     Determines whether the course was deleted or not.
    /// </summary>
    [Required]
    [DisplayName(displayName: "Was Deleted?")]
    public bool WasDeleted { get; set; }


    /// <summary>
    ///     Date and time of the creation of the course.
    /// </summary>
    [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Created At")]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     The user who created the course.
    /// </summary>
    [Required]
    [DisplayName(displayName: "Created By")]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    /// <summary>
    ///     Date and time of the update of the course.
    /// </summary>
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    ///     The user who updated the course.
    /// </summary>
    [DisplayName(displayName: "Updated By")]
    public virtual User? UpdatedBy { get; set; }


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
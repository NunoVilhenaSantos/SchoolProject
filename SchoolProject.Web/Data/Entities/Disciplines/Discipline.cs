using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration.Attributes;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Disciplines;

/// <summary>
///
/// </summary>
public class Discipline : IEntity, INotifyPropertyChanged
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
    public required string Description { get; set; }


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
    public required Guid ProfilePhotoId { get; set; }


    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? StorageHelper.NoImageUrl
        : StorageHelper.AzureStoragePublicUrl +
          DisciplinesController.BucketName +
          "/" + ProfilePhotoId;




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
    ///     Navigation property for the many-to-many relationship between Discipline and Discipline
    /// </summary>
    public virtual HashSet<CourseDisciplines>? CourseDisciplines { get; set; }


    ///// <summary>
    /////     Returns the number of the course for this school classes
    ///// </summary>
    //public int SchoolClassCoursesCount => CourseDisciplines.Count;


    // ---------------------------------------------------------------------- //
    // Student Disciplines relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between Disciplines and Students
    /// </summary>
    public virtual HashSet<StudentCourse>? StudentCourses { get; set; }


    //public int StudentCoursesCount => StudentCourses.Count;


    //public int StudentCount => StudentCourses
    //    .Select(sc => sc.Student).Distinct().Count();


    // ---------------------------------------------------------------------- //
    // Teacher Disciplines relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between Teacher and Discipline
    /// </summary>
    public virtual HashSet<TeacherCourse>? TeacherCourses { get; set; }

    ///// <summary>
    /////     Returns the number of the course for this teachers
    ///// </summary>
    //public int TeacherCoursesCount => TeacherCourses.Count;


    //public int TeachersCount => TeacherCourses
    //    .Select(tc => tc.Teacher).Distinct().Count();


    // ---------------------------------------------------------------------- //
    // Enrollments relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the one-to-many relationship between Discipline and Enrollment
    /// </summary>
    public virtual HashSet<Enrollment>? Enrollments { get; set; }


    /// <summary>
    ///     Returns the number of students enrolled in the course
    /// </summary>
    [DisplayName("Enrolled Students")]
    public int? StudentsCount =>
        Enrollments?.Where(e => e.Discipline.Id == Id).Count() ?? 0;


    /// <summary>
    ///     Returns the highest grade of the course
    /// </summary>
    [DisplayName("Highest Grade")]
    public decimal? HighestGrade => Enrollments?
        .Where(e => e.DisciplineId == Id)
        .Max(e => e.Grade) ?? null;


    /// <summary>
    ///     Returns the average grade of the course
    /// </summary>
    [DisplayName("Average Grade")]
    public decimal? AveregaGrade => Enrollments?
        .Where(e => e.DisciplineId == Id)
        .Average(e => e.Grade) ?? null;


    /// <summary>
    ///     Returns the lowest grade of the course
    /// </summary>
    [DisplayName("Lowest Grade")]
    public decimal? LowestGrade => Enrollments?
        .Where(e => e.DisciplineId == Id)
        .Min(e => e.Grade) ?? null;


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
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
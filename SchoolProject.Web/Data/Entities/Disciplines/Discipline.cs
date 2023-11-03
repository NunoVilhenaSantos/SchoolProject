using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
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
/// </summary>
public class Discipline : IEntity, INotifyPropertyChanged
{
    /// <summary>
    ///     The code of the discipline.
    /// </summary>
    [DisplayName("Code")]
    [MaxLength(7,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Code { get; init; }


    /// <summary>
    ///     The name of the discipline.
    /// </summary>
    [Required]
    [MaxLength(100, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    /// <summary>
    ///     The description of the discipline.
    /// </summary>
    [MaxLength(500, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Description { get; set; }


    /// <summary>
    ///     The number of hours of the discipline.
    /// </summary>
    [Required]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public required int Hours { get; set; }


    /// <summary>
    ///     The number of credit points of the discipline.
    ///     The number of ECTS of the discipline.
    ///     The number of ECTS credits of the discipline.
    ///     European Credit Transfer and Accumulation System (ECTS).
    /// </summary>
    [Required]
    [DisplayName("Credit Points")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public required double CreditPoints { get; set; }


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
    public virtual HashSet<CourseDiscipline>? CourseDisciplines { get; set; }

    /// <summary>
    /// </summary>
    [NotMapped]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public IEnumerable<Course>? Courses =>
        CourseDisciplines?.Select(cd => cd.Course).Distinct();

    /// <summary>
    /// </summary>
    [DisplayName("Courses Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int CoursesCount => Courses?.Count() ?? 0;


    // ---------------------------------------------------------------------- //
    // Student Disciplines relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between Disciplines and Students
    /// </summary>
    public virtual HashSet<StudentDiscipline>? StudentDisciplines { get; set; }

    /// <summary>
    /// </summary>
    [NotMapped]
    public IEnumerable<Student>? Students =>
        StudentDisciplines?.Select(sc => sc.Student).Distinct();

    /// <summary>
    /// </summary>
    [DisplayName("Students Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int StudentCount0 => Students?.Count() ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Students Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int StudentCount1 =>
        StudentDisciplines?.Select(sc => sc.Student).Distinct().Count() ?? 0;


    // ---------------------------------------------------------------------- //
    // Teacher Disciplines relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the many-to-many relationship between Teacher and Discipline
    /// </summary>
    public virtual HashSet<TeacherDiscipline>? TeacherDisciplines { get; set; }

    /// <summary>
    ///     Returns the teachers of the course
    /// </summary>
    [NotMapped]
    public IEnumerable<Teacher>? Teachers =>
        TeacherDisciplines?.Select(sc => sc.Teacher).Distinct();

    /// <summary>
    ///     Returns the number of the teachers for this discipline
    /// </summary>
    [DisplayName("Teachers Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int TeachersCount0 => TeacherDisciplines?.Count ?? 0;


    /// <summary>
    ///     Returns the number of the teachers for this discipline
    /// </summary>
    [DisplayName("Teachers Count")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int TeachersCount1 =>
        TeacherDisciplines?.Select(tc => tc.Teacher).Distinct().Count() ?? 0;


    // ---------------------------------------------------------------------- //
    // Enrollments relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///     Navigation property for the one-to-many relationship between Discipline and Enrollment
    /// </summary>
    public virtual HashSet<Enrollment>? Enrollments { get; set; }


    /// <summary>
    /// </summary>
    [NotMapped]
    public IEnumerable<Student>? EStudents =>
        StudentDisciplines?.Select(sc => sc.Student).Distinct();


    /// <summary>
    ///     Returns the number of students enrolled in the course
    /// </summary>
    [DisplayName("Enrolled Students")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int StudentsCount =>
        Enrollments?.Where(e => e.Discipline.Id == Id).Count() ?? 0;


    /// <summary>
    ///     Returns the highest grade of the course
    /// </summary>
    [DisplayName("Highest Grade")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal HighestGrade => Enrollments?
        .Where(e => e.DisciplineId == Id)
        .Max(e => e.Grade) ?? 0;


    /// <summary>
    ///     Returns the average grade of the course
    /// </summary>
    [DisplayName("Average Grade")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal AveregaGrade => Enrollments?
        .Where(e => e.DisciplineId == Id)
        .Average(e => e.Grade) ?? 0;


    /// <summary>
    ///     Returns the lowest grade of the course
    /// </summary>
    [DisplayName("Lowest Grade")]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public decimal LowestGrade => Enrollments?
        .Where(e => e.DisciplineId == Id)
        .Min(e => e.Grade) ?? 0;


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
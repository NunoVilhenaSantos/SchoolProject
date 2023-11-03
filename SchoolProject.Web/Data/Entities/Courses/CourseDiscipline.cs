using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Courses;

/// <summary>
/// </summary>
public class CourseDiscipline : IEntity, INotifyPropertyChanged
{
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //

    /// <summary>
    ///     Foreign Key for Discipline
    /// </summary>
    [Required]
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    public virtual required Course Course { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     Foreign Key for Discipline
    /// </summary>
    [Required]
    [ForeignKey(nameof(Discipline))]
    public int DisciplineId { get; set; }


    /// <summary>
    ///     Foreign Key for Discipline
    /// </summary>
    [Required]
    public virtual required Discipline Discipline { get; set; }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Created By AppUser Id")]
    [ForeignKey(nameof(CreatedBy))]
    public string CreatedById { get; set; }


    /// <summary>
    ///     Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Updated By AppUser Id")]
    [ForeignKey(nameof(UpdatedBy))]
    public string? UpdatedById { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <inheritdoc />
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName("Created By")]
    public virtual required AppUser CreatedBy { get; set; }


    // [Required]
    /// <inheritdoc />
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


    /// <inheritdoc />
    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName("Updated By")]
    public virtual AppUser? UpdatedBy { get; set; }



    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    ///// <summary>
    ///// </summary>
    //// [NotMapped]
    //public virtual HashSet<StudentDiscipline>?
    //    StudentDisciplines
    //{ get; set; }

    ///// <summary>
    /////     Returns the disciplines associated with this teacher
    ///// </summary>
    //[NotMapped]
    //public IEnumerable<Student>? Students =>
    //    StudentDisciplines?.Where(i => i.DisciplineId == DisciplineId).Select(sc => sc.Student).Distinct();


    ///// <summary>
    ///// </summary>
    //[DisplayName("Students Count")]
    //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    //public int StudentsCount => Students?.Count() ?? 0;


    ///// <summary>
    ///// </summary>
    //[DisplayName("Total Students")]
    //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    //public int TotalStudents => Students?.Sum(t => t.TotalStudents) ?? 0;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    ///// <summary>
    ///// </summary>
    //[NotMapped]
    //public virtual HashSet<TeacherDiscipline>? TeacherDisciplines { get; set; }



    ///// <summary>
    /////     Returns the disciplines associated with this teacher
    ///// </summary>
    //[NotMapped]
    //public IEnumerable<Teacher>? Teachers =>
    //    TeacherDisciplines?.Where(i => i.DisciplineId == DisciplineId).Select(td => td.Teacher).Distinct();


    ///// <summary>
    ///// </summary>
    //[DisplayName("Teachers Count")]
    //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    //public int TeachersCount => Teachers?.Count() ?? 0;

    ///// <summary>
    ///// </summary>
    //[DisplayName("Total Teachers")]
    //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    //public int TotalTeachers => TeacherDisciplines?
    //    .Where(e => e.DisciplineId == Id).Count() ?? 0;

    
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
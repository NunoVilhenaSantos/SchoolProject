using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Courses;

/// <summary>
///
/// </summary>
public class CourseDisciplines : IEntity, INotifyPropertyChanged
{
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //

    /// <summary>
    ///     Foreign Key for Discipline
    /// </summary>
    [Required]
    public required int CourseId { get; set; }


    /// <summary>
    ///
    /// </summary>
    [Required]
    [ForeignKey(nameof(CourseId))]
    public virtual required Course Course { get; set; }




    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     Foreign Key for Discipline
    /// </summary>
    [Required]
    public required int DisciplineId { get; set; }

    /// <summary>
    ///     Foreign Key for Discipline
    /// </summary>
    [Required]
    [ForeignKey(nameof(DisciplineId))]
    public virtual required Discipline Discipline { get; set; }



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


    // Deve ser do mesmo tipo da propriedade Id de User
    /// <summary>
    ///
    /// </summary>
    [DisplayName("Created By User Id")]
    public string CreatedById { get; set; }


    /// <inheritdoc />
    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName("Created By")]
    [ForeignKey(nameof(CreatedById))]
    public virtual required User CreatedBy { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // [Required]
    /// <inheritdoc />
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;



    // Deve ser do mesmo tipo da propriedade Id de User
    /// <summary>
    ///
    /// </summary>
    [DisplayName("Updated By User Id")]
    public string? UpdatedById { get; set; }


    /// <inheritdoc />
    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName("Updated By")]
    [ForeignKey(nameof(UpdatedById))]
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
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Enrollments;

/// <summary>
/// </summary>
public class Enrollment : IEntity, INotifyPropertyChanged
{
    /// <summary>
    /// </summary>
    [Required]
    [ForeignKey(nameof(Student))]
    public required int StudentId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public virtual required Student Student { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [ForeignKey(nameof(Discipline))]
    public required int DisciplineId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public virtual required Discipline Discipline { get; set; }


    // [Column(TypeName = "decimal(18,2)")]
    /// <summary>
    /// </summary>
    [Precision(18, 2)]
    public decimal? Grade { get; set; }


    /// <summary>
    /// </summary>
    public required int Absences { get; set; } = 0;


    /// <summary>
    /// </summary>
    public decimal PercentageOfAbsences =>
        (decimal) Absences / Discipline.Hours;


    /// <summary>
    /// Limite padrão de percentagem para reprovação devido a faltas.
    /// </summary>
    public const decimal ThresholdPercentage = 0.2m;


    /// <summary>
    /// Indica se houve reprovação devido às faltas com base em um limite de percentagem.
    /// </summary>
    public bool FailedDueToAbsences
    {
        get
        {
            // Não há reprovação se não houver faltas
            if (Absences == 0) return false;
            return PercentageOfAbsences > ThresholdPercentage;
        }
    }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    /// <inheritdoc />
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
    [DisplayName("Created By")]
    public virtual required AppUser CreatedBy { get; set; }


    /// <inheritdoc />
    // [Required]
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


    /// <summary>
    /// Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Created By AppUser Id")]
    [ForeignKey(nameof(CreatedBy))]
    public required string CreatedById { get; set; }

    /// <summary>
    /// Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Updated By AppUser Id")]
    [ForeignKey(nameof(UpdatedBy))]
    public string? UpdatedById { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


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
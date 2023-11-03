using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Enrollments;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field |
                AttributeTargets.Parameter)]
public class ValidateAbsencesAttribute : ValidationAttribute
{
    private readonly string _otherProperty;

    public ValidateAbsencesAttribute(string otherProperty)
    {
        _otherProperty = otherProperty;
    }

    protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
    {
        var otherPropertyValue = validationContext.ObjectType
            .GetProperty(_otherProperty)
            .GetValue(validationContext.ObjectInstance, null);

        if (value is int absences &&
            otherPropertyValue is int disciplineWorkLoadHours)
            if (absences < 0 || absences > disciplineWorkLoadHours)
                return new ValidationResult(ErrorMessage);

        return ValidationResult.Success;
    }
}



/// <summary>
/// </summary>
public class Enrollment : IEntity, INotifyPropertyChanged
{
    // This constant represents the default threshold percentage for failure due to absences.
    // It is set to 20% (0.2 as a decimal).
    // This value can be adjusted as needed.
    [DisplayFormat(DataFormatString = "{0:P}", ApplyFormatInEditMode = false)]
    [Display(Name = "Threshold Percentage")]
    public const decimal ThresholdPercentage = 0.20m;

    /// <summary>
    /// </summary>
    [Required]
    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }

    /// <summary>
    /// </summary>
    public Guid StudentIdGuid => Student?.IdGuid ?? Guid.Empty;

    /// <summary>
    /// </summary>
    [Required]
    public virtual required Student Student { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [ForeignKey(nameof(Discipline))]
    public int DisciplineId { get; set; }

    /// <summary>
    /// </summary>
    public Guid DisciplineIdGuid => Discipline?.IdGuid ?? Guid.Empty;

    /// <summary>
    /// </summary>
    [Required]
    public virtual required Discipline Discipline { get; set; }


    // This property represents the student's grade.
    // It is formatted as a decimal with precision 18,2 and a range between 0 and 20.
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    [Range(0, 20, ErrorMessage = "Grade must be between 0 and 20.")]
    public decimal? Grade { get; set; }


    // This property represents the number of absences for the student.
    // It is formatted as an integer with custom validation to ensure it's between 0 and DisciplineWorkLoadHours.
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    [ValidateAbsences(nameof(DisciplineWorkLoadHours),
        ErrorMessage =
            $"Absences must be between 0 and {nameof(DisciplineWorkLoadHours)}.")]
    public int Absences { get; set; } = 0;


    // This is a read-only property representing the workload hours for the discipline.
    [DisplayName("Discipline Work Load Hours")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public int DisciplineWorkLoadHours => Discipline?.Hours ?? 0;


    // This is a calculated property representing the percentage of absences relative to the workload hours.
    [DisplayFormat(DataFormatString = "{0:P}", ApplyFormatInEditMode = false)]
    public decimal PercentageOfAbsences => DisciplineWorkLoadHours == 0
        ? 0
        : (decimal) Absences / DisciplineWorkLoadHours;


    /// <summary>
    ///     Indica se houve reprovação devido às faltas com base em um limite de percentagem.
    /// </summary>
    [DisplayName("Failed Due To Absences")]
    public bool FailedDueToAbsences
    {
        get
        {
            // Não há reprovação se não houver faltas
            if (Absences == 0) return false;
            return PercentageOfAbsences > ThresholdPercentage;
        }
    }


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
    [DisplayName("Created By AppUser")]
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
    [DisplayName("Updated By AppUser")]
    public virtual AppUser? UpdatedBy { get; set; }


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
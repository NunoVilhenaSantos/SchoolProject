using SchoolProject.Web.Data.Entities.Enrollments;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;


namespace SchoolProject.Web.Models.Enrollments;



/// <summary>
/// </summary>
public class GradesAssignmentViewModel
{
    /// <summary>
    /// </summary>
    [DisplayName("Student Id")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required int StudentId { get; set; }

    /// <summary>
    /// </summary>
    [DisplayName("Student IdGuid")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required Guid StudentIdGuid { get; set; }

    /// <summary>
    /// </summary>
    [DisplayName("Student Full Name")]
    public required string? StudentFullName { get; set; }

    /// <summary>
    /// </summary>
    [DisplayName("Student List of Enrollments")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required List<EnrollmentViewModel> Enrollments { get; set; }
}



/// <summary>
/// </summary>
public class EnrollmentViewModel
{
    // This constant represents the default threshold percentage for failure due to absences.
    // It is set to 20% (0.2 as a decimal).
    // This value can be adjusted as needed.
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:P}", ApplyFormatInEditMode = false)]
    [Display(Name="Threshold Percentage")]
    public readonly decimal ThresholdPercentage = 0.20m;


    /// <summary>
    /// </summary>
    [DisplayName("Enrollment Id")]
    public required string? DisciplineCode { get; init; }

    /// <summary>
    /// </summary>
    [DisplayName("Discipline Name")]
    public required string? DisciplineName { get; init; }

    /// <summary>
    /// </summary>
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required int EnrollmentId { get; init; }

    /// <summary>
    /// </summary>
    [DisplayName("Enrollment IdGuid")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required Guid EnrollmentIdGuid { get; init; }

    /// <summary>
    /// </summary>
    // [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    // This property represents the student's grade.
    // It is formatted as a decimal with precision 18,2 and a range between 0 and 20.
    [DisplayName("Grade")]
    [Precision(18, 2)]
    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    [Range(0, 20,
        ErrorMessage = "Grades must be between 0 and 20. ")]
    [AllowNull]
    public required decimal? Grade { get; set; }


    /// <summary>
    /// </summary>
    // This property represents the number of absences for the student.
    // It is formatted as an integer with custom validation to ensure it's between 0 and DisciplineWorkLoadHours.
    [DisplayName("Absences")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    [ValidateAbsences(nameof(DisciplineWorkLoadHours),
        ErrorMessage =
            $"Absences must be between 0 and {nameof(DisciplineWorkLoadHours)}.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    [CustomRange(nameof(DisciplineWorkLoadHours),
        ErrorMessage = "O número de faltas deve estar entre 0 e {0}.")]
    public required int Absences { get; set; }


    /// <summary>
    /// </summary>
    [DisplayName("Percentage Of Absences")]
    [DisplayFormat(DataFormatString = "{0:P}", ApplyFormatInEditMode = false)]
    public required decimal? PercentageOfAbsences { get; init; }



    /// <summary>
    /// </summary>
    [DisplayName("Discipline Work Load Hours")]
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public required int? DisciplineWorkLoadHours { get; init; }

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


}

/// <inheritdoc />
public class CustomRangeAttribute : ValidationAttribute
{
    public CustomRangeAttribute(string dependingProperty)
    {
        DependingProperty = dependingProperty;
    }

    private string DependingProperty { get; }


    protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
    {
        var dependingPropertyValue = (int?) validationContext.ObjectType
            .GetProperty(DependingProperty)
            .GetValue(validationContext.ObjectInstance, null);

        if (!dependingPropertyValue.HasValue || value is not int intValue)
            return ValidationResult.Success;

        if (intValue < 0 || intValue > dependingPropertyValue.Value)
            return new ValidationResult(
                string.Format(ErrorMessage, dependingPropertyValue.Value),
                new[] {validationContext.MemberName});

        return ValidationResult.Success;
    }
}
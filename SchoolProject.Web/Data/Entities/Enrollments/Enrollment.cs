using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Entities.Enrollments;

public class Enrollment : IEntity, INotifyPropertyChanged
{
    [Required] public required Student Student { get; set; }
    [Required] public int StudentId => Student.Id;


    [Required] public required Course Course { get; set; }
    [Required] public int CourseId => Course.Id;


    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)] public decimal? Grade { get; set; }


    [Required] public int Id { get; set; }

    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("EnrollmentId")]
    public required Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public required bool WasDeleted { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    public required DateTime CreatedAt { get; set; }

    [DisplayName("Created By")] public required User CreatedBy { get; set; }


    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    public DateTime? UpdatedAt { get; set; }

    [DisplayName("Updated By")] public User? UpdatedBy { get; set; }


    public event PropertyChangedEventHandler? PropertyChanged;


    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }


    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
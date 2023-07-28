using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClass : IEntity, INotifyPropertyChanged
{
    [Required]
    [DisplayName("Class Acronym")]
    public required string ClassAcronym { get; set; }

    [Required]
    [DisplayName("Class Name")]
    public required string ClassName { get; set; }


    // old version
    // [Required]
    // [DisplayName("Start Date")]
    // [DataType(DataType.Date)]
    // public DateOnly StartDate { get; set; }

    // [Required]
    // [DataType(DataType.Date)]
    // [DisplayName("End Date")]
    // public DateOnly EndDate { get; set; }


    // [Required]
    // [DisplayName("Start Hour")]
    // [DataType(DataType.Time)]
    // public TimeOnly StartHour { get; set; }

    // [Required]
    // [DataType(DataType.Time)]
    // [DisplayName("End Hour")]
    // public TimeOnly EndHour { get; set; }

    // new version
    [Required]
    [DisplayName("Start Date")]
    [DataType(DataType.Date)]
    public required DateTime StartDate { get; set; }

    [Required]
    [DisplayName("End Date")]
    [DataType(DataType.Date)]
    public required DateTime EndDate { get; set; }

    [Required]
    [DisplayName("Start Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan StartHour { get; set; }

    [Required]
    [DisplayName("End Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan EndHour { get; set; }


    public string? Location { get; set; }

    public string? Type { get; set; }

    public string? Area { get; set; }


    [DisplayName("Profile Photo")] public Guid? ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/schoolclasses/" +
          ProfilePhotoId;


    public ICollection<Course>? Courses { get; set; } = new List<Course>();


    [DisplayName("Courses Count")]
    public int? CoursesCount => Courses?.Count ?? 0;


    [DisplayName("SchoolClass Credits")]
    public int? SchoolClassCredits => Courses?.Sum(c => c.Credits) ?? 0;


    [DisplayName("Work Hour Load")]
    public int? WorkHourLoad => Courses?.Sum(c => c.WorkLoad) ?? 0;


    public ICollection<Student>? Students { get; set; } = new List<Student>();


    [DisplayName("Students Count")]
    public int? StudentsCount => Students?.Count ?? 0;


    [DisplayName("Enrollment")]
    public IEnumerable<Enrollment>? Enrollment { get; set; }


    [DisplayName("Class Average")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? ClassAverage =>
        Enrollment?.Where(e => e.Grade.HasValue)
            .Average(e => e.Grade);


    [DisplayName("Highest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? HighestGrade =>
        Enrollment?.Max(e => e.Grade);


    [DisplayName("Lowest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? LowestGrade =>
        Enrollment?.Min(e => e.Grade);


    [DisplayName("Courses Count")]
    public int ECoursesCount =>
        Enrollment?.Select(e => e.Course).Distinct().Count() ?? 0;


    [DisplayName("Work Hour Load")]
    public int EWorkHourLoad =>
        Enrollment?.Sum(e => e.Course.WorkLoad) ?? 0;


    [DisplayName("Students Count")]
    public int EStudentsCount =>
        Enrollment?.Select(e => e.Student).Distinct().Count() ?? 0;


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // [Column("SchoolClassId")]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public required bool WasDeleted { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    public required DateTime CreatedAt { get; set; }

    [DisplayName("Created By")] public required User CreatedBy { get; set; }


    [Required]
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
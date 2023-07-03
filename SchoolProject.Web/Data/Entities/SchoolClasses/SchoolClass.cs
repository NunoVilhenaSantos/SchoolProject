using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClass : IEntity //: INotifyPropertyChanged
{
    [Required]
    [DisplayName("Class Acronym")]
    public string ClassAcronym { get; set; }

    [Required] [DisplayName("Class Name")] public string ClassName { get; set; }


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
    public DateTime StartDate { get; set; }

    [Required]
    [DisplayName("End Date")]
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    [Required]
    [DisplayName("Start Hour")]
    [DataType(DataType.Time)]
    public TimeSpan StartHour { get; set; }

    [Required]
    [DisplayName("End Hour")]
    [DataType(DataType.Time)]
    public TimeSpan EndHour { get; set; }

    public string Location { get; set; }

    public string Type { get; set; }

    public string Area { get; set; }


    [DisplayName("Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/supershoptpsicet77-nuno/courses/" +
          ProfilePhotoId;


    [Required] public int Id { get; init; }

    [Required]
    [Key]
    [Column("SchoolClassId")]
    public Guid IdGuid { get; init; }

    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    public DateTime CreatedAt { get; init; }

    [DisplayName("Created By")] public User CreatedBy { get; init; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    public DateTime? UpdatedAt { get; set; }

    [DisplayName("Updated By")] public User? UpdatedBy { get; set; }


    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Student> Students { get; set; } = new List<Student>();

    public ICollection<Enrollment> Enrollments { get; set; } =
        new List<Enrollment>();


    [DisplayName("Courses Count")]
    public int? CoursesCount => Courses?.Count ?? 0;

    [DisplayName("Work Hour Load")]
    public int? WorkHourLoad => Courses?.Sum(c => c.WorkLoad) ?? 0;

    [DisplayName("Students Count")]
    public int? StudentsCount => Students?.Count ?? 0;


    [DisplayName("Class Average")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? ClassAverage =>
        Enrollments?.Average(e => e.Grade) ?? 0;


    [DisplayName("Highest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? HighestGrade => Enrollments?.Max(e => e.Grade) ?? 0;


    [DisplayName("Lowest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? LowestGrade => Enrollments?.Min(e => e.Grade) ?? 0;
}
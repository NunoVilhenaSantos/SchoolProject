using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public class SchoolClass : IEntity //: INotifyPropertyChanged
{
    [Required]
    [DisplayName("Class Acronym")]
    public string ClassAcronym { get; set; }

    [Required]
    [DisplayName("Class Name")]
    public string ClassName { get; set; }


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


    [DisplayName("Courses Count")] public int? CoursesCount { get; set; }

    [DisplayName("Work Hour Load")] public int? WorkHourLoad { get; set; }

    [DisplayName("Students Count")] public int? StudentsCount { get; set; }

    [DisplayName("Class Average")] public decimal? ClassAverage { get; set; }

    [DisplayName("Highest Grade")] public decimal? HighestGrade { get; set; }

    [DisplayName("Lowest Grade")] public decimal? LowestGrade { get; set; }


    [DisplayName("Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/supershoptpsicet77-nuno/courses/" +
          ProfilePhotoId;


    [Required] [Key] public int Id { get; set; }

    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
}
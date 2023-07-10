using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Web.Data.Entities.Courses;

public class Course : IEntity // : INotifyPropertyChanged
{
    [Required] public required string Name { get; set; }


    [Required] public int WorkLoad { get; set; }


    [Required] public int Credits { get; set; }


    [DisplayName("Students Count")] public int? StudentsCount { get; set; }


    [DisplayName("Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/supershoptpsicet77-nuno/courses/" +
          ProfilePhotoId;


    [Required] public int Id { get; init; }
    [Required] [Key] [Column("CourseId")] public Guid IdGuid { get; init; }

    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    public DateTime CreatedAt { get; init; }

    [DisplayName("Created By")] public required User CreatedBy { get; init; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    public DateTime? UpdatedAt { get; set; }

    [DisplayName("Updated By")] public User? UpdatedBy { get; set; }
}
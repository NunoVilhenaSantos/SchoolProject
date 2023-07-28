using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Data.Entities.Teachers;

public class TeacherCourse : IEntity
{
    [Required] public required int TeacherId { get; set; }

    [Required] public required Teacher Teacher { get; set; }


    [Required] public required int CourseId { get; set; }

    [Required] public required Course Course { get; set; }


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // [Column("TeacherCourseId")]
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
}
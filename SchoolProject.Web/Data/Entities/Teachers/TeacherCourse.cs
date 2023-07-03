using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Data.Entities.Teachers;

public class TeacherCourse : IEntity
{
    [Required] public int TeacherId { get; set; }

    [Required] public Teacher Teacher { get; set; }


    [Required] public int CourseId { get; set; }
    [Required] public Course Course { get; set; }




    [Required] public int Id { get; init; }
    [Required] [Key] [Column("TeacherCourseId")] public Guid IdGuid { get; init; }

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



    public TeacherCourse()
    {
        IdGuid = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
}
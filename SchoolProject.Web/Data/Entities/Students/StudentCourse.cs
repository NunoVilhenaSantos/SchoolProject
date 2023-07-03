using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Data.Entities.Students;

public class StudentCourse : IEntity
{
    [Required] public int StudentId { get; set; }
    [Required] public Student Student { get; set; }


    [Required] public int CourseId { get; set; }
    [Required] public Course Course { get; set; }



    [Required] public int Id { get; init; }
    [Required] [Key] [Column("StudentCourseId")] public Guid IdGuid { get; init; }

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



    public StudentCourse()
    {
        IdGuid = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }


}
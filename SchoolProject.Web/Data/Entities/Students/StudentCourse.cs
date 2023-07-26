using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Data.Entities.Students;

public class StudentCourse : IEntity
{
    public StudentCourse()
    {
        IdGuid = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    [Required] public int StudentId { get; set; }
    [Required] public Student Student { get; set; }


    [Required] public int CourseId { get; set; }
    [Required] public Course Course { get; set; }


    [Required] public int Id { get; set; }

    [Required]
    [Key]
    [Column("StudentCourseId")]
    public Guid IdGuid { get; set; }

    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Created By")] public User CreatedBy { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    public DateTime? UpdatedAt { get; set; }

    [DisplayName("Updated By")] public User? UpdatedBy { get; set; }
}
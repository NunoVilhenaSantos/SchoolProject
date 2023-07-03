using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Courses;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClassCourse : IEntity
{
    [Required] public int SchoolClassId { get; set; }
    [Required] public SchoolClass SchoolClass { get; set; }

    [Required] public int CourseId { get; set; }
    [Required] public Course Course { get; set; }


    [Required] public int Id { get; init; }

    [Required]
    [Key]
    [Column("SchoolClassCourseId")]
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


    public SchoolClassCourse()
    {
        IdGuid = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }
}
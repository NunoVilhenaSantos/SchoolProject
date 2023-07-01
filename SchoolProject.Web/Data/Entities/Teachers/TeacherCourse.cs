using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities.Teachers;

public class TeacherCourse : IEntity
{
    [Required] public int TeacherId { get; set; }
    [Required] public int CourseId { get; set; }


    [Required] [Key] public int Id { get; set; }
    public Guid IdGuid { get; set; }
    [Required] public bool WasDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public User CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User UpdatedBy { get; set; }
}
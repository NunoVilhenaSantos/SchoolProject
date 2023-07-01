using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClassCourse : IEntity
{
    [Required] public int SchoolClassId { get; set; }

    [Required] public int CourseId { get; set; }

    [Required] [Key] public int Id { get; set; }
    public Guid IdGuid { get; set; }
    [Required] public bool WasDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public User CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User UpdatedBy { get; set; }
}
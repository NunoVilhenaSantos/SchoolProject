using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities.Teachers;

public class TeacherCourse : IEntity
{
    [Required] public int TeacherId { get; set; }
    [Required] public int CourseId { get; set; }


    [Required] [Key] public int Id { get; set; }
    [Required] public bool WasDeleted { get; set; }
}
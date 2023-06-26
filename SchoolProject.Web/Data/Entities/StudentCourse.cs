using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public class StudentCourse : IEntity
{
    [Required] public int StudentId { get; set; }
    [Required] public int CourseId { get; set; }

    [Required] [Key] public int Id { get; set; }
    [Required] public bool WasDeleted { get; set; }
}
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Entities.Enrollments;

public class Enrollment : IEntity //: INotifyPropertyChanged
{
    [Required] public Student Student { get; set; }
    //public int StudentId { get; set; }


    [Required] public Course Course { get; set; }
    //public int CourseId { get; set; }


    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)] public decimal? Grade { get; set; }


    [Required] [Key] public int Id { get; set; }
    public Guid IdGuid { get; set; }

    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public User CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User UpdatedBy { get; set; }
}
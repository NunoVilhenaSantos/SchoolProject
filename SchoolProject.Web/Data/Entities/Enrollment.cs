using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public class Enrollment : IEntity //: INotifyPropertyChanged
{
    public Student Student { get; set; }
    // public int StudentId { get; set; }


    public Course Course { get; set; }
    // public int CourseId { get; set; }


    public decimal? Grade { get; set; }


    [Required] [Key] public int Id { get; set; }

    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
}
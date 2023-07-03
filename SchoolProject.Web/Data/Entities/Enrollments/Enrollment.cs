using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Entities.Enrollments;

public class Enrollment : IEntity //: INotifyPropertyChanged
{
    public int StudentId { get; set; }
    [Required] public Student Student { get; set; }


    public int CourseId { get; set; }
    [Required] public Course Course { get; set; }


    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)] public decimal? Grade { get; set; }



    [Required] public int Id { get; init; }
    [Required] [Key] [Column("EnrollmentId")] public Guid IdGuid { get; init; }

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





}
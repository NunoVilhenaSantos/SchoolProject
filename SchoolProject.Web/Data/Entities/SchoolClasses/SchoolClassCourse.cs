using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClassCourse : IEntity
{
    [Required] public required SchoolClass SchoolClass { get; set; }

    // public int SchoolClassId => SchoolClass.Id;
    public Guid SchoolClassGuidId => SchoolClass.IdGuid;


    [Required] public required Course Course { get; set; }

    // public int CourseId => Course.Id;
    public Guid CourseGuidId => Course.IdGuid;


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // [Column("SchoolClassCourseId")]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public required bool WasDeleted { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    public required DateTime CreatedAt { get; set; }


    [DisplayName("Created By")] public required User CreatedBy { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    public DateTime? UpdatedAt { get; set; }


    [DisplayName("Updated By")] public User? UpdatedBy { get; set; }
}
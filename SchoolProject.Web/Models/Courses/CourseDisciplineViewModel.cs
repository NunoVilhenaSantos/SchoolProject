using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Courses;

/// <summary>
/// </summary>
public class CourseDisciplineViewModel
{
    public required int Id { get; set; }


    public required Guid IdGuid { get; set; }


    [DisplayName("Course Id")] public required int CourseId { get; set; }

    [DisplayName("Course IdGuid")]
    public required Guid CourseIdGuid { get; set; }

    [DisplayName("Course Code")] public required string CourseCode { get; set; }

    [DisplayName("Course Acronym")]
    public required string CourseAcronym { get; set; }

    [DisplayName("Course Name")] public required string CourseName { get; set; }


    [DisplayName("Discipline Id")]
    public required int DisciplineId { get; set; }

    [DisplayName("Discipline IdGuid")]
    public required Guid DisciplineIdGuid { get; set; }

    [DisplayName("Discipline Code")]
    public required string DisciplineCode { get; set; }


    [DisplayName("Discipline Name ")]
    public required string DisciplineName { get; set; }


    [DisplayName("Was Deleted?")]
    public required string DisciplineDescription { get; set; }


    [DisplayName("Was Deleted?")] public required bool WasDeleted { get; set; }


    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    public required DateTime CreatedAt { get; set; }


    [DisplayName("Created By")]
    public virtual required string CreatedByFullName { get; set; }


    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    public required DateTime? UpdatedAt { get; set; }


    [DisplayName("Updated By")]
    public virtual required string UpdatedByFullName { get; set; }
}
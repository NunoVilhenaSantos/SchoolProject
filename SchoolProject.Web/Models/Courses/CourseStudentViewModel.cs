using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Courses;

/// <summary>
/// </summary>
public class CourseStudentViewModel
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


    [DisplayName("Student Id")] public required int StudentId { get; set; }

    [DisplayName("Student IdGuid")]
    public required Guid StudentIdGuid { get; set; }

    [DisplayName("Student Full Name")]
    public required string StudentFullName { get; set; }

    [DisplayName("Student Mobile Phone Number")]
    public required string StudentMobilePhone { get; set; }


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
using CsvHelper.Configuration.Attributes;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Courses;

/// <summary>
///
/// </summary>
public class CourseDto
{
    /// <summary>
    ///
    /// </summary>
    public required int Id { get; set; }


    /// <summary>
    ///
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    ///
    /// </summary>
    public required string Acronym { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required string Name { get; set; }


    /// <summary>
    ///
    /// </summary>
    public required byte QnqLevel { get; init; }

    /// <summary>
    ///
    /// </summary>
    public required byte EqfLevel { get; init; }


    /// <summary>
    ///
    /// </summary>
    public required DateTime StartDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required DateTime EndDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required TimeSpan StartHour { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required TimeSpan EndHour { get; set; }


    /// <summary>
    ///
    /// </summary>
    public required decimal PriceForEmployed { get; set; }

    /// <summary>
    ///
    /// </summary>
    public required decimal PriceForUnemployed { get; set; }


    /// <summary>
    ///
    /// </summary>
    public required Guid ProfilePhotoId { get; set; }

    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? StorageHelper.NoImageUrl
        : StorageHelper.AzureStoragePublicUrl +
          CoursesController.BucketName +
          "/" + ProfilePhotoId;




    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    /// </summary>
    public required List<KeyValuePair<string, string>>
        CourseDisciplines { get; set; }


    //// ---------------------------------------------------------------------- //
    //// Navigation property for the many-to-many relationship
    //// ---------------------------------------------------------------------- //

    /// <summary>
    /// </summary>
    public required List<KeyValuePair<string, string>>
        CourseStudents { get; set; }

    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Navigation property for the one-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     List of Disciplines for this Discipline
    /// </summary>
    public required List<KeyValuePair<string, string>> Disciplines { get; set; }


    /// <summary>
    ///     List of Students for this Discipline
    /// </summary>
    public required List<KeyValuePair<string, string>> Students { get; set; }


    /// <summary>
    ///
    /// </summary>
    public required int StudentsCount { get; set; }


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///
    /// </summary>
    public required List<KeyValuePair<string, string>> Enrollment { get; set; }

    public required decimal? ClassAverage { get; set; }

    public required decimal? HighestGrade { get; set; }

    public required decimal? LowestGrade { get; set; }

    public required int ECoursesCount { get; set; }

    public required int EWorkHourLoad { get; set; }

    public required int EStudentsCount { get; set; }

    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Map to DTO
    //
    // Mapear para DTOs (Data Transfer Objects)
    //
    // é uma abordagem comum para resolver problemas de referência circular
    // durante a serialização JSON e para fornecer mais
    // controle sobre os dados que são retornados por uma API.
    //
    // Aqui está um exemplo simples de como você pode fazer isso:
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///
    /// </summary>
    /// <param name="course"></param>
    /// <returns></returns>
    public static CourseDto MapToDto(Course course)
    {
        return new CourseDto
        {
            Id = course.Id,
            Code = course.Code,
            Acronym = course.Acronym,
            Name = course.Name,
            QnqLevel = course.QnqLevel,
            EqfLevel = course.EqfLevel,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            StartHour = course.StartHour,
            EndHour = course.EndHour,
            PriceForEmployed = course.Id,
            PriceForUnemployed = course.Id,
            ProfilePhotoId = course.ProfilePhotoId,
            // ProfilePhotoIdUrl = course.ProfilePhotoIdUrl,

            CourseDisciplines = course.CourseDisciplines
                .Where(e => e.CourseId == course.Id)
                .Select(e =>
                    new KeyValuePair<string, string>(e.Discipline.Code,
                        e.Discipline.Name))
                .ToList(),

            CourseStudents = course.CourseStudents
                .Where(e => e.CourseId == course.Id)
                .Select(e =>
                    new KeyValuePair<string, string>(e.Student.FirstName,
                        e.Student.LastName))
                .ToList(),

            Disciplines = course.Disciplines
                .Select(e =>
                    new KeyValuePair<string, string>(e.Code,
                        e.Name))
                .ToList(),

            Students = course.Students
                .Select(e =>
                    new KeyValuePair<string, string>(e.FirstName,
                        e.LastName))
                .ToList(),

            Enrollment = course.Enrollment
                .Select(e =>
                    new KeyValuePair<string, string>(e.Discipline.Code,
                        e.Discipline.Name))
                .ToList(),

            StudentsCount = course.StudentsCount,
            ClassAverage = course.ClassAverage,
            HighestGrade = course.HighestGrade,
            LowestGrade = course.LowestGrade,
            ECoursesCount = course.ECoursesCount,
            EWorkHourLoad = course.EWorkHourLoad,
            EStudentsCount = course.EStudentsCount,
        };
    }
}
﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Courses;

/// <summary>
/// </summary>
public class CourseDto
{
    /// <summary>
    /// </summary>
    public required int Id { get; set; }


    /// <summary>
    /// </summary>
    [MaxLength(7, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Code { get; init; }

    /// <summary>
    /// </summary>
    [MaxLength(20, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Acronym { get; set; }

    /// <summary>
    /// </summary>
    [MaxLength(100, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    /// <summary>
    /// </summary>
    public required byte QnqLevel { get; init; }

    /// <summary>
    /// </summary>
    public required byte EqfLevel { get; init; }


    /// <summary>
    /// </summary>
    public required DateTime StartDate { get; set; }

    /// <summary>
    /// </summary>
    public required DateTime EndDate { get; set; }

    /// <summary>
    /// </summary>
    public required TimeSpan StartHour { get; set; }

    /// <summary>
    /// </summary>
    public required TimeSpan EndHour { get; set; }


    /// <summary>
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public required decimal PriceForEmployed { get; set; }

    /// <summary>
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
    public required decimal PriceForUnemployed { get; set; }


    /// <summary>
    /// </summary>
    public required Guid ProfilePhotoId { get; set; } = Guid.Empty;

    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    [DisplayName("Profile Photo")]
    public string ProfilePhotoIdUrl =>
        ProfilePhotoId == Guid.Empty || ProfilePhotoId == null
            ? StorageHelper.NoImageUrl
            : StorageHelper.AzureStoragePublicUrl +
              CoursesController.BucketName +
              "/" + ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //


    /// <summary>
    /// </summary>
    public required List<KeyValuePair<string, string>>?
        CourseDisciplines { get; set; }


    //// ---------------------------------------------------------------------- //
    //// Navigation property for the many-to-many relationship
    //// ---------------------------------------------------------------------- //

    /// <summary>
    /// </summary>
    public required List<KeyValuePair<string, string>>?
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
    public required List<KeyValuePair<string, string>>? Disciplines
    {
        get;
        set;
    }


    /// <summary>
    ///     List of Students for this Discipline
    /// </summary>
    public required List<KeyValuePair<string, string>>? Students { get; set; }


    /// <summary>
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public required int StudentsCount { get; set; }


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // ---------------------------------------------------------------------- //

    /// <summary>
    /// </summary>
    public required List<KeyValuePair<string, string>>? Enrollment { get; set; }

    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public required decimal? ClassAverage { get; set; }

    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public required decimal? HighestGrade { get; set; }

    [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
    public required decimal? LowestGrade { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public required int ECoursesCount { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    public required int EWorkHourLoad { get; set; }

    [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
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

            CourseDisciplines = course.CourseDisciplines?
                .Where(e => e.CourseId == course.Id)
                .Select(e =>
                    new KeyValuePair<string, string>
                        (e.Discipline.Code, e.Discipline.Name))
                .ToList(),

            CourseStudents = course.CourseStudents?
                .Where(e => e.CourseId == course.Id)
                .Select(e =>
                    new KeyValuePair<string, string>
                        (e.Student.FirstName, e.Student.LastName))
                .ToList(),

            Disciplines = course.CourseDisciplines?
                .Select(e =>
                    new KeyValuePair<string, string>
                        (e.Discipline.Code, e.Discipline.Name))
                .ToList(),

            Students = course.CourseStudents?
                .Select(e =>
                    new KeyValuePair<string, string>
                        (e.Student.FirstName, e.Student.LastName))
                .ToList(),

            Enrollment = course.Enrollments?
                .Select(e =>
                    new KeyValuePair<string, string>
                        (e.Discipline.Code, e.Discipline.Name))
                .ToList(),

            StudentsCount = course.StudentsCount,
            ClassAverage = course.ClassAverage,
            HighestGrade = course.HighestGrade,
            LowestGrade = course.LowestGrade,
            ECoursesCount = course.ECoursesCount,
            EWorkHourLoad = course.EWorkHourLoad,
            EStudentsCount = course.EStudentsCount
        };
    }
}
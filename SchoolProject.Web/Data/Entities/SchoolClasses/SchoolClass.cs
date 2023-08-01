﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.EntitiesMatrix;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClass : IEntity, INotifyPropertyChanged
{
    [DisplayName("Code")]
    [MaxLength(7,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Code { get; init; }


    [DisplayName("Class Acronym")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Acronym { get; set; }


    [DisplayName("Class Name")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Name { get; set; }


    [DisplayName("QNQ Level")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required byte QnqLevel { get; init; }


    [DisplayName("EQF Level")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required byte EqfLevel { get; init; }

    // old version
    // [Required]
    // [DisplayName("Start Date")]
    // [DataType(DataType.Date)]
    // public DateOnly StartDate { get; set; }

    // [Required]
    // [DataType(DataType.Date)]
    // [DisplayName("End Date")]
    // public DateOnly EndDate { get; set; }


    // [Required]
    // [DisplayName("Start Hour")]
    // [DataType(DataType.Time)]
    // public TimeOnly StartHour { get; set; }

    // [Required]
    // [DataType(DataType.Time)]
    // [DisplayName("End Hour")]
    // public TimeOnly EndHour { get; set; }

    // new version
    [Required]
    [DisplayName("Start Date")]
    [DataType(DataType.Date)]
    public required DateTime StartDate { get; set; }

    [Required]
    [DisplayName("End Date")]
    [DataType(DataType.Date)]
    public required DateTime EndDate { get; set; }

    [Required]
    [DisplayName("Start Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan StartHour { get; set; }

    [Required]
    [DisplayName("End Hour")]
    [DataType(DataType.Time)]
    public required TimeSpan EndHour { get; set; }


    public string? Location { get; set; }

    public string? Type { get; set; }

    public string? Area { get; set; }


    [Required]
    [Precision(10, 2)]
    [DataType(DataType.Currency)]
    [DisplayName("Price for Employed")]
    public required decimal PriceForEmployed { get; set; } = 200;


    [Required]
    [Precision(10, 2)]
    [DataType(DataType.Currency)]
    [DisplayName("Price for Unemployed")]
    public required decimal PriceForUnemployed { get; set; } = 0;


    [DisplayName("Profile Photo")] public Guid? ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/schoolclasses/" +
          ProfilePhotoId;


    public ICollection<Course>? Courses { get; set; } = new List<Course>();


    [DisplayName("Courses Count")]
    public int? CoursesCount => Courses?.Count ?? 0;


    [DisplayName("SchoolClass Credits")]
    public double? SchoolClassCredits => Courses?.Sum(c => c.Credits) ?? 0;


    [DisplayName("Work Hour Load")]
    public int? WorkHourLoad => Courses?.Sum(c => c.WorkLoad) ?? 0;


    public ICollection<Student>? Students { get; set; } = new List<Student>();


    [DisplayName("Students Count")]
    public int? StudentsCount => Students?.Count ?? 0;


    [DisplayName("Enrollment")]
    public IEnumerable<Enrollment>? Enrollment { get; set; }


    [DisplayName("Class Average")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? ClassAverage =>
        Enrollment?.Where(e => e.Grade.HasValue)
            .Average(e => e.Grade);


    [DisplayName("Highest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? HighestGrade =>
        Enrollment?.Max(e => e.Grade);


    [DisplayName("Lowest Grade")]
    // [Column(TypeName = "decimal(18,2)")]
    [Precision(18, 2)]
    public decimal? LowestGrade =>
        Enrollment?.Min(e => e.Grade);


    [DisplayName("Courses Count")]
    public int ECoursesCount =>
        Enrollment?.Select(e => e.Course).Distinct().Count() ?? 0;


    [DisplayName("Work Hour Load")]
    public int EWorkHourLoad =>
        Enrollment?.Sum(e => e.Course.WorkLoad) ?? 0;


    [DisplayName("Students Count")]
    public int EStudentsCount =>
        Enrollment?.Select(e => e.Student).Distinct().Count() ?? 0;


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Created By")] public required User CreatedBy { get; set; }


    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


    [DisplayName("Updated By")] public User? UpdatedBy { get; set; }


    public event PropertyChangedEventHandler? PropertyChanged;


    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }


    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
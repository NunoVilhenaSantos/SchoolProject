using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration.Attributes;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Genders;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Students;

/// <summary>
///  Student class for ef
/// </summary>
public class Student : IEntity, INotifyPropertyChanged
{
    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("First Name")]
    public required string FirstName { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Last Name")]
    public required string LastName { get; set; }


    /// <summary>
    /// </summary>
    [DisplayName("Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    /// <summary>
    /// </summary>
    [Required]
    public required string Address { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Postal Code")]
    public required string PostalCode { get; set; }


    /// <summary>
    ///     The country Id.
    /// </summary>
    // [Required]
    [NotMapped]
    [DisplayName("Country")]
    // [ForeignKey(nameof(City))]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a country...")]
    public required int CountryId { get; set; }


    /// <summary>
    ///
    /// </summary>
    // [Required]
    [ForeignKey(nameof(City))]
    public int CityId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public required City City { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Mobile Phone")]
    public required string MobilePhone { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    public required bool Active { get; set; } = true;


    /// <summary>
    /// </summary>
    // [Required]
    [ForeignKey(nameof(Gender))]
    public int GenderId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public required Gender Gender { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Date Of Birth")]
    [DataType(DataType.Date)]
    public required DateTime DateOfBirth { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Identification Number")]
    public required string IdentificationNumber { get; set; }

    /// <summary>
    /// </summary>
    public required string IdentificationType { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Expiration Date Identification Number")]
    [DataType(DataType.Date)]
    public required DateTime ExpirationDateIdentificationNumber { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Tax Identification Number")]
    public required string TaxIdentificationNumber { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    // [Required]
    /// <summary>
    ///
    /// </summary>
    [ForeignKey(nameof(CountryOfNationality))]
    [DisplayName("Country Of Nationality")]
    public int CountryOfNationalityId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Country Of Nationality")]
    public required Country CountryOfNationality { get; set; }


    /// <summary>
    ///
    /// </summary>
    [ForeignKey(nameof(Birthplace))]
    public int BirthplaceId { get; set; }

    /// <summary>
    ///
    /// </summary>
    [Required]
    public required Country Birthplace { get; set; }


    /// <summary>
    /// </summary>
    [Required]
    [DisplayName("Enroll Date")]
    [DataType(DataType.Date)]
    public required DateTime EnrollDate { get; set; } = DateTime.UtcNow;


    /// <summary>
    ///
    /// </summary>
    [ForeignKey(nameof(AppUser))]
    public int UserId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public required AppUser AppUser { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //

    /// <summary>
    ///     The image of the appUser file from the form to be inserted in the database.
    /// </summary>
    [Ignore]
    [NotMapped]
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }


    /// <summary>
    ///     The profile photo of the appUser.
    /// </summary>
    [DisplayName("Profile Photo")]
    public Guid? ProfilePhotoId { get; set; }

    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? StorageHelper.NoImageUrl
        : StorageHelper.AzureStoragePublicUrl +
          StudentsController.BucketName +
          "/" + ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and Disciplines
    // ---------------------------------------------------------------------- //

    /// <summary>
    ///
    /// </summary>
    public IEnumerable<Discipline>? Disciplines { get; set; }


    // ---------------------------------------------------------------------- //


    /// <summary>
    ///
    /// </summary>
    public IEnumerable<Course>? Courses { get; set; }


    // ---------------------------------------------------------------------- //


    // [DisplayName("Disciplines")]
    // public virtual HashSet<Course>? Courses { get; set; }


    /// <summary>
    ///
    /// </summary>
    [DisplayName("Courses")]
    public virtual HashSet<StudentCourse>? StudentCourses { get; set; }


    // [DisplayName("Disciplines Count")]
    // public int CoursesCount =>
    //     Courses?.Where(s => s.CoursesCount > 0).Count() ?? 0;
    //
    //
    // [DisplayName("Total Work Hours")]
    // public int TotalWorkHours =>
    //     Courses?.Sum(t => t.WorkHourLoad ?? 0) ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and Discipline
    // ---------------------------------------------------------------------- //

    /// <summary>
    /// </summary>
    [DisplayName("Discipline")]
    public virtual HashSet<CourseStudents>? CourseStudents { get; set; }


    /// <summary>
    /// </summary>
    [DisplayName("Course Count")]
    public int CourseStudentsCount => CourseStudents?.Count ?? 0;


    // /// <summary>
    // ///
    // /// </summary>
    // [DisplayName("Discipline With Disciplines Count")]
    // public int ScsCoursesCount
    // {
    //     get
    //     {
    //         if (CourseStudents == null) return 0;
    //
    //         var count = 0;
    //
    //         foreach (var scs in CourseStudents)
    //             if (scs is {Course: not null})
    //                 count += scs.Course(c => c != null);
    //
    //         return count;
    //     }
    // }


    // /// <summary>
    // ///
    // /// </summary>
    // [DisplayName("Total Work Hours")]
    // public int ScsTotalWorkHours
    // {
    //     get
    //     {
    //         if (SchoolClassStudents == null) return 0;
    //
    //         var totalWorkHours = 0;
    //
    //         foreach (var scs in SchoolClassStudents)
    //             if (scs is {SchoolClass: not null})
    //             {
    //                 var courses = scs.SchoolClass.Courses;
    //
    //                 if (courses != null)
    //                     totalWorkHours += courses.Where(c => c != null)
    //                         .Sum(c => c.Hours);
    //             }
    //
    //         return totalWorkHours;
    //     }
    // }


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and Disciplines
    //
    // Using the Enrollment entity
    // ---------------------------------------------------------------------- //

    /// <summary>
    /// </summary>
    public virtual HashSet<Enrollment>? Enrollments { get; set; }


    /// <summary>
    /// </summary>
    [DisplayName("Disciplines Count")]
    public int? CoursesCountEnrollments =>
        Enrollments?.Where(e => e.Discipline.Id == Id).Count() ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Total Work Hours")]
    public int? TotalWorkHoursEnrollments =>
        Enrollments?.Sum(e => e.Discipline.Hours) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Highest Grade")]
    public decimal? HighestGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Max(e => e.Grade) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Average Grade")]
    public decimal? AveregaGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Average(e => e.Grade) ?? 0;


    /// <summary>
    /// </summary>
    [DisplayName("Lowest Grade")]
    public decimal? LowestGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Min(e => e.Grade) ?? 0;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <inheritdoc />
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    /// <inheritdoc />
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    /// <inheritdoc />
    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    /// <inheritdoc />
    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc />
    [Required]
    [DisplayName("Created By")]
    public virtual required AppUser CreatedBy { get; set; }


    /// <inheritdoc />
    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc />
    [DisplayName("Updated By")]
    public virtual AppUser? UpdatedBy { get; set; }

    DateTime IEntity.CreatedAt
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    // ---------------------------------------------------------------------- //
    // Property Changed Event Handler
    // ---------------------------------------------------------------------- //


    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;


    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }

    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
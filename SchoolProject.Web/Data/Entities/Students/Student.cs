using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Students;

public class Student : IEntity //: INotifyPropertyChanged
{
    [Required]
    [DisplayName("First Name")]
    public required string FirstName { get; set; }


    [Required]
    [DisplayName("Last Name")]
    public required string LastName { get; set; }


    [DisplayName("Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    [Required] public required string Address { get; set; }


    [Required]
    [DisplayName("Postal Code")]
    public required string PostalCode { get; set; }


    [Required] public required City City { get; set; }


    [Required]
    // [ForeignKey("CountryId")]
    public required Country Country { get; set; }

    // public  int CountryId => Country.Id;
    public Guid CountryGuidId => Country.IdGuid;


    [Required]
    [DisplayName("Mobile Phone")]
    public required string MobilePhone { get; set; }


    [Required]
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }


    [Required] public required bool Active { get; set; } = true;

    [Required] public required Gender Gender { get; set; }


    [Required]
    [DisplayName("Date Of Birth")]
    [DataType(DataType.Date)]
    public required DateTime DateOfBirth { get; set; }


    [Required]
    [DisplayName("Identification Number")]
    public required string IdentificationNumber { get; set; }

    public required string IdentificationType { get; set; }


    [Required]
    [DisplayName("Expiration Date Identification Number")]
    [DataType(DataType.Date)]
    public required DateTime ExpirationDateIdentificationNumber { get; set; }


    [Required]
    [DisplayName("Tax Identification Number")]
    public required string TaxIdentificationNumber { get; set; }


    [Required]
    [DisplayName("Country Of Nationality")]
    public required Country CountryOfNationality { get; set; }

    [Required]
    public Nationality Nationality => CountryOfNationality.Nationality;

    // [Required] public required Nationality Nationality { get; set; }


    [Required] public required Country Birthplace { get; set; }


    [Required]
    [DisplayName("Enroll Date")]
    [DataType(DataType.Date)]
    public required DateTime EnrollDate { get; set; }


    [Required] public required User User { get; set; }


    // ----------------------------------------------------------------------------------- //
    // ----------------------------------------------------------------------------------- //


    [NotMapped] [DisplayName("Image")] public IFormFile? ImageFile { get; set; }


    [DisplayName("Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/students/" +
          ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and SchoolClass
    // ---------------------------------------------------------------------- //


    // [DisplayName("Courses")]
    // public ICollection<SchoolClass>? SchoolClasses { get; set; } =
    //     new List<SchoolClass>();

    [DisplayName("Courses")]
    public ICollection<StudentCourse>? StudentCourses { get; set; } =
        new List<StudentCourse>();


    // [DisplayName("Courses Count")]
    // public int CoursesCount =>
    //     SchoolClasses?.Where(s => s.CoursesCount > 0).Count() ?? 0;
    //
    //
    // [DisplayName("Total Work Hours")]
    // public int TotalWorkHours =>
    //     SchoolClasses?.Sum(t => t.WorkHourLoad ?? 0) ?? 0;

    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and SchoolClass
    // ---------------------------------------------------------------------- //

    [DisplayName("Courses")]
    public ICollection<SchoolClassStudent>? SchoolClassStudents { get; set; } =
        new List<SchoolClassStudent>();

    [DisplayName("School Classes Count")]
    public int SchoolClassesCount =>
        SchoolClassStudents?.Count ?? 0;

    [DisplayName("Courses Count")]
    public int SCSCoursesCount => SchoolClassStudents?
        .Where(scs => scs.StudentId == Id)
        // Assuming SchoolClass navigation property in SchoolClassStudent
        .Select(scs => scs.SchoolClass)
        .Where(sc => sc != null)
        // Assuming Courses navigation property in SchoolClass
        .SelectMany(sc => sc.Courses)
        .Where(c => c != null)
        .Count() ?? 0;


    [DisplayName("Total Work Hours")]
    public int SCSTotalWorkHours => SchoolClassStudents?
        .Where(scs => scs.StudentId == Id)
        // Assuming SchoolClass navigation property in SchoolClassStudent
        .Select(scs => scs.SchoolClass)
        .Where(sc => sc != null)
        // Assuming Courses navigation property in SchoolClass
        .SelectMany(sc => sc.Courses)
        .Where(c => c != null)
        .Sum(c => c.Hours) ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and Courses
    // ---------------------------------------------------------------------- //

    public ICollection<Enrollment>? Enrollments { get; set; }


    [DisplayName("Courses Count")]
    public int? CoursesCountEnrollments =>
        Enrollments?.Where(e => e.Course.Id == Id).Count() ?? 0;


    [DisplayName("Total Work Hours")]
    public int? TotalWorkHoursEnrollments =>
        Enrollments?.Sum(e => e.Course.Hours) ?? 0;


    [DisplayName("Highest Grade")]
    public decimal? HighestGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Max(e => e.Grade) ?? 0;


    [DisplayName("Average Grade")]
    public decimal? AveregaGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Average(e => e.Grade) ?? 0;


    [DisplayName("Lowest Grade")]
    public decimal? LowestGrade => Enrollments?
        .Where(e => e.StudentId == Id)
        .Min(e => e.Grade) ?? 0;


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

    [Required]
    [DisplayName("Created By")]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Updated By")] public virtual User? UpdatedBy { get; set; }
}
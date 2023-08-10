using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Students;

public class Student : IEntity //: INotifyPropertyChanged
{
    [Required]
    [DisplayName(displayName: "First Name")]
    public required string FirstName { get; set; }


    [Required]
    [DisplayName(displayName: "Last Name")]
    public required string LastName { get; set; }


    [DisplayName(displayName: "Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    [Required] public required string Address { get; set; }


    [Required]
    [DisplayName(displayName: "Postal Code")]
    public required string PostalCode { get; set; }


    [Required] public required City City { get; set; }


    [Required]
    // [ForeignKey("CountryId")]
    public required Country Country { get; set; }

    // public  int CountryId => Country.Id;
    public Guid CountryGuidId => Country.IdGuid;


    [Required]
    [DisplayName(displayName: "Mobile Phone")]
    public required string MobilePhone { get; set; }


    [Required]
    [DataType(dataType: DataType.EmailAddress)]
    public required string Email { get; set; }


    [Required] public required bool Active { get; set; } = true;

    [Required] public required Gender Gender { get; set; }


    [Required]
    [DisplayName(displayName: "Date Of Birth")]
    [DataType(dataType: DataType.Date)]
    public required DateTime DateOfBirth { get; set; }


    [Required]
    [DisplayName(displayName: "Identification Number")]
    public required string IdentificationNumber { get; set; }

    public required string IdentificationType { get; set; }


    [Required]
    [DisplayName(displayName: "Expiration Date Identification Number")]
    [DataType(dataType: DataType.Date)]
    public required DateTime ExpirationDateIdentificationNumber { get; set; }


    [Required]
    [DisplayName(displayName: "Tax Identification Number")]
    public required string TaxIdentificationNumber { get; set; }


    [Required]
    [DisplayName(displayName: "Country Of Nationality")]
    public required Country CountryOfNationality { get; set; }

    [Required]
    public Nationality Nationality => CountryOfNationality.Nationality;

    // [Required] public required Nationality Nationality { get; set; }


    [Required] public required Country Birthplace { get; set; }


    [Required]
    [DisplayName(displayName: "Enroll Date")]
    [DataType(dataType: DataType.Date)]
    public required DateTime EnrollDate { get; set; }


    [Required] public required User User { get; set; }


    [DisplayName(displayName: "Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/students/" +
          ProfilePhotoId;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and SchoolClass
    // ---------------------------------------------------------------------- //


    [DisplayName(displayName: "Courses")]
    public ICollection<SchoolClass>? SchoolClasses { get; set; } =
        new List<SchoolClass>();

    [DisplayName(displayName: "Courses")]
    public ICollection<StudentCourse>? StudentCourses { get; set; } =
        new List<StudentCourse>();


    [DisplayName(displayName: "Courses Count")]
    public int CoursesCount =>
        SchoolClasses?.Where(predicate: s => s.CoursesCount > 0).Count() ?? 0;


    [DisplayName(displayName: "Total Work Hours")]
    public int TotalWorkHours =>
        SchoolClasses?.Sum(selector: t => t.WorkHourLoad ?? 0) ?? 0;


    // ---------------------------------------------------------------------- //
    // Navigation property for the many-to-many relationship
    // between Student and Courses
    // ---------------------------------------------------------------------- //

    public ICollection<Enrollment>? Enrollments { get; set; }


    [DisplayName(displayName: "Courses Count")]
    public int? CoursesCountEnrollments =>
        Enrollments?.Where(predicate: e => e.Course.Id == Id).Count() ?? 0;


    [DisplayName(displayName: "Total Work Hours")]
    public int? TotalWorkHoursEnrollments =>
        Enrollments?.Sum(selector: e => e.Course.Hours) ?? 0;


    [DisplayName(displayName: "Highest Grade")]
    public decimal? HighestGrade => Enrollments?
        .Where(predicate: e => e.StudentId == Id)
        .Max(selector: e => e.Grade) ?? 0;


    [DisplayName(displayName: "Average Grade")]
    public decimal? AveregaGrade => Enrollments?
        .Where(predicate: e => e.StudentId == Id)
        .Average(selector: e => e.Grade) ?? 0;


    [DisplayName(displayName: "Lowest Grade")]
    public decimal? LowestGrade => Enrollments?
        .Where(predicate: e => e.StudentId == Id)
        .Min(selector: e => e.Grade) ?? 0;


    [Key]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName(displayName: "Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Created At")]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [DisplayName(displayName: "Created By")]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName(displayName: "Updated By")] public virtual User? UpdatedBy { get; set; }
}
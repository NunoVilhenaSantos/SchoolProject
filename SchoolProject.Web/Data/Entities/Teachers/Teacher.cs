using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities.Teachers;

public class Teacher : IEntity //: INotifyPropertyChanged
{
    private string _birthplace;
    private string _genre;
    private string _nationality;


    [Required] [DisplayName("First Name")] public string FirstName { get; set; }


    [Required] [DisplayName("Last Name")] public string LastName { get; set; }


    [DisplayName("Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    [Required] public string Address { get; set; }


    [Required]
    [DisplayName("Postal Code")]
    public string PostalCode { get; set; }

    [Required] public string City { get; set; }

    [Required]
    [DisplayName("Mobile Phone")]
    public string MobilePhone { get; set; }


    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required] public bool Active { get; set; } = true;

    [Required]
    public string Genre
    {
        get => _genre;
        set
        {
            if (Enum.TryParse(value, out Genres genre)) _genre = value;
        }
    }

    [Required]
    [DisplayName("Date Of Birth")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [DisplayName("Identification Number")]
    public string IdentificationNumber { get; set; }


    [Required]
    [DisplayName("Expiration Date Identification Number")]
    [DataType(DataType.Date)]
    public DateTime ExpirationDateIdentificationNumber { get; set; }


    [Required]
    [DisplayName("Tax Identification Number")]
    public string TaxIdentificationNumber { get; set; }


    [Required]
    public string Nationality
    {
        get => _nationality;
        set
        {
            if (Enum.TryParse(value, out Countries countries))
                _nationality = value;
        }
    }

    [Required]
    public string Birthplace
    {
        get => _birthplace;
        set
        {
            if (Enum.TryParse(value, out Countries countries))
                _birthplace = value;
        }
    }


    [DisplayName("Courses Count")] public int CoursesCount { get; set; }

    [DisplayName("Total Work Hours")] public int TotalWorkHours { get; set; }

    [Required]
    [DisplayName("Enroll Date")]
    [DataType(DataType.Date)]
    public DateTime EnrollDate { get; set; }


    [Required] public User User { get; set; }


    [DisplayName("Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/supershoptpsicet77-nuno/teachers/" +
          ProfilePhotoId;


    [Required] [Key] public int Id { get; set; }

    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
}
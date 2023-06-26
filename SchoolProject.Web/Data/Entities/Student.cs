using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public class Student : IEntity //: INotifyPropertyChanged
{
    private string _genre;


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

    [Required] public bool Active { get; set; }

    [Required]
    public string Genre
    {
        get => _genre;
        set
        {
            if (Enum.TryParse(value, out Genres genre)) _genre = value;
        }
    }


    public DateOnly DateOfBirth { get; set; }

    public string IdentificationNumber { get; set; }


    public DateOnly ExpirationDateIn { get; set; }


    public string TaxIdentificationNumber { get; set; }


    public string Nationality { get; set; }

    public string Birthplace { get; set; }


    public string Photo { get; set; }


    public int CoursesCount { get; set; }


    public int TotalWorkHours { get; set; }


    public DateOnly EnrollDate { get; set; }


    [Required] public User User { get; set; }


    [DisplayName("Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/supershoptpsicet77-nuno/courses/" +
          ProfilePhotoId;


    [Required] [Key] public int Id { get; set; }

    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
}
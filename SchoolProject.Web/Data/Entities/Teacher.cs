using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public class Teacher : IEntity //: INotifyPropertyChanged
{
    private string _genre;
    public string FirstName { get; set; }

    public string LastName { get; set; }


    public string Address { get; set; }

    public string PostalCode { get; set; }

    public string City { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public bool Active { get; set; }

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


    public DateOnly ExpirationDateIdentificationNumber { get; set; }


    public string TaxIdentificationNumber { get; set; }


    public string Nationality { get; set; }


    public string Birthplace { get; set; }


    public int CoursesCount { get; set; }

    public int TotalWorkHours { get; set; }

    [Required] public User User { get; set; }

    public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://myleasingnunostorage.blob.core.windows.net/lessees/" +
          GetType().BaseType?.Name +
          ProfilePhotoId;

    public int Id { get; set; }
    public bool WasDeleted { get; set; }
}
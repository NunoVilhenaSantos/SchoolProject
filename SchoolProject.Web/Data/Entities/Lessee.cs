using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SchoolProject.Web.Data.Entities;

public class Lessee : IEntity
{
    [DisplayName("Document*")]
    [MaxLength(20,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public string Document { get; set; }


    [DisplayName("First Name*")]
    [MaxLength(50,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public string FirstName { get; set; }


    [DisplayName("Last Name*")]
    [MaxLength(50,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public string LastName { get; set; }


    [DisplayName("Profile Photo")] public string? ProfilePhotoUrl { get; set; }

    public string? ProfilePhotoFullUrl =>
        string.IsNullOrEmpty(ProfilePhotoUrl)
            ? "https://supershopnunostorage.blob.core.windows.net/" +
              "placeholders/no-picture/person/Placeholder-no-text-person-3.png"
            : Regex.Replace(ProfilePhotoUrl, @"^~/lessees/images/",
                "https://myleasingnunostorage.blob.core.windows.net/lessees/");


    public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://myleasingnunostorage.blob.core.windows.net/lessees/" +
          ProfilePhotoId;

    [DisplayName("Fixed Phone")] public string? FixedPhone { get; set; }


    [DisplayName("Cell Phone")] public string? CellPhone { get; set; }


    [MaxLength(100,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    public string? Address { get; set; }


    [DisplayName("Owner Name")]
    public string FullName => $"{FirstName} {LastName}";


    [Display(Name = "Full Name with Document")]
    public string FullNameWithDocument =>
        $"{FirstName} {LastName} - {Document}";


    [Required] public User User { get; set; }


    [Key] public int Id { get; set; }
    public bool WasDeleted { get; set; }
}
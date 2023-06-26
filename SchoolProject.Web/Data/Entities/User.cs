using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolProject.Web.Data.Entities;

public class User : IdentityUser //: INotifyPropertyChanged
{
    [DisplayName("First Name")]
    [MaxLength(50,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public string FirstName { get; set; }


    [DisplayName("Last Name")]
    [MaxLength(50,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public string LastName { get; set; }


    [MaxLength(100,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    public string? Address { get; set; }


    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://myleasingnunostorage.blob.core.windows.net/lessees/" +
          ProfilePhotoId;


    // [Display(Name = "Thumbnail")]
    // public string ImageThumbnailUrl { get; set; }
    //
    // public string ImageThumbnailFullUrl =>
    //     string.IsNullOrEmpty(ImageThumbnailUrl)
    //         ? null
    //         : $"https://supermarketapi.azurewebsites.net{ImageThumbnailUrl[1..]}";


    // [Required] [Key] public int Id { get; set; }

    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
}
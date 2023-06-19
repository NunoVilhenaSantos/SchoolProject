using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolProject.Web.Data.Entities;

public class User : IdentityUser
{
    [DisplayName("Document")]
    [MaxLength(20,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public string Document { get; set; }


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


    [Display(Name = "Full Name with Document")]
    public string FullNameWithDocument =>
        $"{FirstName} {LastName} - {Document}";


    [DisplayName("Profile Photo")] public string? ProfilePhotoUrl { get; set; }


    public string? ProfilePhotoFullUrl =>
        string.IsNullOrEmpty(ProfilePhotoUrl)
            ? null
            : $"https://supermarketapi.azurewebsites.net{ProfilePhotoUrl[1..]}";


    // [Display(Name = "Thumbnail")]
    // public string ImageThumbnailUrl { get; set; }
    //
    // public string ImageThumbnailFullUrl =>
    //     string.IsNullOrEmpty(ImageThumbnailUrl)
    //         ? null
    //         : $"https://supermarketapi.azurewebsites.net{ImageThumbnailUrl[1..]}";
}
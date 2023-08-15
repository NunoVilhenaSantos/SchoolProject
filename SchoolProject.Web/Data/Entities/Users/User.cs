using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace SchoolProject.Web.Data.Entities.Users;

public class User : IdentityUser, INotifyPropertyChanged
{
    [DisplayName("First Name")]
    [MaxLength(50,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    // [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string FirstName { get; set; }


    [DisplayName("Last Name")]
    [MaxLength(50,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    // [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string LastName { get; set; }


    [MaxLength(100,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    public string? Address { get; set; }


    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    [DisplayName("Was Deleted?")] public required bool WasDeleted { get; set; }





    // ----------------------------------------------------------------------------------- //
    // ----------------------------------------------------------------------------------- //


    [NotMapped]
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }




    public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/users/" +
          ProfilePhotoId;
    //    "https://storage.googleapis.com/storage-nuno/products/"+
    //    "130cd374-c068-47ca-b542-3af5ddb9f478";


    // [Display(Name = "Thumbnail")]
    // public string ImageThumbnailUrl { get; set; }
    //
    // public string ImageThumbnailFullUrl =>
    //     string.IsNullOrEmpty(ImageThumbnailUrl)
    //         ? null
    //         : $"https://supermarketapi.azurewebsites.net{ImageThumbnailUrl[1..]}";






    // ----------------------------------------------------------------------------------- //
    // ----------------------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //

    public event PropertyChangedEventHandler? PropertyChanged;


    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }


    private bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
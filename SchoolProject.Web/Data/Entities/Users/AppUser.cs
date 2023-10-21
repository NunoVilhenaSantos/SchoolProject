using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Controllers;
using SchoolProject.Web.Helpers.Storages;

namespace SchoolProject.Web.Data.Entities.Users;

/// <summary>
///     appUser class, inherited from IdentityUser.
/// </summary>
public class AppUser : IdentityUser, INotifyPropertyChanged
{
    /// <summary>
    ///     The first name of the appUser.
    /// </summary>
    [DisplayName("First Name")]
    [MaxLength(50,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string FirstName { get; set; }


    /// <summary>
    ///     The last name of the appUser.
    /// </summary>
    [DisplayName("Last Name")]
    [MaxLength(50,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string LastName { get; set; }


    /// <summary>
    ///     The address of the appUser.
    /// </summary>
    [MaxLength(100,
        ErrorMessage = "The {0} field can not have more than {1} characters.")]
    public string? Address { get; set; }


    // /// <summary>
    // ///     The phone number of the appUser.
    // /// </summary>
    //[MaxLength(20,
    //    ErrorMessage =
    //        "The field {0} can only contain {1} characters in lenght.")]
    //public string? PhoneNumber { get; set; }


    /// <summary>
    /// </summary>
    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";


    /// <summary>
    ///     Was Deleted?
    /// </summary>
    [DisplayName("Was Deleted?")]
    public required bool WasDeleted { get; set; }


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     The image of the appUser file from the form to be inserted in the database.
    /// </summary>
    [CsvHelper.Configuration.Attributes.Ignore]
    [NotMapped]
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }


    /// <summary>
    ///     The profile photo of the appUser.
    /// </summary>
    [DisplayName("Profile Photo")]
    public required Guid ProfilePhotoId { get; set; }


    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? StorageHelper.NoImageUrl
        : StorageHelper.AzureStoragePublicUrl +
          UsersController.BucketName +
          "/" + ProfilePhotoId;


    // [Display(Name = "Thumbnail")]
    // public string ImageThumbnailUrl { get; set; }
    //
    // public string ImageThumbnailFullUrl =>
    //     string.IsNullOrEmpty(ImageThumbnailUrl)
    //         ? null
    //         : $"https://supermarketapi.azurewebsites.net{ImageThumbnailUrl[1..]}";


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;


    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }


    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    private bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
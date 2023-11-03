using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using CsvHelper.Configuration.Attributes;
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
    /// </summary>
    [Display(Name = "Full Name")]
    public string FullName => $"{FirstName} {LastName}";


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


    // /// <summary>
    // ///    The country of the appUser.
    // /// </summary>
    // [NotMapped]
    // [DisplayName("Country")]
    // [Range(1, int.MaxValue, ErrorMessage = "You must select a country...")]
    // public required int CountryId { get; set; }


    // /// <summary>
    // ///     The city of the appUser.
    // /// </summary>
    // [Required]
    // [Display(Name = "City")]
    // [ForeignKey(nameof(City))]
    // [Range(1, int.MaxValue, ErrorMessage = "You must select a city...")]
    // public  int CityId { get; set; }


    // /// <summary>
    // ///
    // /// </summary>
    // [Required]
    // [Display(Name = "City")]
    // public virtual required City City { get; set; }


    /// <summary>
    ///     The name abbreviation of the appUser.
    /// </summary>
    public string NameAbbreviation
    {
        get
        {
            var firstLetters = FullName.Split(' ')
                .Where(word => !string.IsNullOrEmpty(word))
                .ToArray();

            var nameAbbr = string.Concat(firstLetters[0][0],
                firstLetters[^1][0]);

            return nameAbbr;
        }
    }

    // -------------------------------------------------------------- //


    /// <summary>
    ///     Was Deleted?
    /// </summary>
    [DisplayName("Was Deleted?")]
    public required bool WasDeleted { get; set; }


    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //


    /// <summary>
    ///     The image of the appUser file from the form to be inserted in the database.
    /// </summary>
    [Ignore]
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    [NotMapped]
    [DisplayName("Image")]
    public IFormFile? ImageFile { get; set; }


    /// <summary>
    ///     The profile photo of the appUser.
    /// </summary>
    [DisplayName("Profile Photo")]
    public required Guid ProfilePhotoId { get; set; } = Guid.Empty;


    /// <summary>
    ///     The profile photo of the appUser in URL format.
    /// </summary>
    [DisplayName("Profile Photo")]
    public string ProfilePhotoIdUrl =>
        ProfilePhotoId == Guid.Empty || ProfilePhotoId == null
            ? StorageHelper.NoImageUrl
            : StorageHelper.AzureStoragePublicUrl +
              UsersController.BucketName +
              "/" + ProfilePhotoId;


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
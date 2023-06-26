using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SchoolProject.Web.Data.Entities;

public class Owner : IEntity, IPerson
{
    [Key] public int Id { get; set; }
    public bool WasDeleted { get; set; }

    [Required] [DisplayName("Document*")] public string Document { get; set; }


    [Required]
    [DisplayName("First Name*")]
    public string FirstName { get; set; }


    [Required] [DisplayName("Last Name*")] public string LastName { get; set; }


    [DisplayName("Profile Photo")] public string? ProfilePhotoUrl { get; set; }

    public string? ProfilePhotoFullUrl =>
        string.IsNullOrEmpty(ProfilePhotoUrl)
            ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
            : Regex.Replace(ProfilePhotoUrl, @"^~/owners/images/",
                "https://myleasingnunostorage.blob.core.windows.net/owners/");
    // : "https://myleasingnunostorage.blob.core.windows.net/owners/" + ProfilePhotoUrl.Replace("~/owners/images/", "");

    public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://myleasingnunostorage.blob.core.windows.net/owners/" +
          ProfilePhotoId;
    //:   "https://supershopnunostorage.blob.core.windows.net/{GetType().Name.ToLower()}s/{ImageId}";
    //    "https://supershopnunostorage.blob.core.windows.net/products/e1572b5b-3a31-4c9a-a68b-f13bc4f550d4";


    // [Display(Name = "Thumbnail")]
    // public string ImageThumbnailUrl { get; set; }
    //
    // public string ImageThumbnailFullUrl =>
    //     string.IsNullOrEmpty(ImageThumbnailUrl)
    //         ? null
    //         : $"https://supermarketapi.azurewebsites.net{ImageThumbnailUrl[1..]}";


    [DisplayName("Fixed Phone")] public string? FixedPhone { get; set; }


    [DisplayName("Cell Phone")] public string? CellPhone { get; set; }


    public string? Address { get; set; }


    [DisplayName("Owner Name")]
    public string FullName => $"{FirstName} {LastName}";


    [Required] public User? User { get; set; }
}
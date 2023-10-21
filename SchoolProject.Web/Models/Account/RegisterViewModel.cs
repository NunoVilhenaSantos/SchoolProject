using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

public class RegisterViewModel
{
    [Required] [DisplayName("First Name")]
    public required string FirstName { get; set; }


    [Required] [DisplayName("Last Name")]
    public required string LastName { get; set; }


    [EmailAddress]
    [Required]
    public required string Email { get; set; }

    [MaxLength(8, ErrorMessage = "The field {0} only can contain {1} characters length.")]
    [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*\W).*$", 
        ErrorMessage = "A password must contain at least one number, one uppercase letter, " +
        "one lowercase letter, and one special character.")]
    [Required]
    public required string Password { get; set; }


    [Compare("Password")]
    [Display(Name = "Confirm password")]
    [Required]
    public required string ConfirmPassword { get; set; }
}

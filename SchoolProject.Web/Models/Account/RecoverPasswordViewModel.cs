using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

public class RecoverPasswordViewModel
{
    [Required] [EmailAddress] public required string Email { get; set; }
}

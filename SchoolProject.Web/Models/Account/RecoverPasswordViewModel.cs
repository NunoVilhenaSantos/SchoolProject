using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

public class RecoverPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
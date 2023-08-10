using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Users;

public class ChangePasswordViewModel
{
    [Required]
    [DisplayName(displayName: "Current Password")]
    [DataType(dataType: DataType.Password)]
    public string OldPassword { get; set; }


    [Required]
    [DataType(dataType: DataType.Password)]
    [MinLength(length: 6)]
    public string NewPassword { get; set; }


    [Required]
    [DataType(dataType: DataType.Password)]
    [Compare(otherProperty: "NewPassword")]
    [MinLength(length: 6)]
    public string ConfirmPassword { get; set; }
}
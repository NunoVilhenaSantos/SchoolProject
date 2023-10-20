﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models.Account;

public class ChangePasswordViewModel
{
    [Required]
    [DisplayName("Current Password")]
    [DataType(DataType.Password)]
    public string OldPassword { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [DisplayName("New Password")]
    [MinLength(6)]
    public string NewPassword { get; set; }


    [Required]
    [DataType(DataType.Password)]
    [Compare("NewPassword")]
    [DisplayName("Confirm Password")]
    [MinLength(6)]
    public string ConfirmPassword { get; set; }
}
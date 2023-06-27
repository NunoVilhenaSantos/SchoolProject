﻿using System.ComponentModel;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Models;

public class StudentViewModel : Student
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}
﻿using System.ComponentModel;

namespace SchoolProject.Web.Models.Student;

public class StudentViewModel : Data.Entities.Students.Student
{
    [DisplayName(displayName: "Image")] public IFormFile? ImageFile { get; set; }
}
﻿using System.ComponentModel;

namespace SchoolProject.Web.Models.Teacher;

public class TeacherViewModel : Data.Entities.Teachers.Teacher
{
    [DisplayName(displayName: "Image")] public IFormFile? ImageFile { get; set; }
}
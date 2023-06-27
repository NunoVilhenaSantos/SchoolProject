﻿using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClassCourse : IEntity
{
    [Required] public int SchoolClassId { get; set; }

    [Required] public int CourseId { get; set; }

    [Required] [Key] public int Id { get; set; }
    [Required] public bool WasDeleted { get; set; }
}
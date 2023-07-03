﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Web.Data.Entities.Courses;

public class Course : IEntity // : INotifyPropertyChanged
{
    [Required] public string Name { get; set; }


    [Required] public int WorkLoad { get; set; }


    [Required] public int Credits { get; set; }


    [DisplayName("Students Count")] public int? StudentsCount { get; set; }


    [DisplayName("Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/supershoptpsicet77-nuno/courses/" +
          ProfilePhotoId;


    [Required]  public int Id { get; set; }
    [Required] [Key] [Column("StudentId")] public Guid IdGuid { get; set; }

    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public User CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User UpdatedBy { get; set; }
}
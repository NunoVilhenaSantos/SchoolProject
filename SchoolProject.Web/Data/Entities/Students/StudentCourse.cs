﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Students;

public class StudentCourse : IEntity
{
    [Required] public required int StudentId { get; set; }

    [ForeignKey(nameof(StudentId))]
    public virtual required Student Student { get; set; }

    public Guid StudentGuidId => Student.IdGuid;


    [Required] public required int CourseId { get; set; }

    [ForeignKey(nameof(CourseId))]
    [Required]
    public required Course Course { get; set; }

    public Guid CourseGuidId => Course.IdGuid;


    // [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Created By")] public required User CreatedBy { get; set; }


    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Updated By")] public User? UpdatedBy { get; set; }
}
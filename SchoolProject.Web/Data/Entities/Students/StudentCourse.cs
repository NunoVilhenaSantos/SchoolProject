﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Students;

public class StudentCourse : IEntity, INotifyPropertyChanged
{
    [Required] public required int StudentId { get; set; }

    [Required]
    [ForeignKey(nameof(StudentId))]
    public virtual required Student Student { get; set; }

    public Guid StudentGuidId => Student.IdGuid;


    [Required] public required int CourseId { get; set; }

    [Required]
    [ForeignKey(nameof(CourseId))]
    public required Course Course { get; set; }

    public Guid CourseGuidId => Course.IdGuid;


    // Deve ser do mesmo tipo da propriedade Id de User
    [DisplayName("Created By User Id")] public string CreatedById { get; set; }

    // Deve ser do mesmo tipo da propriedade Id de User
    [DisplayName("Updated By User Id")] public string? UpdatedById { get; set; }


    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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


    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName("Created By")]
    [ForeignKey(nameof(CreatedById))]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName("Updated By")]
    [ForeignKey(nameof(UpdatedById))]
    public virtual User? UpdatedBy { get; set; }


    // ---------------------------------------------------------------------- //
    // Property Changed Event Handler
    // ---------------------------------------------------------------------- //


    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;


    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }

    /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged" />
    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
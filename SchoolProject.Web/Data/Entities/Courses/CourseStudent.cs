﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Courses;

/// <summary>
/// </summary>
public class CourseStudent : IEntity, INotifyPropertyChanged
{
    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     Foreign Key for Course
    /// </summary>
    [Required]
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }


    /// <summary>
    ///     The real Object for Course
    /// </summary>
    [Required]
    public virtual required Course Course { get; set; }


    /// <summary>
    ///     Foreign Guid Key for Course
    /// </summary>
    public Guid CourseGuidId => Course?.IdGuid ?? Guid.Empty;


    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <summary>
    ///     Foreign Key for Student
    /// </summary>
    [Required]
    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }

    /// <summary>
    ///     The real Object for Student
    /// </summary>
    [Required]
    public virtual required Student Student { get; set; }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    /// <summary>
    ///     Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Created By AppUser Id")]
    [ForeignKey(nameof(CreatedBy))]
    public string CreatedById { get; set; }


    /// <summary>
    ///     Deve ser do mesmo tipo da propriedade Id de AppUser
    /// </summary>
    [DisplayName("Updated By AppUser Id")]
    [ForeignKey(nameof(UpdatedBy))]
    public string? UpdatedById { get; set; }



    // --------------------------------------------------------------------- //
    // --------------------------------------------------------------------- //


    /// <inheritdoc />
    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    /// <inheritdoc />
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    /// <inheritdoc />
    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    /// <inheritdoc />
    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    /// <inheritdoc />
    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName("Created By")]
    public virtual required AppUser CreatedBy { get; set; }


    /// <inheritdoc />
    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


    /// <inheritdoc />
    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName("Updated By")]
    public virtual AppUser? UpdatedBy { get; set; }


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //


    ///// <summary>
    ///// </summary>
    //[NotMapped]
    //public virtual HashSet<Enrollment>? Enrollments { get; set; }

    ///// <summary>
    /////     Returns the disciplines associated with this teacher
    ///// </summary>
    //[NotMapped]
    //public IEnumerable<Student>? Students =>
    //    Enrollments?.Select(sc => sc.Student).Distinct();


    ///// <summary>
    ///// </summary>
    //[DisplayName("Students Count")]
    //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    //public int StudentsCount => Students?.Count() ?? 0;


    ///// <summary>
    /////     Returns the disciplines associated with this teacher
    ///// </summary>
    //[NotMapped]
    //public IEnumerable<Discipline>? Disciplines =>
    //    Enrollments?.Select(sc => sc.Discipline).Distinct();


    ///// <summary>
    ///// </summary>
    //[DisplayName("Disciplines Count")]
    //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    //public int DisciplinesCount => Disciplines?.Count() ?? 0;

    ///// <summary>
    ///// </summary>
    //[DisplayName("Total Students")]
    //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    //public int TotalStudents => Enrollments?.Where(i => i.Id == Id).Count() ?? 0;


    ///// <summary>
    ///// </summary>
    //[DisplayName("Total Disciplines")]
    //[DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = false)]
    //public int TotalDisciplines => Enrollments?.Where(i => i.Id == Id).Count(e => e.DisciplineId) ?? 0;


    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //

    // ---------------------------------------------------------------------- //
    // ---------------------------------------------------------------------- //




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
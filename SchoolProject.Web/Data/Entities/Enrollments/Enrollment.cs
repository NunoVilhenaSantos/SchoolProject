using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Enrollments;

public class Enrollment : IEntity, INotifyPropertyChanged
{
    [Required] public required int StudentId { get; set; }

    [Required]
    [ForeignKey(name: "StudentId")]
    public virtual required Student Student { get; set; }

    public Guid StudentGuidId => Student.IdGuid;


    [Required] public required int CourseId { get; set; }

    [Required]
    [ForeignKey(name: "CourseId")]
    public virtual required Course Course { get; set; }

    public Guid CourseGuidId => Course.IdGuid;


    // [Column(TypeName = "decimal(18,2)")]
    [Precision(precision: 18, scale: 2)] public decimal? Grade { get; set; }


    // Deve ser do mesmo tipo da propriedade Id de User
    [DisplayName(displayName: "Created By User Id")] public string CreatedById { get; set; }

    // Deve ser do mesmo tipo da propriedade Id de User
    [DisplayName(displayName: "Updated By User Id")] public string? UpdatedById { get; set; }


    // [Key]
    // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName(displayName: "Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Created At")]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName(displayName: "Created By")]
    [ForeignKey(name: nameof(CreatedById))]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    // Propriedade de navegação
    // Especifique o nome da coluna da chave estrangeira
    [DisplayName(displayName: "Updated By")]
    [ForeignKey(name: nameof(UpdatedById))]
    public virtual User? UpdatedBy { get; set; }


    // ---------------------------------------------------------------------- //


    public event PropertyChangedEventHandler? PropertyChanged;


    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(sender: this,
            e: new PropertyChangedEventArgs(propertyName: propertyName));
    }


    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(x: field, y: value)) return false;
        field = value;
        OnPropertyChanged(propertyName: propertyName);
        return true;
    }
}
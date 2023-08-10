using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Teachers;

public class TeacherCourse : IEntity
{
    [Required] public required int TeacherId { get; set; }

    [Required]
    [ForeignKey(name: nameof(TeacherId))]
    public virtual required Teacher Teacher { get; set; }

    public Guid TeacherGuidId => Teacher.IdGuid;


    [Required] public required int CourseId { get; set; }

    [Required]
    [ForeignKey(name: nameof(CourseId))]
    public virtual required Course Course { get; set; }


    public Guid CourseGuidId => Course.IdGuid;


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
}
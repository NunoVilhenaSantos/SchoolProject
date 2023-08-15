using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.EntitiesOthers;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public class SchoolClassStudent : IEntity
{
    /// <summary>
    ///     Foreign Key for SchoolClass
    /// </summary>
    [Required]
    public required int SchoolClassId { get; set; }


    /// <summary>
    ///     The real Object for SchoolClass
    /// </summary>
    [Required]
    [ForeignKey(nameof(SchoolClassId))]
    public virtual required SchoolClass SchoolClass { get; set; }


    /// <summary>
    ///     Foreign Guid Key for SchoolClass
    /// </summary>
    public Guid SchoolClassGuidId => SchoolClass.IdGuid;


    /// <summary>
    ///     Foreign Key for Course
    /// </summary>
    [Required]
    public required int StudentId { get; set; }

    /// <summary>
    ///     The real Object for Course
    /// </summary>
    [Required]
    [ForeignKey(nameof(StudentId))]
    public virtual required Student Student { get; set; }


    public Guid StudentGuidId => Student.IdGuid;


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
}
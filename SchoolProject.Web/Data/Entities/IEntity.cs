using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Data.Entities;

public interface IEntity
{
    [Column(Order = 0)]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(Order = 1)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Column(Order = 20)]
    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Column(Order = 21)]
    [Required]
    [DisplayName("Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }


    [Column(Order = 22)]
    [Required]
    [DisplayName("Created By")]
    public User CreatedBy { get; set; }


    [Column(Order = 23)]
    [Required]
    [DisplayName("Updated At")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedAt { get; set; }


    [Column(Order = 24)]
    [Required]
    [DisplayName("Updated By")]
    public User? UpdatedBy { get; set; }
}
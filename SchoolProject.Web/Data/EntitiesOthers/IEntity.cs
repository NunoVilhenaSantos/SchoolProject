using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Web.Data.EntitiesOthers;

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


    [Required]
    [Column(Order = 21)]
    [DisplayName("Created At")]
    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }


    [Required]
    [Column(Order = 22)]
    [DisplayName("Created By")]
    public User CreatedBy { get; set; }


    // [Required]
    [Column(Order = 23)]
    [DisplayName("Updated At")]
    [DataType(DataType.Date)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }


    [Column(Order = 24)]
    [Required]
    [DisplayName("Updated By")]
    [DataType(DataType.Date)]
    public User? UpdatedBy { get; set; }
}
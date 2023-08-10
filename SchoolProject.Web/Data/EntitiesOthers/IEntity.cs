using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Web.Data.EntitiesOthers;

public interface IEntity
{
    [Column(Order = 0)]
    [Key]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Column(Order = 1)]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Column(Order = 20)]
    [Required]
    [DisplayName(displayName: "Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [Column(Order = 21)]
    [DisplayName(displayName: "Created At")]
    [DataType(dataType: DataType.Date)]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }


    [Required]
    [Column(Order = 22)]
    [DisplayName(displayName: "Created By")]
    public User CreatedBy { get; set; }


    // [Required]
    [Column(Order = 23)]
    [DisplayName(displayName: "Updated At")]
    [DataType(dataType: DataType.Date)]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; }


    [Column(Order = 24)]
    [Required]
    [DisplayName(displayName: "Updated By")]
    [DataType(dataType: DataType.Date)]
    public User? UpdatedBy { get; set; }
}
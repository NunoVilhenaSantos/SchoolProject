using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public interface IEntity
{
    [Required] [Key] public int Id { get; init; }


    [Required] [Key] public Guid IdGuid { get; init; }


    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }


    [Required]
    [DisplayName("Created At")]
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; init; }


    [Required] [DisplayName("Created By")] public User CreatedBy { get; init; }


    [Required]
    [DisplayName("Updated At")]
    [DataType(DataType.Date)]
    public DateTime? UpdatedAt { get; set; }


    [Required] [DisplayName("Updated By")] public User? UpdatedBy { get; set; }
}
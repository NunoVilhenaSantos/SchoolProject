using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public interface IEntity
{
    [Required] [Key] public int Id { get; set; }


    [Required] [Key] public Guid IdGuid { get; set; }


    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }


    [Required]
    [DisplayName("Created At")]
    public DateTime CreatedAt { get; set; }


    [Required] [DisplayName("Created By")] public User CreatedBy { get; set; }


    [Required]
    [DisplayName("Updated At")]
    public DateTime UpdatedAt { get; set; }


    [Required] [DisplayName("Updated By")] public User UpdatedBy { get; set; }
}
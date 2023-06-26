using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public interface IEntity
{
    [Required] [Key] public int Id { get; set; }

    [DisplayName("Was Deleted?")] public bool WasDeleted { get; set; }


    // string Name { get; set; }
}
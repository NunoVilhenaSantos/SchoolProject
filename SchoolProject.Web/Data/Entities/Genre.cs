using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolProject.Web.Data.Entities;

public class Genre : IEntity
{
    [MaxLength(20,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Name { get; set; }


    [Required] public int Id { get; init; }


    [Required] [Key] [Column("GenreId")] public Guid IdGuid { get; init; }


    [Required] [Column("Was Deleted?")] public bool WasDeleted { get; set; }


    [DataType(DataType.Date)] public DateTime CreatedAt { get; init; }

    public required User CreatedBy { get; init; }


    [DataType(DataType.Date)] public DateTime? UpdatedAt { get; set; }

    public User? UpdatedBy { get; set; }
}
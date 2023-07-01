using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Data.Entities;

public class Genre : IEntity
{
    [MaxLength(20,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]

    public string Name { get; set; }

    [Key] public int Id { get; set; }
    public Guid IdGuid { get; set; }
    public bool WasDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public User CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public User UpdatedBy { get; set; }
}
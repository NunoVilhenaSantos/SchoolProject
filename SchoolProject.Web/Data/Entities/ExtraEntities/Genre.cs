using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace SchoolProject.Web.Data.Entities.ExtraEntities;

public class Genre : IEntity, INotifyPropertyChanged
{
    [MaxLength(20,
        ErrorMessage =
            "The {0} field can not have more than {1} characters.")]
    [Required(ErrorMessage = "The field {0} is mandatory.")]
    public required string Name { get; set; }


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // [Column("GenreId")]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public required bool WasDeleted { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    public required DateTime CreatedAt { get; set; }


    [DisplayName("Created By")] public required User CreatedBy { get; set; }


    // [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    public DateTime? UpdatedAt { get; set; }

    [DisplayName("Updated By")] public User? UpdatedBy { get; set; }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this,
            new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
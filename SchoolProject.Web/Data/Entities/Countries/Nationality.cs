using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Data.Entities.Countries;

public class Nationality : IEntity, INotifyPropertyChanged
{
    [Required]
    [DisplayName("Nationality")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // [DisplayName("NationalityId")]
    public Guid IdGuid { get; set; }


    public required bool WasDeleted { get; set; }


    [Required] public required DateTime CreatedAt { get; set; }

    [Required] public required User CreatedBy { get; set; }


    public DateTime? UpdatedAt { get; set; }
    public User? UpdatedBy { get; set; }


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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Data.Entities.Countries;

public class City : IEntity, INotifyPropertyChanged
{
    [Required]
    [DisplayName("City")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    [Key] [Required] public  int Id { get; set; }


    [DisplayName("CityId")] [Required] public required Guid IdGuid { get; set; }


    [Required] public required DateTime CreatedAt { get; set; }
    [Required] public required User CreatedBy { get; set; }


    public DateTime? UpdatedAt { get; set; }
    public User? UpdatedBy { get; set; }


    [Required] public required bool WasDeleted { get; set; }


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
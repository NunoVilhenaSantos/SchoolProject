using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Data.Entities.Countries;

public class Country : IEntity, INotifyPropertyChanged
{
    [Required]
    [DisplayName("Country")]
    [MaxLength(50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    public ICollection<City>? Cities { get; set; }


    [DisplayName("Number of Cities")]
    public int NumberCities => Cities?.Count ?? 0;


    [Required]
    // [ForeignKey("NationalityId")]
    public required Nationality Nationality { get; set; }

    // public int NationalityId => Nationality.Id;
    public Guid NationalityGuidId => Nationality.IdGuid;


    [Key] [Required] public int Id { get; set; }


    [Required]
    [DisplayName("CountryId")]
    public required Guid IdGuid { get; set; }


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
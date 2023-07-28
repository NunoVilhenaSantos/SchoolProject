using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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





    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public required Guid IdGuid { get; set; }


    [Required]
    [DisplayName("Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Created At")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName("Created By")] public required User CreatedBy { get; set; }


    [Required]
    [DataType(DataType.Date)]
    [DisplayName("Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

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
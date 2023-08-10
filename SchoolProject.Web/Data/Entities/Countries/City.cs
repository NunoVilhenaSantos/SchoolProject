using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Entities.Countries;

// [PrimaryKey(nameof(Id), nameof(IdGuid))]
public class City : IEntity, INotifyPropertyChanged
{
    [Required]
    [DisplayName(displayName: "City")]
    [MaxLength(length: 50, ErrorMessage = "The field {0} can contain {1} characters.")]
    public required string Name { get; set; }


    [Key]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public Guid IdGuid { get; set; }


    [Required]
    [DisplayName(displayName: "Was Deleted?")]
    public bool WasDeleted { get; set; }


    [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Created At")]
    [DatabaseGenerated(databaseGeneratedOption: DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [DisplayName(displayName: "Created By")]
    public virtual required User CreatedBy { get; set; }


    // [Required]
    [DataType(dataType: DataType.Date)]
    [DisplayName(displayName: "Update At")]
    // [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;

    [DisplayName(displayName: "Updated By")] public virtual User? UpdatedBy { get; set; }


    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(sender: this,
            e: new PropertyChangedEventArgs(propertyName: propertyName));
    }

    protected bool SetField<T>(ref T field, T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(x: field, y: value)) return false;
        field = value;
        OnPropertyChanged(propertyName: propertyName);
        return true;
    }
}
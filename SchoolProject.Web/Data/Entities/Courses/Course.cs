using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.ExtraEntities;

namespace SchoolProject.Web.Data.Entities.Courses;

public class Course : IEntity, INotifyPropertyChanged
{
    private string _name;

    [Required]
    public required string Name
    {
        get => _name;
        set
        {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }


    [Required] public required int WorkLoad { get; set; }


    [Required] public required int Credits { get; set; }


    [DisplayName("Profile Photo")] public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://storage.googleapis.com/storage-nuno/courses/" +
          ProfilePhotoId;


    public ICollection<Enrollment>? Enrollments { get; set; }


    [DisplayName("Students Count")]
    public int? StudentsCount =>
        Enrollments?.Where(e => e.Course.Id == Id).Count() ?? 0;



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
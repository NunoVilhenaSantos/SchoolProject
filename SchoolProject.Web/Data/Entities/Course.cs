namespace SchoolProject.Web.Data.Entities;

public class Course : IEntity // : INotifyPropertyChanged
{
    public string Name { get; set; }


    public int WorkLoad { get; set; }

    public int Credits { get; set; }


    public int? StudentsCount { get; set; }

    public Guid ProfilePhotoId { get; set; }

    public string ProfilePhotoIdUrl => ProfilePhotoId == Guid.Empty
        ? "https://supershopweb.blob.core.windows.net/noimage/noimage.png"
        : "https://myleasingnunostorage.blob.core.windows.net/lessees/" +
          ProfilePhotoId;

    public int Id { get; set; }
    public bool WasDeleted { get; set; }
}
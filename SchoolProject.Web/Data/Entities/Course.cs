using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SchoolProject.Web.Data.Entities;

public class Course : IEntity // : INotifyPropertyChanged
{


    public string Name { get; set; }


    public int WorkLoad { get; set; }

    public int Credits { get; set; }


    public int? StudentsCount { get; set; }


    public int Id { get; set; }
    public bool WasDeleted { get; set; }
}
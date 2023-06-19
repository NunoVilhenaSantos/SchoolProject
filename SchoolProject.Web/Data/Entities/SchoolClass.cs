using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SchoolProject.Web.Data.Entities;

public class SchoolClass  : IEntity //: INotifyPropertyChanged
{
    [Key] public int IdSchoolClass { get; set; }

    public string ClassAcronym { get; set; }


    public string ClassName { get; set; }


    public DateOnly StartDate { get; set; }


    public DateOnly EndDate { get; set; }

    public TimeOnly StartHour { get; set; }


    public TimeOnly EndHour { get; set; }


    public string Location { get; set; }

    public string Type { get; set; }


    public string Area { get; set; }

    public int? CoursesCount { get; set; }


    public int? WorkHourLoad { get; set; }

    public int? StudentsCount { get; set; }

    public decimal? ClassAverage { get; set; }

    public decimal? HighestGrade { get; set; }

    public decimal? LowestGrade { get; set; }
    public int Id { get; set; }
    public bool WasDeleted { get; set; }
}
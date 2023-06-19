using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SchoolProject.Web.Data.Entities;

public class Teacher : IEntity //: INotifyPropertyChanged
{


    public string FirstName { get; set; }

    public string LastName { get; set; }


    public string Address { get; set; }

    public string PostalCode { get; set; }

    public string City { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public bool Active { get; set; }

    public string Genre { get; set; }


    public DateOnly DateOfBirth { get; set; }


    public string IdentificationNumber { get; set; }


    public DateOnly ExpirationDateIdentificationNumber { get; set; }


    public string TaxIdentificationNumber { get; set; }


    public string Nationality { get; set; }


    public string Birthplace { get; set; }


    public string Photo { get; set; }


    public int CoursesCount { get; set; }

    public int TotalWorkHours { get; set; }

    [Required] public User User { get; set; }


    public int Id { get; set; }
    public bool WasDeleted { get; set; }
}
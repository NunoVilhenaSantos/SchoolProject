using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Data.Repositories.Enrollments;

public interface IReservationRepository : IGenericRepository<Enrollment>
{
    IOrderedQueryable<Enrollment> GetEnrollments();

    IOrderedQueryable<Enrollment> GetAllEnrollmentsWithUsers();

    IOrderedQueryable<Enrollment> GetEnrollmentById(int id);


    // -------------------------------------------------------------- //

    IOrderedQueryable<Enrollment> GetAllEnrollments();


    // -------------------------------------------------------------- //

    protected IOrderedQueryable<Enrollment> GetAllEnrollmentsAsync();

    IOrderedQueryable<Enrollment> GetEnrollment(int id);

    IOrderedQueryable<Enrollment> GetEnrollment(Student student);

    IOrderedQueryable<Enrollment> GetEnrollment(Discipline discipline);

    Task<IOrderedQueryable<Enrollment>> GetEnrollment(string userName);


    // -------------------------------------------------------------- //

    //protected IOrderedQueryable<ReservationDetailsTemp> GetDetailsTemp();


    //IOrderedQueryable<ReservationDetailsTemp> GetDetailsTemp(int studentId, int disciplineId);

    //IOrderedQueryable<ReservationDetailsTemp> GetDetailsTemp(Student student);

    //IOrderedQueryable<ReservationDetailsTemp> GetDetailsTemp(Discipline discipline);


    //Task<IOrderedQueryable<ReservationDetailsTemp>> GetDetailsTemp(string userName);


    // -------------------------------------------------------------- //


    //Task ModifyDetailTempQuantityAsync(int id, int quantity);

    //Task DeleteDetailTempAsync(int id);

    // Task AddBookEditionToReservationAsync(AddBookEditionViewModel model, string userName);

    //Task<bool> ConfirmReservationAsync(string userName);

    // Task<bool> PickupReservation(ReservationPickingViewModel model);


    // -------------------------------------------------------------- //

    IEnumerable<SelectListItem> GetComboStudents();

    IEnumerable<SelectListItem> GetComboDisciplines();

    // IEnumerable<SelectListItem> GetComboBookEditions(int bookId);
}
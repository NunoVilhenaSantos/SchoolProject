using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Enrollments;

public class ReservationRepository : GenericRepository<Enrollment>,
    IReservationRepository
{
    private readonly DataContextMySql _dataContext;
    private readonly IUserHelper _userHelper;


    public ReservationRepository(DataContextMySql dataContext,
        DataContextMySql dataContextMySql, DataContextMsSql dataContextMsSql,
        DataContextSqLite dataContextSqLite, IUserHelper userHelper) : base(
        dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
    }


    // -------------------------------------------------------------- //

    public IOrderedQueryable<Enrollment> GetEnrollments()
    {
        return _dataContext.Enrollments
            .Include(r => r.Student)
            .ThenInclude(r => r.AppUser)
            .Include(r => r.Discipline)
            .OrderBy(e => e.Student.FirstName);
    }


    public IOrderedQueryable<Enrollment> GetAllEnrollmentsWithUsers()
    {
        return _dataContext.Enrollments
            .Include(o => o.Student)
            .ThenInclude(r => r.AppUser)
            .Include(r => r.Discipline)
            // .Include(r => r.Items)
            .OrderBy(e => e.Student.FirstName);
    }


    public IOrderedQueryable<Enrollment> GetEnrollmentById(int id)
    {
        return _dataContext.Set<Enrollment>()
            .Include(r => r.Student)
            .ThenInclude(r => r.AppUser)
            .Include(r => r.Discipline)
            //.Include(r => r.Items)
            //.ThenInclude(i => i.BookEdition)
            .Where(e => e.Id == id)
            .OrderBy(e => e.Student.FirstName);
    }


    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //


    // -------------------------------------------------------------- //

    public IOrderedQueryable<Enrollment> GetAllEnrollments()
    {
        return _dataContext.Set<Enrollment>()
            .Include(o => o.Student)
            .ThenInclude(c => c.AppUser)
            .Include(o => o.Discipline)
            // .Include(o => o.Items)
            // .ThenInclude(i => i.BookEdition)
            .OrderBy(o => o.Student.FirstName);
    }


    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //


    public IOrderedQueryable<Enrollment> GetAllEnrollmentsAsync()
    {
        return _dataContext.Set<Enrollment>()
            .Include(o => o.Student)
            .ThenInclude(c => c.AppUser)
            .Include(o => o.Discipline)
            .ThenInclude(p => p.TeacherDisciplines)
            // .Include(o => o.Items)
            // .ThenInclude(i => i.BookEdition)
            // .ThenInclude(i => i.Book)
            .OrderBy(o => o.Id);
    }

    public IOrderedQueryable<Enrollment> GetEnrollment(int id)
    {
        return GetAllEnrollmentsAsync()
            .Where(o => o.Id == id)
            .OrderBy(o => o.Id);
    }


    public IOrderedQueryable<Enrollment> GetEnrollment(Student student)
    {
        return GetAllEnrollmentsAsync()
            .Where(o => o.Student == student)
            .OrderBy(o => o.Id);
    }

    public IOrderedQueryable<Enrollment> GetEnrollment(Discipline discipline)
    {
        return GetAllEnrollmentsAsync()
            .Where(o => o.Discipline == discipline)
            .OrderBy(o => o.Id);
    }


    public async Task<IOrderedQueryable<Enrollment>> GetEnrollment(
        string userName)
    {
        var user = await _userHelper.GetUserByEmailAsync(userName);

        if (user == null)
            return new EnumerableQuery<Enrollment>(Array.Empty<Enrollment>());

        return await _userHelper.IsUserInRoleAsync(user, "Admin")
            ? GetAllEnrollmentsAsync().OrderBy(o => o.Id)
            : GetAllEnrollmentsAsync().Where(o => o.Student.AppUser == user)
                .OrderBy(o => o.Id);
    }


    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //


    //public IOrderedQueryable<ReservationDetailsTemp> GetDetailsTempAsync()
    //{
    //    return _dataContext.Set<ReservationDetailsTemp>()
    //        .Include(o => o.Customer)
    //        .ThenInclude(c => c.AppUser)
    //        .Include(o => o.Library)
    //        .Include(o => o.BookEdition)
    //        .OrderBy(o => o.Id);
    //}

    //public IOrderedQueryable<ReservationDetailsTemp> GetDetailsTemp(int studentId = -1, int disciplineId = -1);
    //{
    //    return GetDetailsTempAsync()
    //        .Where(o => o.Customer.Id == customerId)
    //        .OrderBy(o => o.Id);
    //}

    //public IOrderedQueryable<ReservationDetailsTemp> GetDetailsTempAsync(
    //    Customer customer)
    //{
    //    return GetDetailsTempAsync()
    //        .Where(o => o.Customer == customer)
    //        .OrderBy(o => o.Id);
    //}
    //public IOrderedQueryable<ReservationDetailsTemp> GetDetailsTempAsync(Student student)
    //{
    //    throw new NotImplementedException();
    //}

    //public IOrderedQueryable<ReservationDetailsTemp> GetDetailsTempAsync(Discipline discipline)
    //{
    //    throw new NotImplementedException();
    //}


    //public async Task<IOrderedQueryable<ReservationDetailsTemp>>
    //    GetDetailsTempAsync(string userName)
    //{
    //    var user = await _userHelper.GetUserByEmailAsync(userName);

    //    if (user == null)
    //        return new EnumerableQuery<ReservationDetailsTemp>(
    //            Array.Empty<ReservationDetailsTemp>());

    //    return await _userHelper.IsUserInRoleAsync(user, "Admin")
    //        ? GetDetailsTempAsync()
    //            .OrderBy(o => o.Id)
    //        : GetDetailsTempAsync()
    //            .Where(o => o.Customer.AppUser == user)
    //            .OrderBy(o => o.Id);
    //}


    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //
    // -------------------------------------------------------------- //


    //public async Task ModifyDetailTempQuantityAsync(int id, int quantity)
    //{
    //    var orderDetailTemp =
    //        await _dataContext.Set<ReservationDetailsTemp>().FindAsync(id);

    //    if (orderDetailTemp == null) return;

    //    orderDetailTemp.Quantity += quantity;

    //    if (orderDetailTemp.Quantity > 0)
    //        _dataContext.Set<ReservationDetailsTemp>().Update(orderDetailTemp);
    //    else
    //        _dataContext.Set<ReservationDetailsTemp>().Remove(orderDetailTemp);

    //    await _dataContext.SaveChangesAsync();
    //}


    //public async Task DeleteDetailTempAsync(int id)
    //{
    //    var orderDetailTemp =
    //        await _dataContext.ReservationDetailsTemps.FindAsync(id);

    //    if (orderDetailTemp == null) return;

    //    _dataContext.ReservationDetailsTemps.Remove(orderDetailTemp);

    //    await _dataContext.SaveChangesAsync();
    //}


    // -------------------------------------------------------------- //


    //public async Task AddBookEditionToReservationAsync(
    //    AddBookEditionViewModel model, string userName)
    //{
    //    var user = await _userHelper.GetUserByEmailAsync(userName);
    //    if (user == null) return;

    //    var customer =
    //        await _dataContext.Set<Customer>().FindAsync(model.CustomerId);
    //    if (customer == null) return;

    //    var library =
    //        await _dataContext.Libraries.FindAsync(model.LibraryId);
    //    if (library == null) return;

    //    var bookEdition =
    //        await _dataContext.Set<BookEdition>()
    //            .FindAsync(model.BookEditionId);
    //    if (bookEdition == null) return;

    //    var loanDetailTemp =
    //        await _dataContext.ReservationDetailsTemps
    //            .Where(o =>
    //                o.Customer == customer &&
    //                o.Library == library &&
    //                o.BookEdition == bookEdition)
    //            .FirstOrDefaultAsync();

    //    if (loanDetailTemp == null)
    //    {
    //        loanDetailTemp = new ReservationDetailsTemp
    //        {
    //            CustomerId = customer.Id,
    //            Customer = customer,
    //            LibraryId = library.Id,
    //            Library = library,
    //            BookEditionId = bookEdition.Id,
    //            BookEdition = bookEdition,
    //            Quantity = model.Quantity,
    //            Items = null,
    //        };

    //        _dataContext.ReservationDetailsTemps.Add(loanDetailTemp);
    //    }
    //    else
    //    {
    //        loanDetailTemp.Quantity += model.Quantity;
    //        _dataContext.ReservationDetailsTemps.Update(loanDetailTemp);
    //    }

    //    await _dataContext.SaveChangesAsync();
    //}


    //public async Task<bool> ConfirmReservationAsync(string userName)
    //{
    //    var user = await _userHelper.GetUserByEmailAsync(userName);

    //    if (user == null) return false;
    //    var detailsTemps =
    //        await _dataContext.ReservationDetailsTemps
    //            .Include(o => o.Customer)
    //            .ThenInclude(c => c.AppUser)
    //            .Include(o => o.Library)
    //            .Include(o => o.BookEdition)
    //            .ThenInclude(o => o.Book)
    //            .Where(
    //                o =>
    //                    o.Customer.AppUser == user // && o.Library == library
    //            )
    //            .ToListAsync();

    //    if (detailsTemps.Count == 0) return false;

    //    var customer = detailsTemps.FirstOrDefault()?.Customer;
    //    if (customer == null) return false;

    //    var library = detailsTemps.FirstOrDefault()?.Library;
    //    if (library == null) return false;

    //    var loan = new Enrollment
    //    {
    //        CustomerId = customer.Id,
    //        Customer = customer,
    //        LibraryId = library.Id,
    //        Library = library,
    //        Items = null,
    //    };

    //    loan.Items = detailsTemps.Select(ldt => new ReservationDetail
    //    {
    //        ReservationId = loan.Id,
    //        Enrollment = loan,
    //        BookEditionId = ldt.BookEditionId,
    //        BookEdition = ldt.BookEdition,
    //        Quantity = ldt.Quantity,
    //    })
    //        .ToList();

    //    // _dataContext.Orders.Add(order);
    //    await CreateAsync(loan);
    //    await _dataContext.SaveChangesAsync();

    //    _dataContext.ReservationDetailsTemps.RemoveRange(detailsTemps);

    //    await _dataContext.SaveChangesAsync();

    //    return true;
    //}


    //public async Task<bool> PickupReservation(ReservationPickingViewModel model)
    //{
    //    var order = await _dataContext.Reservations.FindAsync(model.Id);

    //    if (order == null) return false;

    //    order.PickupDate = model.PickingReservationDate;

    //    _dataContext.Reservations.Update(order);

    //    await _dataContext.SaveChangesAsync();

    //    return true;
    //}


    public IEnumerable<SelectListItem> GetComboStudents()
    {
        var list = _dataContext.StudentDisciplines
            .Include(o => o.Student)
            .Include(o => o.Discipline)
            .Select(p => new SelectListItem
            {
                Text = $"{p.Student.FirstName} ({p.Student.FullName})",
                Value = p.Id.ToString()
            }).ToList();

        // list.Insert(0,
        //     new SelectListItem {Text = "(Select a Country....)", Value = "0"});

        return list;
    }

    public IEnumerable<SelectListItem> GetComboDisciplines()
    {
        var list = _dataContext.StudentDisciplines
            .Include(o => o.Student)
            .Include(o => o.Discipline)
            .Select(p => new SelectListItem
            {
                Text = $"{p.Discipline.Code} ({p.Discipline.Name})",
                Value = p.Id.ToString()
            }).ToList();

        // list.Insert(0,
        //     new SelectListItem {Text = "(Select a Country....)", Value = "0"});

        return list;
    }


    //public IEnumerable<SelectListItem>? GetComboBookEditions(int bookId)
    //{
    //    var book = _dataContext.Books
    //        .Include(o => o.BookEditions)
    //        .FirstOrDefault(c => c.Id == bookId);

    //    // Retornar uma opção vazia se o país não for encontrado
    //    if (book == null)
    //        return new List<SelectListItem>
    //            {new() {Text = "(Select a Book...)", Value = "0"}};

    //    var bookEditionsList = book.BookEditions != null
    //        ? book.BookEditions.Select(c => new SelectListItem
    //        {
    //            Text = $"{c.Title} - ({c.Edition}) ({c.Book.Title})",
    //            Value = c.Id.ToString()
    //        })
    //            .OrderBy(c => c.Text)
    //            .ToList()
    //        : new List<SelectListItem>
    //            {new() {Text = "(This book does not have a Book Edition...)", Value = "0"}};


    //    return bookEditionsList;
    //}
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Repositories.Enrollments;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///    EnrollmentsController class.
/// </summary>
public class EnrollmentsController : Controller
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly DataContextMySql _context;


    /// <summary>
    ///   EnrollmentsController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="enrollmentRepository"></param>
    public EnrollmentsController(
        DataContextMySql context, IEnrollmentRepository enrollmentRepository
    )
    {
        _context = context;
        _enrollmentRepository = enrollmentRepository;
    }


    // GET: Enrollments
    /// <summary>
    ///   Index method, for the main view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetEnrollmentsWithCoursesAndStudents());
    }


    private IEnumerable<Enrollment> GetEnrollmentsWithCoursesAndStudents()
    {
        //var citiesWithCountries =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        var enrollmentsWithStudent =
            _context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Include(e => e.CreatedBy)
                .Include(e => e.UpdatedBy);

        return enrollmentsWithStudent;
    }


    // GET: Enrollments
    /// <summary>
    ///   IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetEnrollmentsWithCoursesAndStudents());
    }


    // GET: Enrollments
    // /// <summary>
    // ///  Index1 method for the view to test pagination.
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var totalCount = _context.Enrollments.Count();
    //
    //     var records = _context.Enrollments
    //         .Skip((pageNumber - 1) * pageSize)
    //         .Take(pageSize)
    //         .ToList();
    //
    //     var model = new PaginationViewModel<Enrollment>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = totalCount
    //     };
    //
    //     return View(model);
    // }

    private List<Enrollment> GetEnrollmentsWithCoursesAndStudents(
        int pageNumber, int pageSize = 10)
    {
        //var citiesWithCountries =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        var records = GetEnrollmentsWithCoursesAndStudents()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return records;
    }


    // GET: Enrollments
    /// <summary>
    ///  IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = "FirstName")

    {
        // var records =
        //     GetEnrollmentsWithCoursesAndStudents(pageNumber, pageSize);

        // TODO: Fix the sort order
        // var model = new PaginationViewModel<Enrollment>
        // {
        //     Records = records,
        //     PageNumber = pageNumber,
        //     PageSize = pageSize,
        //     TotalCount = _context.Enrollments.Count(),
        //     SortOrder = "asc",
        // };

        var model = new PaginationViewModel<Enrollment>(
            GetEnrollmentsWithCoursesAndStudents().ToList(),
            pageNumber, pageSize,
            _context.Enrollments.Count(),
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Enrollments/Details/5
    /// <summary>
    ///  Details method.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .Include(e => e.CreatedBy)
            .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (enrollment == null) return NotFound();

        return View(enrollment);
    }


    // GET: Enrollments/Create
    /// <summary>
    /// Create method, for the create view.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code");

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address");

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        return View();
    }

    // POST: Enrollments/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Create method, for adding a new enrollment.
    /// </summary>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Enrollment enrollment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(enrollment);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                enrollment.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                enrollment.CreatedById);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                enrollment.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                enrollment.UpdatedById);

        return View(enrollment);
    }


    // GET: Enrollments/Edit/5
    /// <summary>
    /// Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment == null) return NotFound();

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                enrollment.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                enrollment.CreatedById);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                enrollment.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                enrollment.UpdatedById);

        return View(enrollment);
    }

    // POST: Enrollments/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Edit method, for editing a enrollment.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Enrollment enrollment)
    {
        if (id != enrollment.StudentId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(enrollment);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(enrollment.StudentId))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                enrollment.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                enrollment.CreatedById);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                enrollment.StudentId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                enrollment.UpdatedById);

        return View(enrollment);
    }

    // GET: Enrollments/Delete/5
    /// <summary>
    /// Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .Include(e => e.CreatedBy)
            .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        if (enrollment == null) return NotFound();

        return View(enrollment);
    }

    // POST: Enrollments/Delete/5
    /// <summary>
    /// DeleteConfirmed method, for deleting a enrollment.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment != null) _context.Enrollments.Remove(enrollment);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool EnrollmentExists(int id) =>
        _context.Enrollments.Any(e => e.StudentId == id);
}
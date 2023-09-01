using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Repositories.Courses;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     courses controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CoursesController : Controller
{
    private readonly ICourseRepository _courseRepository;
    private readonly DataContextMySql _context;

    private const string BucketName = "courses";

    /// <summary>
    ///    constructor for the courses controller
    /// </summary>
    /// <param name="context"></param>
    /// <param name="courseRepository"></param>
    public CoursesController(
        DataContextMySql context, ICourseRepository courseRepository
    )
    {
        _context = context;
        _courseRepository = courseRepository;
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///    Index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Courses
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(CoursesList());
    }


    private IEnumerable<Course> CoursesList()
    {
        //var coursesList =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        return _context.Courses.ToList();
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///   Index action cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Courses
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(CoursesList());
    }


    // GET: Courses
    // /// <summary>
    // ///     Action to show all the roles
    // /// </summary>
    // /// <returns>a list of roles</returns>
    // [HttpGet]
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records = CoursesList(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<Course>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Courses.Count(),
    //     };
    //
    //     return View(model);
    // }


    private List<Course> CoursesList(int pageNumber, int pageSize)
    {
        var records = CoursesList()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return records;
    }


    // GET: Courses
    /// <summary>
    ///  IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10)
    {
        var records = CoursesList(pageNumber, pageSize);

        var model = new PaginationViewModel<Course>
        {
            Records = records,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = _context.Courses.Count(),
        };

        return View(model);
    }


    // GET: Courses/Details/5
    /// <summary>
    ///  Details action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(m => m.Id == id);

        if (course == null) return NotFound();

        return View(course);
    }

    // GET: Courses/Create
    /// <summary>
    ///   Create action
    /// </summary>
    /// <returns></returns>
    public IActionResult Create() => View();


    // POST: Courses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///  Create action
    /// </summary>
    /// <param name="course"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        if (!ModelState.IsValid) return View(course);

        _context.Add(course);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Courses/Edit/5
    /// <summary>
    ///  Edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses.FindAsync(id);

        if (course == null) return NotFound();

        return View(course);
    }

    // POST: Courses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Edit action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="course"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course)
    {
        if (id != course.Id) return NotFound();

        if (!ModelState.IsValid) return View(course);

        try
        {
            _context.Update(course);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CourseExists(course.Id)) return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    // GET: Courses/Delete/5
    /// <summary>
    /// Delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(m => m.Id == id);

        if (course == null) return NotFound();

        return View(course);
    }

    // POST: Courses/Delete/5
    /// <summary>
    /// Delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);

        if (course != null) _context.Courses.Remove(course);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool CourseExists(int id) =>
        _context.Courses.Any(e => e.Id == id);
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Repositories.SchoolClasses;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///         School class with courses
/// </summary>
public class SchoolClassCoursesController : Controller
{
    private readonly ISchoolClassCourseRepository _schoolClassCourseRepository;
    private readonly DataContextMySql _context;


    /// <summary>
    ///    School class with courses
    /// </summary>
    /// <param name="context"></param>
    /// <param name="schoolClassCourseRepository"></param>
    public SchoolClassCoursesController(
        DataContextMySql context,
        ISchoolClassCourseRepository schoolClassCourseRepository
    )
    {
        _context = context;
        _schoolClassCourseRepository = schoolClassCourseRepository;
    }


    // GET: SchoolClassCourses
    /// <summary>
    ///    Index, list of school class with courses
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetSchoolClassesWithCourses());
    }


    private IEnumerable<SchoolClassCourse> GetSchoolClassesWithCourses()
    {
        var schoolClassesWithCourses =
            _context.SchoolClassCourses
                .Include(s => s.Course)
                .Include(s => s.SchoolClass)
                .Include(s => s.CreatedBy)
                .Include(s => s.UpdatedBy);

        return schoolClassesWithCourses;
    }


    // GET: SchoolClassCourses
    /// <summary>
    ///   Index with cards, list of school class with courses
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetSchoolClassesWithCourses());
    }


    // GET: SchoolClassCourses
    // /// <summary>
    // /// Index1 for testing pagination, list of school class with courses
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records =
    //         GetSchoolClassesWithCoursesList(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<SchoolClassCourse>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Genders.Count(),
    //     };
    //
    //     return View(model);
    // }


    private List<SchoolClassCourse> GetSchoolClassesWithCoursesList(
        int pageNumber = 1, int pageSize = 10) =>
        GetSchoolClassesWithCourses()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();


    // GET: SchoolClassCourses
    /// <summary>
    ///  Index with cards, list of school class with courses
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10)
    {
        var records =
            GetSchoolClassesWithCoursesList(pageNumber, pageSize);

        var model = new PaginationViewModel<SchoolClassCourse>
        {
            Records = records,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = _context.Genders.Count(),
        };

        return View(model);
    }


    // GET: SchoolClassCourses/Details/5
    /// <summary>
    ///   Details, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses
            .Include(s => s.Course)
            .Include(s => s.SchoolClass)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);

        if (schoolClassCourse == null) return NotFound();

        return View(schoolClassCourse);
    }


    // GET: SchoolClassCourses/Create
    /// <summary>
    ///  Create, school class with courses
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

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym");

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        return View();
    }

    // POST: SchoolClassCourses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Create, school class with courses
    /// </summary>
    /// <param name="schoolClassCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SchoolClassCourse schoolClassCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(schoolClassCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                schoolClassCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassCourse.SchoolClassId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.UpdatedById);

        return View(schoolClassCourse);
    }

    // GET: SchoolClassCourses/Edit/5
    /// <summary>
    /// Edit, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(id);

        if (schoolClassCourse == null) return NotFound();

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                schoolClassCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassCourse.SchoolClassId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.UpdatedById);

        return View(schoolClassCourse);
    }

    // POST: SchoolClassCourses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Edit, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schoolClassCourse"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, SchoolClassCourse schoolClassCourse)
    {
        if (id != schoolClassCourse.SchoolClassId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(schoolClassCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassCourseExists(schoolClassCourse.SchoolClassId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] =
            new SelectList(_context.Courses,
                "Id", "Code",
                schoolClassCourse.CourseId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.CreatedById);

        ViewData["SchoolClassId"] =
            new SelectList(_context.SchoolClasses,
                "Id", "Acronym",
                schoolClassCourse.SchoolClassId);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id",
                schoolClassCourse.UpdatedById);

        return View(schoolClassCourse);
    }


    // GET: SchoolClassCourses/Delete/5
    /// <summary>
    /// Delete, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses
            .Include(s => s.Course)
            .Include(s => s.SchoolClass)
            .Include(s => s.CreatedBy)
            .Include(s => s.UpdatedBy)
            .FirstOrDefaultAsync(m => m.SchoolClassId == id);

        if (schoolClassCourse == null) return NotFound();

        return View(schoolClassCourse);
    }


    // POST: SchoolClassCourses/Delete/5
    /// <summary>
    /// Delete, school class with courses
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(id);

        if (schoolClassCourse != null)
            _context.SchoolClassCourses.Remove(schoolClassCourse);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool SchoolClassCourseExists(int id) =>
        _context.SchoolClassCourses
            .Any(e => e.SchoolClassId == id);
}
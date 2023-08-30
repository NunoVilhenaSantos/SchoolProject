using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Controllers;

public class TeacherCoursesController : Controller
{
    private readonly DataContextMySql _context;

    public TeacherCoursesController(DataContextMySql context)
    {
        _context = context;
    }



    private IEnumerable<TeacherCourse> GetTeacherCoursesList()
    {
        var teacherCoursesList =
            _context.TeacherCourses
            .Include(t => t.Course)
            .Include(t => t.Teacher)
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .ToListAsync();

        return teacherCoursesList.Result ?? Enumerable.Empty<TeacherCourse>();
    }




    // GET: TeacherCourses
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeacherCoursesList());
    }


    // GET: TeacherCourses
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeacherCoursesList());
    }


    // GET: TeacherCourses
    [HttpGet]
    public IActionResult Index2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeacherCoursesList());
    }


    // GET: TeacherCourses
    [HttpGet]
    public IActionResult IndexCards2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeacherCoursesList());
    }




    // GET: TeacherCourses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses
            .Include(t => t.Course)
            .Include(t => t.Teacher)
            .Include(t => t.CreatedBy)
            .Include(t => t.UpdatedBy)
            .FirstOrDefaultAsync(m => m.TeacherId == id);

        if (teacherCourse == null) return NotFound();

        return View(teacherCourse);
    }

    // GET: TeacherCourses/Create
    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code");
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["TeacherId"] =
            new SelectList(_context.Teachers, "Id", "Address");
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: TeacherCourses/Create
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "TeacherId,CourseId,CreatedById,UpdatedById,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        TeacherCourse teacherCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(teacherCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            teacherCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            teacherCourse.CreatedById);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id",
            "Address", teacherCourse.TeacherId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            teacherCourse.UpdatedById);
        return View(teacherCourse);
    }

    // GET: TeacherCourses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses.FindAsync(id);
        if (teacherCourse == null) return NotFound();
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            teacherCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            teacherCourse.CreatedById);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id",
            "Address", teacherCourse.TeacherId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            teacherCourse.UpdatedById);
        return View(teacherCourse);
    }

    // POST: TeacherCourses/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            "TeacherId,CourseId,CreatedById,UpdatedById,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        TeacherCourse teacherCourse)
    {
        if (id != teacherCourse.TeacherId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacherCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherCourseExists(teacherCourse.TeacherId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            teacherCourse.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            teacherCourse.CreatedById);
        ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id",
            "Address", teacherCourse.TeacherId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            teacherCourse.UpdatedById);
        return View(teacherCourse);
    }

    // GET: TeacherCourses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses
            .Include(t => t.Course)
            .Include(t => t.CreatedBy)
            .Include(t => t.Teacher)
            .Include(t => t.UpdatedBy)
            .FirstOrDefaultAsync(m => m.TeacherId == id);
        if (teacherCourse == null) return NotFound();

        return View(teacherCourse);
    }

    // POST: TeacherCourses/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.TeacherCourses == null)
            return Problem(
                "Entity set 'DataContextMySql.TeacherCourses'  is null.");
        var teacherCourse = await _context.TeacherCourses.FindAsync(id);
        if (teacherCourse != null)
            _context.TeacherCourses.Remove(teacherCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TeacherCourseExists(int id)
    {
        return (_context.TeacherCourses?.Any(e => e.TeacherId == id))
            .GetValueOrDefault();
    }
}
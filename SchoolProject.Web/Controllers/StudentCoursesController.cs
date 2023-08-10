using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Students;

namespace SchoolProject.Web.Controllers;

public class StudentCoursesController : Controller
{
    private readonly DataContextMySql _context;

    public StudentCoursesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: StudentCourses
    public async Task<IActionResult> Index()
    {
        var dataContextMySql = _context.StudentCourses.Include(navigationPropertyPath: s => s.Course)
            .Include(navigationPropertyPath: s => s.CreatedBy).Include(navigationPropertyPath: s => s.Student)
            .Include(navigationPropertyPath: s => s.UpdatedBy);
        return View(model: await dataContextMySql.ToListAsync());
    }

    // GET: StudentCourses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .Include(navigationPropertyPath: s => s.Course)
            .Include(navigationPropertyPath: s => s.CreatedBy)
            .Include(navigationPropertyPath: s => s.Student)
            .Include(navigationPropertyPath: s => s.UpdatedBy)
            .FirstOrDefaultAsync(predicate: m => m.StudentId == id);
        if (studentCourse == null) return NotFound();

        return View(model: studentCourse);
    }

    // GET: StudentCourses/Create
    public IActionResult Create()
    {
        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code");
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id");
        ViewData[index: "StudentId"] =
            new SelectList(items: _context.Students, dataValueField: "Id", dataTextField: "Address");
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id");
        return View();
    }

    // POST: StudentCourses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            include: "StudentId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        StudentCourse studentCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: studentCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: studentCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: studentCourse.CreatedById);
        ViewData[index: "StudentId"] = new SelectList(items: _context.Students, dataValueField: "Id",
            dataTextField: "Address", selectedValue: studentCourse.StudentId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: studentCourse.UpdatedById);
        return View(model: studentCourse);
    }

    // GET: StudentCourses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses.FindAsync(keyValues: id);
        if (studentCourse == null) return NotFound();
        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: studentCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: studentCourse.CreatedById);
        ViewData[index: "StudentId"] = new SelectList(items: _context.Students, dataValueField: "Id",
            dataTextField: "Address", selectedValue: studentCourse.StudentId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: studentCourse.UpdatedById);
        return View(model: studentCourse);
    }

    // POST: StudentCourses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            include: "StudentId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        StudentCourse studentCourse)
    {
        if (id != studentCourse.StudentId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: studentCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentCourseExists(id: studentCourse.StudentId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: studentCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: studentCourse.CreatedById);
        ViewData[index: "StudentId"] = new SelectList(items: _context.Students, dataValueField: "Id",
            dataTextField: "Address", selectedValue: studentCourse.StudentId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: studentCourse.UpdatedById);
        return View(model: studentCourse);
    }

    // GET: StudentCourses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.StudentCourses == null) return NotFound();

        var studentCourse = await _context.StudentCourses
            .Include(navigationPropertyPath: s => s.Course)
            .Include(navigationPropertyPath: s => s.CreatedBy)
            .Include(navigationPropertyPath: s => s.Student)
            .Include(navigationPropertyPath: s => s.UpdatedBy)
            .FirstOrDefaultAsync(predicate: m => m.StudentId == id);
        if (studentCourse == null) return NotFound();

        return View(model: studentCourse);
    }

    // POST: StudentCourses/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.StudentCourses == null)
            return Problem(
                detail: "Entity set 'DataContextMySql.StudentCourses'  is null.");
        var studentCourse = await _context.StudentCourses.FindAsync(keyValues: id);
        if (studentCourse != null)
            _context.StudentCourses.Remove(entity: studentCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool StudentCourseExists(int id)
    {
        return (_context.StudentCourses?.Any(predicate: e => e.StudentId == id))
            .GetValueOrDefault();
    }
}
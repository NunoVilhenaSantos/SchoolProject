using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Controllers;

public class TeacherCoursesController : Controller
{
    private readonly DataContextMySql _context;

    public TeacherCoursesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: TeacherCourses
    public async Task<IActionResult> Index()
    {
        var dataContextMySql = _context.TeacherCourses.Include(navigationPropertyPath: t => t.Course)
            .Include(navigationPropertyPath: t => t.CreatedBy).Include(navigationPropertyPath: t => t.Teacher)
            .Include(navigationPropertyPath: t => t.UpdatedBy);
        return View(model: await dataContextMySql.ToListAsync());
    }

    // GET: TeacherCourses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses
            .Include(navigationPropertyPath: t => t.Course)
            .Include(navigationPropertyPath: t => t.CreatedBy)
            .Include(navigationPropertyPath: t => t.Teacher)
            .Include(navigationPropertyPath: t => t.UpdatedBy)
            .FirstOrDefaultAsync(predicate: m => m.TeacherId == id);
        if (teacherCourse == null) return NotFound();

        return View(model: teacherCourse);
    }

    // GET: TeacherCourses/Create
    public IActionResult Create()
    {
        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code");
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id");
        ViewData[index: "TeacherId"] =
            new SelectList(items: _context.Teachers, dataValueField: "Id", dataTextField: "Address");
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id");
        return View();
    }

    // POST: TeacherCourses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            include: "TeacherId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        TeacherCourse teacherCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: teacherCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: teacherCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: teacherCourse.CreatedById);
        ViewData[index: "TeacherId"] = new SelectList(items: _context.Teachers, dataValueField: "Id",
            dataTextField: "Address", selectedValue: teacherCourse.TeacherId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: teacherCourse.UpdatedById);
        return View(model: teacherCourse);
    }

    // GET: TeacherCourses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses.FindAsync(keyValues: id);
        if (teacherCourse == null) return NotFound();
        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: teacherCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: teacherCourse.CreatedById);
        ViewData[index: "TeacherId"] = new SelectList(items: _context.Teachers, dataValueField: "Id",
            dataTextField: "Address", selectedValue: teacherCourse.TeacherId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: teacherCourse.UpdatedById);
        return View(model: teacherCourse);
    }

    // POST: TeacherCourses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            include: "TeacherId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        TeacherCourse teacherCourse)
    {
        if (id != teacherCourse.TeacherId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: teacherCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherCourseExists(id: teacherCourse.TeacherId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: teacherCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: teacherCourse.CreatedById);
        ViewData[index: "TeacherId"] = new SelectList(items: _context.Teachers, dataValueField: "Id",
            dataTextField: "Address", selectedValue: teacherCourse.TeacherId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: teacherCourse.UpdatedById);
        return View(model: teacherCourse);
    }

    // GET: TeacherCourses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.TeacherCourses == null) return NotFound();

        var teacherCourse = await _context.TeacherCourses
            .Include(navigationPropertyPath: t => t.Course)
            .Include(navigationPropertyPath: t => t.CreatedBy)
            .Include(navigationPropertyPath: t => t.Teacher)
            .Include(navigationPropertyPath: t => t.UpdatedBy)
            .FirstOrDefaultAsync(predicate: m => m.TeacherId == id);
        if (teacherCourse == null) return NotFound();

        return View(model: teacherCourse);
    }

    // POST: TeacherCourses/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.TeacherCourses == null)
            return Problem(
                detail: "Entity set 'DataContextMySql.TeacherCourses'  is null.");
        var teacherCourse = await _context.TeacherCourses.FindAsync(keyValues: id);
        if (teacherCourse != null)
            _context.TeacherCourses.Remove(entity: teacherCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool TeacherCourseExists(int id)
    {
        return (_context.TeacherCourses?.Any(predicate: e => e.TeacherId == id))
            .GetValueOrDefault();
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;

namespace SchoolProject.Web.Controllers;

public class SchoolClassCoursesController : Controller
{
    private readonly DataContextMySql _context;

    public SchoolClassCoursesController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: SchoolClassCourses
    public async Task<IActionResult> Index()
    {
        var dataContextMySql = _context.SchoolClassCourses
            .Include(navigationPropertyPath: s => s.Course).Include(navigationPropertyPath: s => s.CreatedBy)
            .Include(navigationPropertyPath: s => s.SchoolClass).Include(navigationPropertyPath: s => s.UpdatedBy);
        return View(model: await dataContextMySql.ToListAsync());
    }

    // GET: SchoolClassCourses/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses
            .Include(navigationPropertyPath: s => s.Course)
            .Include(navigationPropertyPath: s => s.CreatedBy)
            .Include(navigationPropertyPath: s => s.SchoolClass)
            .Include(navigationPropertyPath: s => s.UpdatedBy)
            .FirstOrDefaultAsync(predicate: m => m.SchoolClassId == id);
        if (schoolClassCourse == null) return NotFound();

        return View(model: schoolClassCourse);
    }

    // GET: SchoolClassCourses/Create
    public IActionResult Create()
    {
        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code");
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id");
        ViewData[index: "SchoolClassId"] =
            new SelectList(items: _context.SchoolClasses, dataValueField: "Id", dataTextField: "Acronym");
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id");
        return View();
    }

    // POST: SchoolClassCourses/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            include: "SchoolClassId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        SchoolClassCourse schoolClassCourse)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: schoolClassCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: schoolClassCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: schoolClassCourse.CreatedById);
        ViewData[index: "SchoolClassId"] = new SelectList(items: _context.SchoolClasses, dataValueField: "Id",
            dataTextField: "Acronym", selectedValue: schoolClassCourse.SchoolClassId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: schoolClassCourse.UpdatedById);
        return View(model: schoolClassCourse);
    }

    // GET: SchoolClassCourses/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(keyValues: id);
        if (schoolClassCourse == null) return NotFound();
        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: schoolClassCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: schoolClassCourse.CreatedById);
        ViewData[index: "SchoolClassId"] = new SelectList(items: _context.SchoolClasses, dataValueField: "Id",
            dataTextField: "Acronym", selectedValue: schoolClassCourse.SchoolClassId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: schoolClassCourse.UpdatedById);
        return View(model: schoolClassCourse);
    }

    // POST: SchoolClassCourses/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            include: "SchoolClassId,CourseId,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        SchoolClassCourse schoolClassCourse)
    {
        if (id != schoolClassCourse.SchoolClassId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: schoolClassCourse);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolClassCourseExists(id: schoolClassCourse.SchoolClassId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: schoolClassCourse.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: schoolClassCourse.CreatedById);
        ViewData[index: "SchoolClassId"] = new SelectList(items: _context.SchoolClasses, dataValueField: "Id",
            dataTextField: "Acronym", selectedValue: schoolClassCourse.SchoolClassId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: schoolClassCourse.UpdatedById);
        return View(model: schoolClassCourse);
    }

    // GET: SchoolClassCourses/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.SchoolClassCourses == null)
            return NotFound();

        var schoolClassCourse = await _context.SchoolClassCourses
            .Include(navigationPropertyPath: s => s.Course)
            .Include(navigationPropertyPath: s => s.CreatedBy)
            .Include(navigationPropertyPath: s => s.SchoolClass)
            .Include(navigationPropertyPath: s => s.UpdatedBy)
            .FirstOrDefaultAsync(predicate: m => m.SchoolClassId == id);
        if (schoolClassCourse == null) return NotFound();

        return View(model: schoolClassCourse);
    }

    // POST: SchoolClassCourses/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.SchoolClassCourses == null)
            return Problem(
                detail: "Entity set 'DataContextMySql.SchoolClassCourses'  is null.");
        var schoolClassCourse = await _context.SchoolClassCourses.FindAsync(keyValues: id);
        if (schoolClassCourse != null)
            _context.SchoolClassCourses.Remove(entity: schoolClassCourse);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool SchoolClassCourseExists(int id)
    {
        return (_context.SchoolClassCourses?.Any(predicate: e => e.SchoolClassId == id))
            .GetValueOrDefault();
    }
}
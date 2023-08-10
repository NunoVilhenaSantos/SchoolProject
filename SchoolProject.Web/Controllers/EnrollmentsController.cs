using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Enrollments;

namespace SchoolProject.Web.Controllers;

public class EnrollmentsController : Controller
{
    private readonly DataContextMySql _context;

    public EnrollmentsController(DataContextMySql context)
    {
        _context = context;
    }

    // GET: Enrollments
    public async Task<IActionResult> Index()
    {
        var dataContextMySql = _context.Enrollments.Include(navigationPropertyPath: e => e.Course)
            .Include(navigationPropertyPath: e => e.CreatedBy).Include(navigationPropertyPath: e => e.Student)
            .Include(navigationPropertyPath: e => e.UpdatedBy);
        return View(model: await dataContextMySql.ToListAsync());
    }

    // GET: Enrollments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Enrollments == null) return NotFound();

        var enrollment = await _context.Enrollments
            .Include(navigationPropertyPath: e => e.Course)
            .Include(navigationPropertyPath: e => e.CreatedBy)
            .Include(navigationPropertyPath: e => e.Student)
            .Include(navigationPropertyPath: e => e.UpdatedBy)
            .FirstOrDefaultAsync(predicate: m => m.StudentId == id);
        if (enrollment == null) return NotFound();

        return View(model: enrollment);
    }

    // GET: Enrollments/Create
    public IActionResult Create()
    {
        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code");
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id");
        ViewData[index: "StudentId"] =
            new SelectList(items: _context.Students, dataValueField: "Id", dataTextField: "Address");
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id");
        return View();
    }

    // POST: Enrollments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            include: "StudentId,CourseId,Grade,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        Enrollment enrollment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(entity: enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(actionName: nameof(Index));
        }

        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: enrollment.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: enrollment.CreatedById);
        ViewData[index: "StudentId"] = new SelectList(items: _context.Students, dataValueField: "Id",
            dataTextField: "Address", selectedValue: enrollment.StudentId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: enrollment.UpdatedById);
        return View(model: enrollment);
    }

    // GET: Enrollments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Enrollments == null) return NotFound();

        var enrollment = await _context.Enrollments.FindAsync(keyValues: id);
        if (enrollment == null) return NotFound();
        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: enrollment.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: enrollment.CreatedById);
        ViewData[index: "StudentId"] = new SelectList(items: _context.Students, dataValueField: "Id",
            dataTextField: "Address", selectedValue: enrollment.StudentId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: enrollment.UpdatedById);
        return View(model: enrollment);
    }

    // POST: Enrollments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            include: "StudentId,CourseId,Grade,Id,IdGuid,WasDeleted,CreatedAt,CreatedById,UpdatedAt,UpdatedById")]
        Enrollment enrollment)
    {
        if (id != enrollment.StudentId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(entity: enrollment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(id: enrollment.StudentId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(actionName: nameof(Index));
        }

        ViewData[index: "CourseId"] = new SelectList(items: _context.Courses, dataValueField: "Id", dataTextField: "Code",
            selectedValue: enrollment.CourseId);
        ViewData[index: "CreatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: enrollment.CreatedById);
        ViewData[index: "StudentId"] = new SelectList(items: _context.Students, dataValueField: "Id",
            dataTextField: "Address", selectedValue: enrollment.StudentId);
        ViewData[index: "UpdatedById"] = new SelectList(items: _context.Users, dataValueField: "Id", dataTextField: "Id",
            selectedValue: enrollment.UpdatedById);
        return View(model: enrollment);
    }

    // GET: Enrollments/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Enrollments == null) return NotFound();

        var enrollment = await _context.Enrollments
            .Include(navigationPropertyPath: e => e.Course)
            .Include(navigationPropertyPath: e => e.CreatedBy)
            .Include(navigationPropertyPath: e => e.Student)
            .Include(navigationPropertyPath: e => e.UpdatedBy)
            .FirstOrDefaultAsync(predicate: m => m.StudentId == id);
        if (enrollment == null) return NotFound();

        return View(model: enrollment);
    }

    // POST: Enrollments/Delete/5
    [HttpPost]
    [ActionName(name: "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Enrollments == null)
            return Problem(
                detail: "Entity set 'DataContextMySql.Enrollments'  is null.");
        var enrollment = await _context.Enrollments.FindAsync(keyValues: id);
        if (enrollment != null) _context.Enrollments.Remove(entity: enrollment);

        await _context.SaveChangesAsync();
        return RedirectToAction(actionName: nameof(Index));
    }

    private bool EnrollmentExists(int id)
    {
        return (_context.Enrollments?.Any(predicate: e => e.StudentId == id))
            .GetValueOrDefault();
    }
}
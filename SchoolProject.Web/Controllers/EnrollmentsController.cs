using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Enrollments;

namespace SchoolProject.Web.Controllers;

public class EnrollmentsController : Controller
{
    private readonly DataContextMySql _context;


    public EnrollmentsController(DataContextMySql context)
    {
        _context = context;
    }




    private IEnumerable<Enrollment> EnrollmentsWithStudent()
    {
        //var citiesWithCountries =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        var enrollmentsWithStudent =
            _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .Include(e => e.CreatedBy)
            .Include(e => e.UpdatedBy);


        return enrollmentsWithStudent ?? Enumerable.Empty<Enrollment>();
    }


    // GET: Enrollments
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(EnrollmentsWithStudent());
    }


    // GET: Enrollments
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(EnrollmentsWithStudent());
    }



    // GET: Cities
    public IActionResult Index2(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = _context.Enrollments.Count();

        var records = _context.Enrollments
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        //var model = new PaginationViewModel
        //{
        //    Records = records,
        //    PageNumber = pageNumber,
        //    PageSize = pageSize,
        //    TotalCount = totalCount
        //};

        //return View(model);
        return View(records);
        
    }

    // GET: Cities
    public IActionResult IndexCards2(int pageNumber = 1, int pageSize = 10)
    {
        var totalCount = _context.Enrollments.Count();

        var records = _context.Enrollments
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        //var model = new PaginationViewModel
        //{
        //    Records = records,
        //    PageNumber = pageNumber,
        //    PageSize = pageSize,
        //    TotalCount = totalCount
        //};

        //return View(model);
        return View(records);
    }



    // GET: Enrollments/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Enrollments == null) return NotFound();

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.CreatedBy)
            .Include(e => e.Student)
            .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);
        if (enrollment == null) return NotFound();

        return View(enrollment);
    }

    // GET: Enrollments/Create
    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code");
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["StudentId"] =
            new SelectList(_context.Students, "Id", "Address");
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: Enrollments/Create
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind(
            "StudentId,CourseId,Grade,CreatedById,UpdatedById,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Enrollment enrollment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            enrollment.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            enrollment.CreatedById);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", enrollment.StudentId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            enrollment.UpdatedById);
        return View(enrollment);
    }

    // GET: Enrollments/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Enrollments == null) return NotFound();

        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null) return NotFound();
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            enrollment.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            enrollment.CreatedById);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", enrollment.StudentId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            enrollment.UpdatedById);
        return View(enrollment);
    }

    // POST: Enrollments/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind(
            "StudentId,CourseId,Grade,CreatedById,UpdatedById,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Enrollment enrollment)
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

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Code",
            enrollment.CourseId);
        ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id",
            enrollment.CreatedById);
        ViewData["StudentId"] = new SelectList(_context.Students, "Id",
            "Address", enrollment.StudentId);
        ViewData["UpdatedById"] = new SelectList(_context.Users, "Id", "Id",
            enrollment.UpdatedById);
        return View(enrollment);
    }

    // GET: Enrollments/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Enrollments == null) return NotFound();

        var enrollment = await _context.Enrollments
            .Include(e => e.Course)
            .Include(e => e.CreatedBy)
            .Include(e => e.Student)
            .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);
        if (enrollment == null) return NotFound();

        return View(enrollment);
    }

    // POST: Enrollments/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Enrollments == null)
            return Problem(
                "Entity set 'DataContextMySql.Enrollments'  is null.");
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment != null) _context.Enrollments.Remove(enrollment);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EnrollmentExists(int id)
    {
        return (_context.Enrollments?.Any(e => e.StudentId == id))
            .GetValueOrDefault();
    }
}
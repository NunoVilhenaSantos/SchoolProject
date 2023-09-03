using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Repositories.SchoolClasses;
using SchoolProject.Web.Models;


namespace SchoolProject.Web.Controllers;

/// <summary>
/// SchoolClassesController
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class SchoolClassesController : Controller
{
    private readonly ISchoolClassRepository _schoolClassRepository;
    private readonly DataContextMySql _context;

    private const string BucketName = "schoolclasses";


    /// <summary>
    /// SchoolClassesController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="schoolClassRepository"></param>
    public SchoolClassesController(
        DataContextMySql context,
        ISchoolClassRepository schoolClassRepository
    )
    {
        _context = context;
        _schoolClassRepository = schoolClassRepository;
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    /// Index
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: SchoolClasses
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetSchoolClasses());
    }


    private IEnumerable<SchoolClass> GetSchoolClasses() =>
        _context.SchoolClasses.ToListAsync().Result;


    // Allow unrestricted access to the Index action
    /// <summary>
    /// Index cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: SchoolClasses
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetSchoolClasses());
    }


    // Allow unrestricted access to the Index action
    // /// <summary>
    // /// Index 1 method, for the main view, for testing purposes.
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // [AllowAnonymous]
    // // GET: SchoolClasses
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records = GetSchoolClassesList(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<SchoolClass>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Teachers.Count(),
    //     };
    //
    //     return View(model);
    // }


    private List<SchoolClass> GetSchoolClasses(
        int pageNumber, int pageSize) =>
        GetSchoolClasses()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();


    // Allow unrestricted access to the Index action
    /// <summary>
    /// Index cards 1 method, for the main view, for testing purposes.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: SchoolClasses
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = "FirstName")
    {
        // var records = GetSchoolClasses(pageNumber, pageSize);

        // TODO: Fix the sort order
        // var model = new PaginationViewModel<SchoolClass>
        // {
        //     Records = records,
        //     PageNumber = pageNumber,
        //     PageSize = pageSize,
        //     TotalCount = _context.SchoolClasses.Count(),
        //     SortOrder = "asc",
        // };

        var model = new PaginationViewModel<SchoolClass>(
            GetSchoolClasses().ToList(),
            pageNumber, pageSize,
            _context.SchoolClasses.Count(),
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: SchoolClasses/Details/5
    /// <summary>
    /// Details of a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var schoolClass = await _context.SchoolClasses
            .FirstOrDefaultAsync(m => m.Id == id);

        if (schoolClass == null) return NotFound();

        return View(schoolClass);
    }

    // GET: SchoolClasses/Create
    /// <summary>
    /// Create a new school class, view.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create() => View();


    // POST: SchoolClasses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Create a new school class, post.
    /// validates and saves the new school class.
    /// </summary>
    /// <param name="schoolClass"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SchoolClass schoolClass)
    {
        if (!ModelState.IsValid) return View(schoolClass);

        _context.Add(schoolClass);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: SchoolClasses/Edit/5
    /// <summary>
    /// Edit a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var schoolClass = await _context.SchoolClasses.FindAsync(id);

        if (schoolClass == null) return NotFound();

        return View(schoolClass);
    }

    // POST: SchoolClasses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Edit a school class, post.
    /// validate and save the edited school class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="schoolClass"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SchoolClass schoolClass)
    {
        if (id != schoolClass.Id) return NotFound();

        if (!ModelState.IsValid) return View(schoolClass);

        try
        {
            _context.Update(schoolClass);

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SchoolClassExists(schoolClass.Id)) return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    // GET: SchoolClasses/Delete/5
    /// <summary>
    /// Delete a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var schoolClass = await _context.SchoolClasses
            .FirstOrDefaultAsync(m => m.Id == id);

        if (schoolClass == null) return NotFound();

        return View(schoolClass);
    }


    // POST: SchoolClasses/Delete/5
    /// <summary>
    /// Delete a school class, post.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var schoolClass = await _context.SchoolClasses.FindAsync(id);

        if (schoolClass != null) _context.SchoolClasses.Remove(schoolClass);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool SchoolClassExists(int id) =>
        _context.SchoolClasses.Any(e => e.Id == id);
}
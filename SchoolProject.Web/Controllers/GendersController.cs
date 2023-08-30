using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.OtherEntities;

namespace SchoolProject.Web.Controllers;

public class GendersController : Controller
{
    private readonly DataContextMySql _context;


    private const string BucketName = "genders";


    public GendersController(DataContextMySql context)
    {
        _context = context;
    }



    private IEnumerable<Gender> GendersList()
    {
        //var citiesWithCountries =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        var gendersList = _context.Genders.ToListAsync();

        return gendersList.Result ?? Enumerable.Empty<Gender>();
    }


    // GET: Genders
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GendersList());
    }


    // GET: Genders
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GendersList());
    }

    // GET: Genders
    public IActionResult Index2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GendersList());
    }


    // GET: Genders
    public IActionResult IndexCards2(int pageNumber = 1, int pageSize = 10)
    {
        return View(GendersList());
    }


    // GET: Genders/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var gender = await _context.Genders
            .FirstOrDefaultAsync(m => m.Id == id);

        if (gender == null) return NotFound();

        return View(gender);
    }

    // GET: Genders/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Genders/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Gender gender)
    {
        if (!ModelState.IsValid) return View(gender);

        _context.Add(gender);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

    // GET: Genders/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Genders == null) return NotFound();

        var gender = await _context.Genders.FindAsync(id);
        if (gender == null) return NotFound();
        return View(gender);
    }

    // POST: Genders/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Name,ProfilePhotoId,Id,IdGuid,WasDeleted,CreatedAt,UpdatedAt")]
        Gender gender)
    {
        if (id != gender.Id) return NotFound();

        if (!ModelState.IsValid) return View(gender);

        try
        {
            _context.Update(gender);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GenderExists(gender.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));

    }

    // GET: Genders/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var gender = await _context.Genders
            .FirstOrDefaultAsync(m => m.Id == id);

        if (gender == null) return NotFound();

        return View(gender);
    }

    // POST: Genders/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Genders == null)
            return Problem("Entity set 'DataContextMySql.Genders' is null.");

        var gender = await _context.Genders.FindAsync(id);
        if (gender != null) _context.Genders.Remove(gender);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GenderExists(int id)
    {
        return (_context.Genders?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
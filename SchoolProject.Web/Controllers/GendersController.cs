using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Repositories.OtherEntities;
using SchoolProject.Web.Models;


namespace SchoolProject.Web.Controllers;

/// <summary>
///     GendersController class.
/// </summary>
public class GendersController : Controller
{
    private readonly IGenderRepository _genderRepository;
    private readonly DataContextMySql _context;


    private const string BucketName = "genders";


    /// <summary>
    ///  GendersController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="genderRepository"></param>
    public GendersController(
        DataContextMySql context, IGenderRepository genderRepository
    )
    {
        _context = context;
        _genderRepository = genderRepository;
    }


    private IEnumerable<Gender> GendersList()
    {
        //var citiesWithCountries =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        return _context.Genders.ToListAsync().Result;
    }


    // GET: Genders
    /// <summary>
    ///  Index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GendersList());
    }


    // GET: Genders
    /// <summary>
    /// Index action cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GendersList());
    }


    // GET: Genders
    // /// <summary>
    // /// Index action for testing the pagination
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records = GendersList(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<Gender>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Teachers.Count(),
    //     };
    //
    //     return View(model);
    // }


    private List<Gender> GendersList(int pageNumber, int pageSize)
    {
        var records = GendersList()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return records;
    }


    // GET: Genders
    /// <summary>
    ///  IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10)
    {
        var records = GendersList(pageNumber, pageSize);

        var model = new PaginationViewModel<Gender>
        {
            Records = records,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = _context.Teachers.Count(),
        };

        return View(model);
    }


    // GET: Genders/Details/5
    /// <summary>
    /// Details action, to open the view for details.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var gender = await _context.Genders
            .FirstOrDefaultAsync(m => m.Id == id);

        if (gender == null) return NotFound();

        return View(gender);
    }

    // GET: Genders/Create
    /// <summary>
    ///    Create action, to open the view for creating.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create() => View();


    // POST: Genders/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///    Create action validation and confirmation.
    /// </summary>
    /// <param name="gender"></param>
    /// <returns></returns>
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
    /// <summary>
    ///     Edit action, to open the view for editing.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var gender = await _context.Genders.FindAsync(id);

        if (gender == null) return NotFound();

        return View(gender);
    }

    // POST: Genders/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    /// Edit action validation and confirmation.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="gender"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Gender gender)
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
            if (!GenderExists(gender.Id)) return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Genders/Delete/5
    /// <summary>
    /// Delete action, to open the view for confirmation.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var gender = await _context.Genders
            .FirstOrDefaultAsync(m => m.Id == id);

        if (gender == null) return NotFound();

        return View(gender);
    }

    // POST: Genders/Delete/5
    /// <summary>
    /// Delete action confirmed.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var gender = await _context.Genders.FindAsync(id);

        if (gender != null) _context.Genders.Remove(gender);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool GenderExists(int id) =>
        _context.Genders.Any(e => e.Id == id);
}
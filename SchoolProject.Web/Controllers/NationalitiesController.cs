using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;


namespace SchoolProject.Web.Controllers;

/// <summary>
/// Controller for the Nationalities entity.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class NationalitiesController : Controller
{
    private readonly INationalityRepository _nationalityRepository;


    /// <summary>
    /// Constructor for the NationalitiesController.
    /// </summary>
    /// <param name="countryRepository"></param>
    /// <param name="nationalityRepository"></param>
    public NationalitiesController(
        ICountryRepository countryRepository,
        INationalityRepository nationalityRepository
    )
    {
        _nationalityRepository = nationalityRepository;
    }


    private IEnumerable<Nationality> GetNationalitiesWithCountries()
    {
        var nationalitiesWithCountries =
            _nationalityRepository?.GetNationalitiesWithCountries();

        return nationalitiesWithCountries ?? Enumerable.Empty<Nationality>();
    }


    /// <summary>
    /// GET: Nationalities
    /// </summary>
    /// <returns></returns>
    /// GET: Nationalities
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetNationalitiesWithCountries());
    }


    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetNationalitiesWithCountries());
    }


    // GET: Nationalities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var nationality =
            await _nationalityRepository.GetNationalityAsync(id.Value);

        if (nationality == null) return NotFound();

        return View(nationality);
    }

    // GET: Nationalities/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Nationalities/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Nationality nationality)
    {
        if (!ModelState.IsValid) return View(nationality);

        await _nationalityRepository.AddNationalityAsync(nationality);

        await _nationalityRepository.SaveAllAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Nationalities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var nationality = await
            _nationalityRepository.GetNationalityAsync(id.Value);

        if (nationality == null) return NotFound();

        return View(nationality);
    }

    // POST: Nationalities/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Nationality nationality)
    {
        if (id != nationality.Id) return NotFound();

        if (!ModelState.IsValid) return View(nationality);

        try
        {
            await _nationalityRepository.UpdateNationalityAsync(nationality);
            await _nationalityRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var test = await _nationalityRepository
                .GetNationalityAsync(nationality.Id);

            if (test == null) return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Nationalities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var nationality = await _nationalityRepository
            .GetNationalityAsync(id.Value);

        if (nationality == null) return NotFound();

        return View(nationality);
    }

    // POST: Nationalities/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var nationality =
            await _nationalityRepository.GetNationalityAsync(id);

        if (nationality != null)
            await _nationalityRepository.DeleteNationalityAsync(nationality);

        await _nationalityRepository.SaveAllAsync();

        return RedirectToAction(nameof(Index));
    }
}
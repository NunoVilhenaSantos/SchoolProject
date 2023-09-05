using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     Controller for the Nationalities entity.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class NationalitiesController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly INationalityRepository _nationalityRepository;


    /// <summary>
    ///     Constructor for the NationalitiesController.
    /// </summary>
    /// <param name="countryRepository"></param>
    /// <param name="nationalityRepository"></param>
    public NationalitiesController(
        ICountryRepository countryRepository,
        INationalityRepository nationalityRepository
    )
    {
        _countryRepository = countryRepository;
        _nationalityRepository = nationalityRepository;
    }


    /// <summary>
    ///     GET: Nationalities
    /// </summary>
    /// <returns></returns>
    /// GET: Nationalities
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetNationalitiesWithCountries());
    }


    private List<Nationality> GetNationalitiesWithCountries()
    {
        return _nationalityRepository.GetNationalitiesWithCountries().ToList();
    }


    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetNationalitiesWithCountries());
    }


    // GET: Countries
    // /// <summary>
    // /// Index action for testing the pagination
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records =
    //         GetNationalitiesWithCountries(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<Nationality>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _nationalityRepository.GetCount().Result,
    //     };
    //
    //     return View(model);
    // }


    // GET: Countries
    /// <summary>
    ///     IndexCards1 method for the cards view with pagination mode.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = "FirstName")
    {
        // TODO: Fix the sort order
        var model = new PaginationViewModel<Nationality>(
            GetNationalitiesWithCountries(),
            pageNumber, pageSize,
            _nationalityRepository.GetCount().Result,
            sortOrder, sortProperty
        );


        return View(model);
    }


    // GET: Nationalities/Details/5
    /// <summary>
    ///     details action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var nationality =
            await _nationalityRepository.GetNationalityAsync(id.Value);

        if (nationality == null) return NotFound();

        return View(nationality);
    }

    // GET: Nationalities/Create
    /// <summary>
    ///     create action
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        return View();
    }

    // POST: Nationalities/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     create action
    /// </summary>
    /// <param name="nationality"></param>
    /// <returns></returns>
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
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="nationality"></param>
    /// <returns></returns>
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
    /// <summary>
    ///     delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var nationality = await _nationalityRepository
            .GetNationalityAsync(id.Value);

        if (nationality == null) return NotFound();

        return View(nationality);
    }

    // POST: Nationalities/Delete/5
    /// <summary>
    ///     delete action confirmation
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
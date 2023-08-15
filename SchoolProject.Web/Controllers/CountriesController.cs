using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Models.Countries;

namespace SchoolProject.Web.Controllers;

[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CountriesController : Controller
{
    private readonly ICountryRepository _countryRepository;


    public CountriesController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }


    // ------------------------------ --------- ----------------------------- //
    // ------------------------------ Countries ----------------------------- //
    // ------------------------------ --------- ----------------------------- //


    // GET: Countries
    [HttpGet]
    public IActionResult Index()
    {
        var countriesWithCities =
            _countryRepository?.GetCountriesWithCities();

        if (countriesWithCities != null) return View(countriesWithCities);

        var problemDetails = new ProblemDetails
        {
            Title = "Data Error",
            Detail = "Entity set 'DataContextMySql.Countries' is null.",
            Status = StatusCodes.Status500InternalServerError
            // You can add more properties to the ProblemDetails if needed
        };

        return StatusCode(
            StatusCodes.Status500InternalServerError, problemDetails);


        // return _countryRepository?.GetCountriesWithCities() != null
        //     ? View(_countryRepository?.GetCountriesWithCities())
        //     : Problem("Entity set 'DataContextMySql.Countries'  is null.");
    }


    // GET: Countries/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var country =
            await _countryRepository.GetCountryWithCitiesAsync(id.Value);

        if (country == null) return NotFound();

        return View(country);
    }


    // GET: Countries/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    // POST: Countries/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Country country)
    {
        if (!ModelState.IsValid) return View(country);

        await _countryRepository.CreateAsync(country);

        return RedirectToAction(nameof(Index));
    }


    // GET: Countries/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var country = await _countryRepository.GetByIdAsync(id.Value);

        if (country == null) return NotFound();

        return View(country);
    }


    // POST: Countries/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Country country)
    {
        if (!ModelState.IsValid) return View(country);

        await _countryRepository.UpdateAsync(country);

        return RedirectToAction(nameof(Index));
    }


    // POST: Countries/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Country country)
    {
        if (id != country.Id) return NotFound();

        if (!ModelState.IsValid) return View(country);

        try
        {
            _countryRepository.UpdateAsync(country);
            await _countryRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CountryExists(country.Id)) return NotFound();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    // GET: Countries/Delete/5
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var country = await _countryRepository.GetByIdAsync(id.Value);

        if (country == null) return NotFound();

        await _countryRepository.DeleteAsync(country);

        return RedirectToAction(nameof(Index));
    }


    // POST: Countries/Delete/5
    // [HttpPost, ActionName("Delete")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteConfirmed(int id)
    // {
    //     if (_countryRepository?.GetAll() == null)
    //         return Problem(
    //             "Entity set 'DataContextMySql.Countries' is null.");
    //
    //     var country = await _countryRepository.GetByIdAsync(id);
    //
    //     if (country != null) await _countryRepository.DeleteAsync(country);
    //
    //     await _countryRepository.SaveAllAsync();
    //
    //     return RedirectToAction(nameof(Index));
    // }


    // ------------------------------- ------ ------------------------------- //
    // ------------------------------- Cities ------------------------------- //
    // ------------------------------- ------ ------------------------------- //


    // GET: Countries/AddCity/5
    [HttpGet]
    public async Task<IActionResult> AddCity(
        int? id, int countryId, string countryName, int method)
    {
        if (id == null) return NotFound();

        var country = await _countryRepository.GetByIdAsync(id.Value);

        if (country == null) return NotFound();


        CityViewModel model;
        switch (method)
        {
            case 1:
                // Passe as informações do país para a vista
                model = new CityViewModel {CountryId = country.Id};
                break;

            case 2:
                // Passe as informações do país para a vista
                model = new CityViewModel
                {
                    CountryId = country.Id,
                    CountryName = country.Name
                };
                break;

            default:
                // algo deu errado
                return NotFound();
        }


        return View(model);
    }


    // POST: Countries/AddCity
    [HttpPost]
    public async Task<IActionResult> AddCity(CityViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _countryRepository.AddCityAsync(model);

        return RedirectToAction(
            nameof(Details), new {id = model.CountryId});
    }


    // GET: Countries/DeleteCity/5
    [HttpGet]
    public async Task<IActionResult> DeleteCity(int? id)
    {
        if (id == null) return NotFound();

        var city = await _countryRepository.GetCityAsync(id.Value);

        if (city == null) return NotFound();

        var countryId = await _countryRepository.DeleteCityAsync(city);

        return RedirectToAction(nameof(Details), new {id = countryId});
    }


    // GET: Countries/EditCity/5
    [HttpGet]
    public async Task<IActionResult> EditCity(
        int? id, int countryId, string countryName)
    {
        if (id == null) return NotFound();

        var city = await _countryRepository.GetCityAsync(id.Value);

        if (city == null) return NotFound();

        ViewData["CountryId"] = countryId;
        ViewData["CountryName"] = countryName;

        return View(city);
    }


    // POST: Countries/EditCity/5
    [HttpPost]
    public async Task<IActionResult> EditCity(City city)
    {
        if (!ModelState.IsValid) return View(city);

        var countryId = await _countryRepository.UpdateCityAsync(city);

        if (countryId != 0)
            return RedirectToAction(
                nameof(Details), new {id = countryId});

        return View(city);
    }


    private bool CountryExists(int id)
    {
        return _countryRepository.GetCountryAsync(id).Result != null;
    }
}
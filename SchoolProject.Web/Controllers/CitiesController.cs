using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;

namespace SchoolProject.Web.Controllers;

[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CitiesController : Controller
{
    private readonly ICityRepository _cityRepository;
    // private readonly ICountryRepository _countryRepository;

    private const string BucketName = "cities";


    public CitiesController(
        ICityRepository cityRepository,
        ICountryRepository countryRepository
    )
    {
        _cityRepository = cityRepository;
        // _countryRepository = countryRepository;
    }


    // GET: Cities
    public IActionResult Index()
    {
        var citiesWithCountries =
            _cityRepository?.GetCitiesWithCountriesAsync();

        if (citiesWithCountries != null) return View(citiesWithCountries);

        var problemDetails = new ProblemDetails
        {
            Title = "Data Error",
            Detail = "Entity set 'DataContextMySql.Countries' is null.",
            Status = StatusCodes.Status500InternalServerError
            // You can add more properties to the ProblemDetails if needed
        };

        return StatusCode(
            StatusCodes.Status500InternalServerError, problemDetails);
    }

    public IActionResult IndexCards()
    {
        var citiesWithCountries =
            _cityRepository?.GetCitiesWithCountriesAsync();

        if (citiesWithCountries != null) return View(citiesWithCountries);

        var problemDetails = new ProblemDetails
        {
            Title = "Data Error",
            Detail = "Entity set 'DataContextMySql.Countries' is null.",
            Status = StatusCodes.Status500InternalServerError
            // You can add more properties to the ProblemDetails if needed
        };

        return StatusCode(
            StatusCodes.Status500InternalServerError, problemDetails);
    }


    // GET: Cities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var city = await _cityRepository.GetCityAsync(id.Value);

        if (city == null) return NotFound();

        return View(city);
    }

    // GET: Cities/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Cities/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(City city)
    {
        if (!ModelState.IsValid) return View(city);

        await _cityRepository.AddCityAsync(city);

        await _cityRepository.SaveAllAsync();

        return RedirectToAction(nameof(Index));
    }

    // GET: Cities/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var city = await _cityRepository.GetCityAsync(id.Value);

        if (city == null) return NotFound();

        return View(city);
    }

    // POST: Cities/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, City city)
    {
        if (id != city.Id) return NotFound();

        if (!ModelState.IsValid) return View(city);

        try
        {
            await _cityRepository.UpdateCityAsync(city);
            await _cityRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            var test = await _cityRepository.GetCityAsync(city.Id);

            if (test == null) return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Cities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var city = await _cityRepository.GetCityAsync(id.Value);

        if (city == null) return NotFound();

        return View(city);
    }


    // POST: Cities/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var city = await _cityRepository.GetCityAsync(id);

        if (city != null)
        {
            try
            {
                await _cityRepository.DeleteCityAsync(city);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);

                if (ex.InnerException == null ||
                    !ex.InnerException.Message.Contains("DELETE"))
                    return View("Error");


                TempData["DbUpdateException"] = true;

                TempData["ModalErrorTitle"] =
                    "Está a ser usada como chave estrangeira!!";

                TempData["ModalErrorMessage"] =
                    $"{city.Name} não pode ser apagado visto ter relações " +
                    "com outras tabelas e que se encontra em uso.</br></br>";
                // "Experimente primeiro apagar todas as encomendas " +
                // "que o estão a usar, e torne novamente a apagá-lo";


                TempData["DbUpdateException"] = ex.Message;
                TempData["DbUpdateInnerException"] = ex.InnerException;
                TempData["DbUpdateInnerExceptionMessage"] =
                    ex.InnerException.Message;


                return RedirectToAction(
                    nameof(Delete),
                    new {id = city.Id, showErrorModal = true});

                // return RedirectToAction(nameof(Delete));
                // return View("Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (ex.InnerException == null ||
                    !ex.InnerException.Message.Contains("DELETE"))
                    return View("Error");

                TempData["ErrorTitle"] =
                    $"{city.Name} provavelmente está a ser usado!!";

                TempData["ErrorMessage"] =
                    $"{city.Name} não pode ser apagado visto " +
                    $"haverem encomendas que o usam.</br></br>" +
                    $"Experimente primeiro apagar todas as encomendas " +
                    $"que o estão a usar, e torne novamente a apagá-lo";

                return View("Error");
            }
        }

        await _cityRepository.SaveAllAsync();

        return RedirectToAction(nameof(Index));
    }
}
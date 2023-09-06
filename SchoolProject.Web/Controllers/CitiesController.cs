using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     cities controller, only the admins,
///     superusers and the functionaries can access this controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CitiesController : Controller
{
    internal const string SessionVarName = "AllCitiesWithCountries";
    private const string BucketName = "cities";
    private const string SortProperty = "Name";

    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ICountryRepository _countryRepository;
    private readonly ICityRepository _cityRepository;


    /// <summary>
    ///     constructor
    /// </summary>
    /// <param name="cityRepository"></param>
    /// <param name="countryRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public CitiesController(
        ICountryRepository countryRepository,
        ICityRepository cityRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _cityRepository = cityRepository;
        _hostingEnvironment = hostingEnvironment;
        _countryRepository = countryRepository;
    }


    private List<City> CitiesWithCountries()
    {
        var citiesWithCountries =
            _cityRepository.GetCitiesWithCountriesAsync();

        return citiesWithCountries.ToList();
    }


    private List<City> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<City> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery = JsonConvert.DeserializeObject<List<City>>(json) ??
                           new List<City>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = CitiesWithCountries();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<City>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Cities
    /// <summary>
    ///     index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        var recordsQuery = SessionData<City>();
        return View(recordsQuery);
    }


    // GET: Cities
    /// <summary>
    ///     index action with cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        var recordsQuery = SessionData<City>();
        return View(recordsQuery);
    }


    // GET: Cities
    /// <summary>
    ///     IndexCards1 method for the cards view with pagination mode.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery = SessionData<City>();

        var model = new PaginationViewModel<City>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Cities/Details/5
    /// <summary>
    ///     details action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var city = await _cityRepository.GetCityAsync(id.Value);

        if (city == null) return NotFound();

        return View(city);
    }

    // GET: Cities/Create
    /// <summary>
    ///     create action
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    ///     create action
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
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
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="city"></param>
    /// <returns></returns>
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
    /// <summary>
    ///     delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

        await _cityRepository.SaveAllAsync();

        return RedirectToAction(nameof(Index));
    }
}
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Countries;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     countries controller, only the admins, superusers and the functionaries
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CountriesController : Controller
{
    
    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(Country);
    private const string CurrentAction = nameof(Index);

    internal string BucketName = CurrentClass.ToLower();
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "Name";


    // Obtém o controlador atual
    private string CurrentController
    {
        get
        {
            // Obtém o nome do controlador atual e remove "Controller" do nome
            var controllerTypeInfo =
                ControllerContext.ActionDescriptor.ControllerTypeInfo;
            return controllerTypeInfo.Name.Replace("Controller", "");
        }
    }

    private readonly ICountryRepository _countryRepository;
    private readonly IWebHostEnvironment _hostingEnvironment;


    /// <summary>
    ///     constructor
    /// </summary>
    /// <param name="countryRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public CountriesController(
        ICountryRepository countryRepository,
        IWebHostEnvironment hostingEnvironment)
    {
        _countryRepository = countryRepository;
        _hostingEnvironment = hostingEnvironment;
    }


    // ------------------------------ --------- ----------------------------- //
    // ------------------------------ Countries ----------------------------- //
    // ------------------------------ --------- ----------------------------- //


    private List<Country> CountriesWithCities()
    {
        return _countryRepository.GetCountriesWithCities().ToList();
    }


    private List<Country> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Country> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery = JsonConvert.DeserializeObject<List<Country>>(json) ??
                           new List<Country>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = CountriesWithCities();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Country>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Countries
    /// <summary>
    ///     index action
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Country>();
        return View(recordsQuery);
    }


    // GET: Countries
    /// <summary>
    ///     index action with cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Country>();
        return View(recordsQuery);
    }


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
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10

        var recordsQuery = SessionData<Country>();

        var model = new PaginationViewModel<Country>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Countries/Details/5
    /// <summary>
    ///     details action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var country = await
            _countryRepository.GetCountryWithCitiesAsync(id.Value);

        return country == null
            ? new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(IndexCards))
            : View(country);
    }


    // GET: Countries/Create
    /// <summary>
    ///     create action
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create() => View();


    // POST: Countries/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    //
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     create action
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Country country)
    {
        if (!ModelState.IsValid) return View(country);

        await _countryRepository.CreateAsync(country);

        return RedirectToAction(nameof(Index));
    }


    // GET: Countries/Edit/5
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var country = await _countryRepository.GetByIdAsync(id.Value);

        if (country == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        return View(country);
    }


    // POST: Countries/Edit/5
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="country"></param>
    /// <returns></returns>
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
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="country"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Country country)
    {
        if (id != country.Id)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(country);

        try
        {
            await _countryRepository.UpdateAsync(country);
            await _countryRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _countryRepository.ExistAsync(country.Id))
                return new NotFoundViewResult(
                    nameof(CountryNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }

        return RedirectToAction(nameof(Index));
    }


    // GET: Countries/Delete/5
    /// <summary>
    ///     delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var country = await _countryRepository.GetByIdAsync(id.Value);

        return country == null
            ? new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(country);
    }


    // POST: Countries/Delete/5
    /// <summary>
    ///
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var country = await _countryRepository.GetByIdAsync(id);

        if (country == null)
            return new NotFoundViewResult(
                nameof(CountryNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            //throw new Exception("Excepção de Teste");
            await _countryRepository.DeleteAsync(country);

            await _countryRepository.SaveAllAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            // Handle DbUpdateException, specifically for this controller.
            Console.WriteLine(ex.Message);

            // Handle foreign key constraint violation.
            DbErrorViewModel dbErrorViewModel;

            if (ex.InnerException != null &&
                ex.InnerException.Message.Contains("DELETE"))
            {
                dbErrorViewModel = new DbErrorViewModel
                {
                    DbUpdateException = true,
                    ErrorTitle = "Foreign Key Constraint Violation",
                    ErrorMessage =
                        "This entity is being used as a foreign key elsewhere." +
                        $"The {nameof(Country)} with the ID " +
                        $"{country.Id} - {country.Name}." +
                        $"{country.IdGuid} não pode ser apagado visto haverem" +
                        " dependencies que o usam.</br></br>" +
                        "Experimente apagar primeiro todas as encomendas " +
                        "que o estão a usar," +
                        "e torne novamente a apagá-lo",
                    ItemClass = nameof(Country),
                    ItemId = country.Id,
                    ItemGuid = country.IdGuid,
                    ItemName = country.Name,
                };

                // Redirecione para o DatabaseError com os dados apropriados
                return RedirectToAction(
                    "DatabaseError", "Errors", dbErrorViewModel);
            }

            // Handle other DbUpdateExceptions.
            dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Database Error",
                ErrorMessage = "An error occurred while deleting the entity.",
                ItemClass = nameof(Country),
                ItemId = country.Id,
                ItemGuid = country.IdGuid,
                ItemName = country.Name,
            };


            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    // ------------------------------- ------ ------------------------------- //
    // ------------------------------- Cities ------------------------------- //
    // ------------------------------- ------ ------------------------------- //


    // GET: Countries/AddCity/5
    /// <summary>
    ///     add city action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="countryId"></param>
    /// <param name="countryName"></param>
    /// <param name="method"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> AddCity(
        int? id, int countryId, string countryName, int method)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var country = await _countryRepository.GetByIdAsync(id.Value);

        if (country == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        CityViewModel model;
        switch (method)
        {
            case 1:
                // Passe as informações do país para a vista
                model = new CityViewModel
                {
                    CountryId = country.Id,
                    CityId = 0,
                    Name = string.Empty
                };
                break;

            case 2:
                // Passe as informações do país para a vista
                model = new CityViewModel
                {
                    CountryId = country.Id,
                    CountryName = country.Name,
                    CityId = 0,
                    Name = string.Empty
                };
                break;

            default:
                // algo deu errado
                return new NotFoundViewResult(
                    nameof(CountryNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));
        }


        return View(model);
    }


    // POST: Countries/AddCity
    /// <summary>
    ///     add city action
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> AddCity(CityViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _countryRepository.AddCityAsync(model);

        return RedirectToAction(
            nameof(Details), new {id = model.CountryId});
    }


    // GET: Countries/DeleteCity/5
    /// <summary>
    ///     delete city action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> DeleteCity(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var city = await _countryRepository.GetCityAsync(id.Value);

        if (city == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var countryId = await _countryRepository.DeleteCityAsync(city);

        return RedirectToAction(nameof(Details), new {id = countryId});
    }


    // GET: Countries/EditCity/5
    /// <summary>
    ///     edit city action
    /// </summary>
    /// <param name="id"></param>
    /// <param name="countryId"></param>
    /// <param name="countryName"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> EditCity(
        int? id, int countryId, string countryName)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var city = await _countryRepository.GetCityAsync(id.Value);

        if (city == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        ViewData["CountryId"] = countryId;
        ViewData["CountryName"] = countryName;

        return View(city);
    }


    // POST: Countries/EditCity/5
    /// <summary>
    ///     edit city action
    /// </summary>
    /// <param name="city"></param>
    /// <returns></returns>
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


    // ------------------------------- ------ ------------------------------- //


    /// <summary>
    /// CountryFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CountryNotFound => View();


    /// <summary>
    /// CityNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CityNotFound => View();
}
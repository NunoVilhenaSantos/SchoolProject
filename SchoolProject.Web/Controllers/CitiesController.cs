using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     cities controller, only the admins,
///     superusers and the functionaries can access this controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class CitiesController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(City.Name);
    internal const string CurrentClass = nameof(City);

    internal const string CurrentAction = nameof(Index);

    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;
    private readonly ICityRepository _cityRepository;


    // Helpers
    private readonly IConverterHelper _converterHelper;


    // Repositories.
    private readonly ICountryRepository _countryRepository;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;


    /// <summary>
    ///     constructor
    /// </summary>
    /// <param name="cityRepository"></param>
    /// <param name="countryRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="storageHelper"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="converterHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    public CitiesController(
        ICountryRepository countryRepository,
        ICityRepository cityRepository,
        IWebHostEnvironment hostingEnvironment,
        IStorageHelper storageHelper,
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IUserHelper userHelper,
        IMailHelper mailHelper)
    {
        _storageHelper = storageHelper;
        _cityRepository = cityRepository;
        _countryRepository = countryRepository;
        _hostingEnvironment = hostingEnvironment;
        _authenticatedUserInApp = authenticatedUserInApp;
        _converterHelper = converterHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
    }

    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(CitiesController));


    // Obtém o controlador atual
    internal string CurrentController
    {
        get
        {
            // Obtém o nome do controlador atual e remove "Controller" do nome
            var controllerTypeInfo =
                ControllerContext.ActionDescriptor.ControllerTypeInfo;
            return controllerTypeInfo.Name.Replace("Controller", "");
        }
    }


    private List<City> CitiesWithCountries()
    {
        // return _cityRepository.GetCitiesWithCountriesAsync().ToList();

        // return _cityRepository.GetCitiesWithCountriesAsync().AsNoTracking().ToList();

        return _cityRepository.GetCitiesWithCountriesAsync()
            .AsNoTrackingWithIdentityResolution().ToList();

        // return _cityRepository.GetAll().AsNoTracking().ToList();
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

            return JsonConvert.DeserializeObject<List<City>>(json) ??
                   new List<City>();
        }

        // Caso contrário, obtenha a lista completa do banco de dados
        // Chame a função GetTeachersList com o tipo T
        recordsQuery = CitiesWithCountries();

        PaginationViewModel<T>.Initialize(_hostingEnvironment);

        var json1 = PaginationViewModel<City>
            .StoreListToFileInJson(recordsQuery);

        // Armazene a lista na sessão para uso futuro
        HttpContext.Session.Set(SessionVarName, Encoding.UTF8.GetBytes(json1));

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
    public IActionResult Index(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

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
    public IActionResult IndexCards(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

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
    public IActionResult IndexCards1(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

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
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var city = await _cityRepository.GetCityAsync(id.Value)
            .FirstOrDefaultAsync();

        return city == null
            ? new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(city);
    }

    // GET: Cities/Create
    /// <summary>
    ///     create action
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewBag.CountryId =
            _countryRepository.GetComboCountries();

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
        var country1 = await _countryRepository
            .GetCountryWithCitiesAsync(city.CountryId)
            .FirstOrDefaultAsync();

        if (country1 == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, country1.ToString(),
                CurrentController, nameof(Index));


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = city.ProfilePhotoId;

        if (city.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    city.ImageFile, BucketName);

        city.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        var city1 = new City
        {
            Name = city.Name,
            ProfilePhotoId = city.ProfilePhotoId,
            Country = country1,
            CreatedBy = await _authenticatedUserInApp.GetAuthenticatedUser()
        };


        await _cityRepository.CreateAsync(city1);

        await _cityRepository.SaveAllAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: Cities/Edit/5
    /// <summary>
    ///     edit action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var city = await _cityRepository
            .GetByIdAsync(id.Value).FirstOrDefaultAsync();

        if (city == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

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
        if (id != city.Id)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        // if (!ModelState.IsValid) return View(country);

        var city1 = await _cityRepository
            .GetByIdAsync(id).FirstOrDefaultAsync();

        if (city1 == null) return View(city);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = city.ProfilePhotoId;

        if (city.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    city.ImageFile, BucketName);

        city.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        city1.Name = city.Name;
        city1.ProfilePhotoId = city.ProfilePhotoId;

        try
        {
            await _cityRepository.UpdateAsync(city1);

            await _cityRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _cityRepository.ExistAsync(city.Id))
                return new NotFoundViewResult(
                    nameof(CityNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }
    }

    // GET: Cities/Delete/5
    /// <summary>
    ///     delete action
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var city = await _cityRepository.GetCityAsync(id.Value)
            .FirstOrDefaultAsync();

        return city == null
            ? new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(city);
    }


    // POST: Cities/Delete/5
    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var city = await _cityRepository
            .GetByIdAsync(id).FirstOrDefaultAsync();

        if (city == null)
            return new NotFoundViewResult(
                nameof(CityNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            await _cityRepository.DeleteAsync(city);

            await _cityRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

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
                        "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                        $"The {nameof(City)} with the ID " +
                        $"{city.Id} - {city.Name} {city.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(City),
                    ItemId = city.Id.ToString(),
                    ItemGuid = city.IdGuid,
                    ItemName = city.Name
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
                ItemClass = nameof(City),
                ItemId = city.Id.ToString(),
                ItemGuid = city.IdGuid,
                ItemName = city.Name
            };

            HttpContext.Session.Remove(SessionVarName);

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    /// <summary>
    ///     City Not Found action.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult CityNotFound()
    {
        return View();
    }


    // ---------------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }

    // ---------------------------------------------------------------------- //
}
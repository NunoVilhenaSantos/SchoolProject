using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Controllers.API;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Genders;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Data.Repositories.Genders;
using SchoolProject.Web.Data.Repositories.Teachers;
using SchoolProject.Web.Data.Seeders;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     TeachersController class.
/// </summary>
[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class TeachersController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Teacher.FirstName);
    internal const string CurrentClass = nameof(Teacher);
    internal const string CurrentAction = nameof(Index);

    internal const string ClassRole = CurrentClass;

    // Obtém o tipo da classe atual
    internal static string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IMailHelper _mailHelper;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SelectItensController _selectItensController;


    //  repositories
    private readonly ITeacherRepository _teacherRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IGenderRepository _genderRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICityRepository _cityRepository;
    private readonly IUserHelper _userHelper;


    // data context
    // private readonly DataContextMySql _context;


    /// <summary>
    ///     TeachersController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="teacherRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="converterHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="selectItensController"></param>
    /// <param name="storageHelper"></param>
    /// <param name="genderRepository"></param>
    /// <param name="cityRepository"></param>
    /// <param name="countryRepository"></param>
    /// <param name="userManager"></param>
    public TeachersController(
        DataContextMySql context,
        ITeacherRepository teacherRepository,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        IHttpContextAccessor httpContextAccessor,
        SelectItensController selectItensController,
        ICountryRepository countryRepository,
        IGenderRepository genderRepository,
        ICityRepository cityRepository, UserManager<AppUser> userManager)
    {
        _authenticatedUserInApp = authenticatedUserInApp;
        _httpContextAccessor = httpContextAccessor;
        _hostingEnvironment = hostingEnvironment;
        _teacherRepository = teacherRepository;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
        // _context = context;
        _selectItensController = selectItensController;
        _countryRepository = countryRepository;
        _genderRepository = genderRepository;
        _cityRepository = cityRepository;
        _userManager = userManager;
    }


    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(TeachersController));


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

    /// <summary>
    ///     TeacherNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult TeacherNotFound()
    {
        return View();
    }


    private List<Teacher> GetTeachersList()
    {
        return _teacherRepository.GetTeachers()
            // .Include(t => t.City)
            // .ThenInclude(c => c.Country)
            // .Include(t => t.CountryOfNationality)
            // .Include(t => t.Birthplace)
            // .ThenInclude(c => c.Nationality)
            // .Include(t => t.Gender)
            // .ThenInclude(g => g.CreatedBy)
            // .Include(t => t.AppUser)
            // Se desejar carregar os cursos associados
            // .Include(t => t.TeacherDisciplines)
            // E seus detalhes, se necessário
            // .ThenInclude(tc => tc.Discipline)
            .AsNoTracking()
            .ToList();
    }


    private List<Teacher> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Teacher> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery = JsonConvert.DeserializeObject<List<Teacher>>(json) ??
                           new List<Teacher>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetTeachersList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Teacher>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Teachers
    /// <summary>
    ///     Index method, for the main view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Teacher>();

        return View(recordsQuery);
    }


    // GET: Teachers
    /// <summary>
    ///     IndexCards method for the cards view.
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

        var recordsQuery = SessionData<Teacher>();

        return View(recordsQuery);
    }


    // GET: Teachers
    /// <summary>
    ///     IndexCards1 method for the cards view, for testing purposes.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Teacher>();

        var model = new PaginationViewModel<Teacher>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Teachers/Details/5
    /// <summary>
    ///     Details method, for the details view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var teacher = await _teacherRepository.GetTeacherById(id.Value)
            .FirstOrDefaultAsync(m => m.Id == id);

        return teacher == null
            ? new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(teacher);
    }


    // GET: Teachers/Create
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create()
    {
        FillViewLists();

        var teacher = new Teacher
        {
            FirstName = null,
            LastName = null,
            Address = null,
            PostalCode = null,
            CountryId = 0,
            City = null,
            MobilePhone = null,
            Email = null,
            Active = false,
            Gender = null,
            DateOfBirth = default,
            IdentificationNumber = null,
            IdentificationType = null,
            ExpirationDateIdentificationNumber = default,
            TaxIdentificationNumber = null,
            CountryOfNationality = null,
            Birthplace = null,
            EnrollDate = default,
            AppUser = null,
            ProfilePhotoId = default,
            CreatedBy =
                _authenticatedUserInApp.GetAuthenticatedUser().Result,
        };

        return View(teacher);
    }


    // POST: Teachers/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create method, for adding a new teacher.
    /// </summary>
    /// <param name="teacher"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Teacher teacher)
    {
        //if (!ModelState.IsValid) return View(teacher);

        var user = await _userHelper.GetUserByEmailAsync(teacher.Email);

        if (user != null)
        {
            ViewBag.UserMessage =
                "Email has already been used to register an account.";
            return View(teacher);
        }


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var imageId = teacher.ProfilePhotoId;

        if (teacher.ImageFile is {Length: > 0})
            imageId =
                await _storageHelper.UploadStorageAsync(
                    teacher.ImageFile, BucketName);

        teacher.ProfilePhotoId = imageId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        var city = await _cityRepository.GetCityAsync(teacher.CityId)
            .FirstOrDefaultAsync();

        user = new AppUser
        {
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            PhoneNumber = teacher.MobilePhone,
            ProfilePhotoId = teacher.ProfilePhotoId,
            WasDeleted = teacher.WasDeleted,

            Email = teacher.Email,
            UserName = teacher.Email,
            Address = teacher.Address

            //CityId = city.Id,
            //City = city,
        };

        // _converterHelper.AddUser(
        //     customer.FirstName, customer.LastName,
        //     customer.Address ?? string.Empty,
        //     customer.Email,
        //     customer.CellPhone, "Customer"
        // );

        await _userHelper.AddUserAsync(user, SeedDb.DefaultPassword);
        await _userHelper.AddUserToRoleAsync(user, ClassRole);

        var teacher1 = new Teacher
        {
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            MobilePhone = teacher.MobilePhone,
            Email = teacher.Email,
            Address = teacher.Address,
            Gender = teacher.Gender,
            PostalCode = teacher.PostalCode,
            Active = teacher.Active,
            Birthplace = teacher.Birthplace,
            CreatedBy = teacher.CreatedBy,
            DateOfBirth = teacher.DateOfBirth,
            CountryOfNationality = teacher.CountryOfNationality,
            EnrollDate = teacher.EnrollDate,
            ExpirationDateIdentificationNumber =
                teacher.ExpirationDateIdentificationNumber,
            IdentificationNumber = teacher.IdentificationNumber,
            IdentificationType = teacher.IdentificationType,
            TaxIdentificationNumber = teacher.TaxIdentificationNumber,

            City = city,
            CityId = city.Id,
            CountryId = city.CountryId,

            ProfilePhotoId = teacher.ProfilePhotoId,

            UserId = user.Id,
            AppUser = user
        };


        // ***** Incluído por causa do Email de Confirmação *******
        var myToken =
            await _userHelper.GenerateEmailConfirmationTokenAsync(user);


        var tokenLink = Url.Action(
            "ConfirmEmail", "Account",
            new
            {
                userId = user.Id, token = myToken
            },
            HttpContext.Request.Scheme);


        var response = _mailHelper.SendEmail(user.UserName,
            "Email confirmation",
            "<h2>Email Confirmation<h2>" +
            "To allow the appUser, " +
            "please click in this link:" +
            "</br></br>" +
            $"<a href = \"{tokenLink}\">Confirm Email</a>" +
            "</br></br>" +
            $"<p>Password temporária: {SeedDb.DefaultPassword}</a>");

        //var response = _mailHelper.SendEmail1(model.UserName,
        //    "Email confirmation",
        //    "<h2>Email Confirmation<h2>" +
        //    "To allow the appUser, " +
        //    "please click in this link:" +
        //    "</br></br>" +
        //    $"<a href = \"{tokenLink}\">Confirm Email</a>" +
        //    "</br></br>" +
        //    $"<p>Password temporária: {model.Password}</a>");

        if (response.IsSuccess)
        {
            ViewBag.Message =
                "The instructions to allow you appUser has been sent to email";

            return View(teacher1);
        }


        ModelState.AddModelError(
            string.Empty, "The AppUser couldn't be logged");


        //*****Retirado por causa do Email de Confirmação*********

        // var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

        // Confirma o email para este AppUser
        // await _userHelper.ConfirmEmailAsync(user, token);

        await _teacherRepository.CreateAsync(teacher1);

        await _teacherRepository.SaveAllAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: Teachers/Edit/5
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var teacher = await _teacherRepository.GetTeacherById(id.Value)
            .FirstOrDefaultAsync(t => t.Id == id);

        ViewData["Countries"] =
            _selectItensController.GetCountriesWithNationalitiesJson();

        ViewData["Cities"] =
            _selectItensController.GetCitiesJson(teacher.City.CountryId);

        return teacher == null
            ? new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(teacher);
    }


    // POST: Teachers/Edit/5
    // To protect from over-posting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit method, for editing a teacher.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="teacher"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("FirstName,LastName,Birthplace,...")]
        Teacher teacher)
    {
        if (id != teacher.Id)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        // if (!ModelState.IsValid) return View(teacher);

        var teacher1 = await _teacherRepository
            .GetTeacherById(id).FirstOrDefaultAsync();

        if (teacher1 == null) return View(teacher);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = teacher.ProfilePhotoId;

        if (teacher.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    teacher.ImageFile, BucketName);

        teacher.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        teacher1.FirstName = teacher.FirstName;
        teacher1.LastName = teacher.LastName;
        teacher1.MobilePhone = teacher.MobilePhone;
        teacher1.Email = teacher.Email;
        teacher1.Address = teacher.Address;
        teacher1.Gender = teacher.Gender;
        teacher1.PostalCode = teacher.PostalCode;
        teacher1.Active = teacher.Active;
        teacher1.Birthplace = teacher.Birthplace;
        teacher1.CreatedBy =
            _authenticatedUserInApp.GetAuthenticatedUser().Result;
        teacher1.DateOfBirth = teacher.DateOfBirth;
        teacher1.CountryOfNationality = teacher.CountryOfNationality;
        teacher1.EnrollDate = teacher.EnrollDate;
        teacher1.ExpirationDateIdentificationNumber =
            teacher.ExpirationDateIdentificationNumber;
        teacher1.IdentificationNumber = teacher.IdentificationNumber;
        teacher1.IdentificationType = teacher.IdentificationType;
        teacher1.TaxIdentificationNumber = teacher.TaxIdentificationNumber;

        teacher1.City = await _cityRepository.GetCityAsync(teacher.CityId)
            .FirstOrDefaultAsync();
        // teacher1.CityId = teacher.CityId;
        teacher1.CountryId = teacher.CountryId;

        //teacher1.City = city;
        //teacher1.CityId = city.Id;
        //teacher1.CountryId = city.CountryId;

        teacher1.ProfilePhotoId = teacher.ProfilePhotoId;

        //teacher1.UserId = user.Id;
        //teacher1.AppUser = user;


        // teacher1.AppUser = await _userHelper.GetUserByIdAsync(teacher.AppUserId);
        // teacher1.AppUserId = teacher.AppUserId;
        //teacher1.ImageId = teacher.ImageId;

        try
        {
            await _teacherRepository.UpdateAsync(teacher1);

            await _teacherRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _teacherRepository.ExistAsync(teacher.Id))
                return new NotFoundViewResult(
                    nameof(TeacherNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }

        //if (id != teacher.Id)
        //    return new NotFoundViewResult(
        //        nameof(TeacherNotFound), CurrentClass, id.ToString(),
        //        CurrentController, nameof(Index));

        //if (!ModelState.IsValid) return View(teacher);

        //try
        //{
        //    await _teacherRepository.UpdateAsync(teacher);
        //    await _teacherRepository.SaveAllAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!TeacherExists(teacher.Id))
        //        return new NotFoundViewResult(
        //            nameof(TeacherNotFound), CurrentClass, id.ToString(),
        //            CurrentController, nameof(Index));

        //    throw;
        //}

        //HttpContext.Session.Remove(SessionVarName);

        //return RedirectToAction(nameof(Index));
    }


    // GET: Teachers/Delete/5
    /// <summary>
    ///     Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var teacher = await _teacherRepository.GetTeacherById(id.Value)
            .FirstOrDefaultAsync(m => m.Id == id);

        return teacher == null
            ? new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(teacher);
    }


    // POST: Teachers/Delete/5
    /// <summary>
    ///     DeleteConfirmed method, for deleting a teacher.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id)
            .FirstOrDefaultAsync();

        if (teacher == null)
            return new NotFoundViewResult(
                nameof(TeacherNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        try
        {
            await _teacherRepository.DeleteAsync(teacher);

            await _teacherRepository.SaveAllAsync();

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
                        $"The {nameof(Student)} with the ID " +
                        $"{teacher.Id} - {teacher.FullName} {teacher.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Student),
                    ItemId = teacher.Id.ToString(),
                    ItemGuid = teacher.IdGuid,
                    ItemName = teacher.FullName
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
                ItemClass = nameof(Student),
                ItemId = teacher.Id.ToString(),
                ItemGuid = teacher.IdGuid,
                ItemName = teacher.FullName
            };

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    private bool TeacherExists(int id)
    {
        return _teacherRepository.ExistAsync(id).Result;
    }


    private void FillViewLists(
        int courseId = 0, int disciplineId = 0,
        int countryOfNationalityId = 0, int birthplaceId = 0,
        string? createdById = null, string? updatedById = null
    )
    {
        ViewData[nameof(Teacher.CountryId)] =
            _countryRepository.GetComboCountries();

        ViewData[nameof(Teacher.CityId)] = _countryRepository.GetComboCities(0);

        // ViewData[nameof(CourseDiscipline.DisciplineId)] =
        //     new SelectList(test,
        //         nameof(Discipline.Id),
        //         $"{nameof(Discipline.Code)}",
        //         disciplineId);

        ViewData[nameof(Teacher.CountryOfNationalityId)] =
            new SelectList(_countryRepository.GetAll().ToList(),
                nameof(Country.Id),
                $"{nameof(Country.Name)} ({nameof(Country.Nationality.Name)})",
                countryOfNationalityId);

        ViewData[nameof(Teacher.BirthplaceId)] =
            new SelectList(_countryRepository.GetAll().ToList(),
                nameof(Country.Id),
                $"{nameof(Country.Name)} ({nameof(Country.Nationality.Name)})",
                birthplaceId);

        ViewData[nameof(Teacher.GenderId)] =
            new SelectList(_genderRepository.GetAll().ToList(),
                nameof(Gender.Id),
                nameof(Gender.Name),
                updatedById);


        ViewData[nameof(Teacher.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                createdById);

        ViewData[nameof(Teacher.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                updatedById);
    }
}
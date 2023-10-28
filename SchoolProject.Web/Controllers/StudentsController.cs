using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Genders;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Countries;
using SchoolProject.Web.Data.Repositories.Genders;
using SchoolProject.Web.Data.Repositories.Students;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using SchoolProject.Web.Models;
using SchoolProject.Web.Models.Errors;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     students controller
/// </summary>
[Authorize(Roles = "SuperUser,Functionary")]
public class StudentsController : Controller
{
    // Obtém o tipo da classe atual
    internal static string BucketName = CurrentClass.ToLower();
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Student.FullName);
    internal const string CurrentClass = nameof(Student);
    internal const string CurrentAction = nameof(Index);

    // Obtém o nome do controlador atual
    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(StudentsController));


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;
    private readonly IMailHelper _mailHelper;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IHttpContextAccessor _httpContextAccessor;


    //  repositories
    private readonly IStudentRepository _studentRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IGenderRepository _genderRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly ICityRepository _cityRepository;

    // data context
    // private readonly DataContextMySql _context;


    /// <summary>
    ///     students controller constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="studentRepository"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="converterHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="genderRepository"></param>
    /// <param name="cityRepository"></param>
    /// <param name="countryRepository"></param>
    /// <param name="userManager"></param>
    public StudentsController(
        DataContextMySql context,
        IStudentRepository studentRepository,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp,
        IConverterHelper converterHelper, IStorageHelper storageHelper,
        IUserHelper userHelper, IMailHelper mailHelper,
        IHttpContextAccessor httpContextAccessor,
        ICountryRepository countryRepository, IGenderRepository genderRepository,
        ICityRepository cityRepository, UserManager<AppUser> userManager)
    {
        // _context = context;
        _mailHelper = mailHelper;
        _userHelper = userHelper;
        _storageHelper = storageHelper;
        _converterHelper = converterHelper;
        _studentRepository = studentRepository;
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
        _countryRepository = countryRepository;
        _genderRepository = genderRepository;
        _cityRepository = cityRepository;
        _userManager = userManager;
        _authenticatedUserInApp = authenticatedUserInApp;
    }


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
    ///     StudentNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult StudentNotFound()
    {
        return View();
    }


    private List<Student> GetStudentsList()
    {
        return _studentRepository.GetStudents().ToList();

        // return _studentRepository.GetAll()
        //     .Include(s => s.City)
        //     .ThenInclude(c => c.CreatedBy)
        //     .Include(s => s.City)
        //     .ThenInclude(c => c.CreatedBy)
        //     .Include(s => s.CountryOfNationality)
        //     .ThenInclude(c => c.Nationality)
        //     .Include(s => s.CountryOfNationality)
        //     .ThenInclude(c => c.CreatedBy)
        //     .Include(s => s.Birthplace)
        //     .ThenInclude(c => c.Nationality)
        //     .Include(s => s.Birthplace)
        //     .ThenInclude(c => c.CreatedBy)
        //     .Include(s => s.Gender)
        //     .ThenInclude(g => g.CreatedBy)
        //     .Include(s => s.AppUser).ToList();

        // Se desejar carregar as turmas associadas
        // .Include(s => s.CourseStudents)
        // .ThenInclude(scs => scs.Discipline)
        // .ThenInclude(sc => sc.Disciplines)

        // Se desejar carregar os cursos associados
        // E seus detalhes, se necessário
        // .Include(t => t.StudentDisciplines)
        // .ThenInclude(tc => tc.Discipline)
        // .ToList();
    }


    private List<Student> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Student> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery = JsonConvert.DeserializeObject<List<Student>>(json) ??
                           new List<Student>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetStudentsList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Student>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Students
    /// <summary>
    ///     students index
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

        var recordsQuery = SessionData<Student>();

        return View(recordsQuery);
    }


    // GET: Students
    /// <summary>
    ///     students index
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

        var recordsQuery = SessionData<Student>();

        return View(recordsQuery);
    }


    // GET: Students
    /// <summary>
    ///     Index1 method, for the main view, for testing purposes.
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

        var recordsQuery = SessionData<Student>();

        var model = new PaginationViewModel<Student>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Students/Details/5
    /// <summary>
    ///     students details
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var student = await _studentRepository.GetStudentById(id.Value)
            .FirstOrDefaultAsync(m => m.Id == id);

        return student == null
            ? new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(student);
    }


    // GET: Students/Create
    /// <summary>
    ///     students create
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        return View();
    }


    // POST: Students/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     students create
    /// </summary>
    /// <param name="student"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid) return View(student);

        await _studentRepository.CreateAsync(student);

        await _studentRepository.SaveAllAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: Students/Edit/5
    /// <summary>
    ///     students edit
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var student = await _studentRepository.GetStudentById(id.Value)
            .FirstOrDefaultAsync(m => m.Id == id);

        return student == null
            ? new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index))
            : View(student);
    }


    // POST: Students/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     students edit
    /// </summary>
    /// <param name="id"></param>
    /// <param name="student"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        if (id != student.Id)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        if (!ModelState.IsValid) return View(student);

        try
        {
            await _studentRepository.UpdateAsync(student);

            HttpContext.Session.Remove(SessionVarName);

            await _studentRepository.SaveAllAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(student.Id))
                return new NotFoundViewResult(
                    nameof(StudentNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));
    }


    // GET: Students/Delete/5
    /// <summary>
    ///     students delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        var student = await _studentRepository.GetByIdAsync(id.Value)
            .FirstOrDefaultAsync();

        return student == null
            ? new NotFoundViewResult(nameof(StudentNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(student);

        //**********************

        //if (id == null)
        //    return new NotFoundViewResult(
        //        nameof(StudentNotFound), CurrentClass, id.ToString(),
        //        CurrentController, nameof(Index));

        //var student = await _context.Students
        //    .FirstOrDefaultAsync(m => m.Id == id);

        //return student == null
        //    ? new NotFoundViewResult(
        //        nameof(StudentNotFound), CurrentClass, id.ToString(),
        //        CurrentController, nameof(Index))
        //    : View(student);
    }


    // POST: Students/Delete/5
    /// <summary>
    ///     students delete
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id)
            .FirstOrDefaultAsync();

        if (student == null)
            return new NotFoundViewResult(
                nameof(StudentNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));


        try
        {
            await _studentRepository.DeleteAsync(student);

            await _studentRepository.SaveAllAsync();

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
                        $"{student.Id} - {student.FullName} {student.IdGuid} +" +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Student),
                    ItemId = student.Id.ToString(),
                    ItemGuid = student.IdGuid,
                    ItemName = student.FullName
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
                ItemId = student.Id.ToString(),
                ItemGuid = student.IdGuid,
                ItemName = student.FullName
            };

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    private bool StudentExists(int id)
    {
        return _studentRepository.ExistAsync(id).Result;
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

        ViewData[nameof(Student.CountryOfNationalityId)] =
            new SelectList(_countryRepository.GetAll().ToList(),
                nameof(Country.Id),
                $"{nameof(Country.Name)} ({nameof(Country.Nationality.Name)})",
                countryOfNationalityId);

        ViewData[nameof(Student.BirthplaceId)] =
            new SelectList(_countryRepository.GetAll().ToList(),
                nameof(Country.Id),
                $"{nameof(Country.Name)} ({nameof(Country.Nationality.Name)})",
                birthplaceId);

        ViewData[nameof(Student.GenderId)] =
            new SelectList(_genderRepository.GetAll().ToList(),
                nameof(Gender.Id),
                nameof(Gender.Name),
                updatedById);


        ViewData[nameof(Student.CreatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                createdById);

        ViewData[nameof(Student.UpdatedById)] =
            new SelectList(_userManager.Users,
                nameof(AppUser.Id),
                nameof(AppUser.FirstName),
                updatedById);
    }
}
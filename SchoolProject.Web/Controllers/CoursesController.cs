using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Data.Repositories.Courses;
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
///     Courses Controller
/// </summary>
[Authorize(Roles = "Admin,SuperUser")]
public class CoursesController : Controller
{
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = nameof(Course.Name);
    internal const string CurrentClass = nameof(Course);

    internal const string CurrentAction = nameof(Index);

    // Obtém o tipo da classe atual
    internal static readonly string BucketName = CurrentClass.ToLower();


    // A private field to get the authenticated user in app.
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // Helpers
    private readonly IConverterHelper _converterHelper;


    //  repositories
    private readonly ICourseRepository _courseRepository;


    // Host Environment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IMailHelper _mailHelper;
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;


    /// <summary>
    ///     SchoolClassesController
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    /// <param name="authenticatedUserInApp"></param>
    /// <param name="courseRepository"></param>
    /// <param name="converterHelper"></param>
    /// <param name="storageHelper"></param>
    /// <param name="userHelper"></param>
    /// <param name="mailHelper"></param>
    public CoursesController(
        //DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        AuthenticatedUserInApp authenticatedUserInApp,
        ICourseRepository courseRepository, IConverterHelper converterHelper,
        IStorageHelper storageHelper, IUserHelper userHelper,
        IMailHelper mailHelper)
    {
        //_context = context;
        _hostingEnvironment = hostingEnvironment;
        _authenticatedUserInApp = authenticatedUserInApp;
        _courseRepository = courseRepository;
        _converterHelper = converterHelper;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
        _mailHelper = mailHelper;
    }

    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(CoursesController));


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
    ///     Course Not Found action.
    /// </summary>
    /// <returns></returns>
    public IActionResult CourseNotFound()
    {
        return View();
    }


    private List<Course> GetCoursesList()
    {
        return _courseRepository.GetCourses().AsNoTracking().ToList();
    }


    private List<Course> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Course> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<Course>>(json) ??
                new List<Course>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetCoursesList();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Course>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///     Index
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Courses
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Course>();

        return View(recordsQuery);
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///     Index cards
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Courses
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Course>();

        return View(recordsQuery);
    }


    // Allow unrestricted access to the Index action
    /// <summary>
    ///     Index cards 1 method, for the main view, for testing purposes.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    [AllowAnonymous]
    // GET: Courses
    public IActionResult IndexCards1(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Course>();

        var model = new PaginationViewModel<Course>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Courses/Details/5
    /// <summary>
    ///     Details of a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(CourseNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var course = await _courseRepository
            .GetCourseByIdAsync(id.Value)
            .FirstOrDefaultAsync();

        return course == null
            ? new NotFoundViewResult(nameof(CourseNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(course);
    }


    // GET: Courses/Create
    /// <summary>
    ///     Create a new school class, view.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        var course = new Course
        {
            Code = null,
            Acronym = null,
            Name = null,
            QnqLevel = 1,
            EqfLevel = 1,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddYears(1),
            StartHour = TimeSpan.FromHours(8),
            EndHour = TimeSpan.FromHours(17),
            PriceForEmployed = 100,
            PriceForUnemployed = 0,
            ProfilePhotoId = default,
            CreatedAt = DateTime.Now,
            CreatedBy = null,
        };

        return View(course);
    }


    // POST: Courses/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create a new school class, post.
    ///     validates and saves the new school class.
    /// </summary>
    /// <param name="course"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        //if (!ModelState.IsValid) return View(teacher);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var imageId = course.ProfilePhotoId;

        if (course.ImageFile is {Length: > 0})
            imageId =
                await _storageHelper.UploadStorageAsync(
                    course.ImageFile, BucketName);

        course.ProfilePhotoId = imageId;

        // *** FIM PARA GRAVAR A IMAGEM ***


        var course1 = new Course
        {
            Code = course.Code,
            Acronym = course.Acronym,
            Name = course.Name,
            QnqLevel = course.QnqLevel,
            EqfLevel = course.EqfLevel,
            StartDate = course.StartDate,
            EndDate = course.EndDate,
            StartHour = course.StartHour,
            EndHour = course.EndHour,
            ProfilePhotoId = course.ProfilePhotoId,
            PriceForEmployed = course.PriceForEmployed,
            PriceForUnemployed = course.PriceForUnemployed,
            CreatedBy = _authenticatedUserInApp.GetAuthenticatedUser().Result
        };


        await _courseRepository.CreateAsync(course1);

        await _courseRepository.SaveAllAsync();

        HttpContext.Session.Remove(SessionVarName);

        return RedirectToAction(nameof(Index));

        //if (!ModelState.IsValid) return View(course);

        //await _courseRepository.AddCourseAsync(course);

        //HttpContext.Session.Remove(SessionVarName);

        //return RedirectToAction(nameof(Index));
    }


    // GET: Courses/Edit/5
    /// <summary>
    ///     Edit a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="idGuid"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(CourseNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index));

        var course = await _courseRepository
            .GetCourseByIdAsync(id.Value).FirstOrDefaultAsync();

        return course == null
            ? new NotFoundViewResult(nameof(CourseNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(course);
    }


    // POST: Courses/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit a school class, post.
    ///     validate and save the edited school class.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="course"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course)
    {
        if (id != course.Id)
            return new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        // if (!ModelState.IsValid) return View(course);

        var course1 = await _courseRepository
            .GetCourseByIdAsync(id).FirstOrDefaultAsync();

        if (course1 == null) return View(course);


        // *** INICIO PARA GRAVAR A IMAGEM ***

        var profilePhotoId = course.ProfilePhotoId;

        if (course.ImageFile is {Length: > 0})
            profilePhotoId =
                await _storageHelper.UploadStorageAsync(
                    course.ImageFile, BucketName);

        course.ProfilePhotoId = profilePhotoId;

        // *** FIM PARA GRAVAR A IMAGEM ***

        //****** APENAS ESTES???

        course1.Acronym = course.Acronym;
        course1.WasDeleted = course.WasDeleted;
        course1.CreatedAt = course.CreatedAt;
        course1.ProfilePhotoId = course.ProfilePhotoId;
        course1.CreatedBy =
            _authenticatedUserInApp.GetAuthenticatedUser().Result;


        try
        {
            await _courseRepository.UpdateAsync(course1);

            await _courseRepository.SaveAllAsync();

            HttpContext.Session.Remove(SessionVarName);

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _courseRepository.ExistAsync(course.Id))
                return new NotFoundViewResult(
                    nameof(CourseNotFound), CurrentClass, id.ToString(),
                    CurrentController, nameof(Index));

            throw;
        }

        //********************

        //if (id != course.Id)
        //    return new NotFoundViewResult(nameof(CourseNotFound),
        //        CurrentClass, id.ToString(), CurrentController, nameof(Index));


        //if (!ModelState.IsValid)
        //{
        //    course = await _courseRepository
        //        .GetCourseByIdAsync(id).FirstAsync();

        //    return course == null
        //        ? new NotFoundViewResult(nameof(CourseNotFound), CurrentClass,
        //            id.ToString(), CurrentController, nameof(Index))
        //        : View(course);
        //}


        //try
        //{
        //    await _courseRepository.UpdateAsync(course);

        //    await _courseRepository.SaveAllAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!await SchoolClassExists(course.Id))
        //        return new NotFoundViewResult(nameof(CourseNotFound),
        //            CurrentClass, id.ToString(), CurrentController,
        //            nameof(Index));

        //    throw;
        //}

        //HttpContext.Session.Remove(SessionVarName);

        //return RedirectToAction(nameof(Index));
    }


    // GET: Courses/Delete/5
    /// <summary>
    ///     Delete a school class, view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id, Guid? idGuid)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(CourseNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index));

        var course = await _courseRepository
            .GetCourseByIdAsync(id.Value)
            .FirstOrDefaultAsync();

        if (course == null)
            return new NotFoundViewResult(nameof(CourseNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index));

        return View(course);
    }


    // POST: Courses/Delete/5
    /// <summary>
    ///     Delete a school class, post.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _courseRepository
            .GetCourseByIdAsync(id).FirstOrDefaultAsync();

        if (course == null)
            return new NotFoundViewResult(
                nameof(CourseNotFound), CurrentClass, id.ToString(),
                CurrentController, nameof(Index));

        try
        {
            await _courseRepository.DeleteAsync(course);

            await _courseRepository.SaveAllAsync();

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
                        $"The {nameof(Course)} with the ID " +
                        $"{course.Id} - {course.Name} {course.IdGuid} " +
                        "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                        "Try deleting possible dependencies and try again. ",
                    ItemClass = nameof(Course),
                    ItemId = course.Id.ToString(),
                    ItemGuid = course.IdGuid,
                    ItemName = course.Name
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
                ItemClass = nameof(Course),
                ItemId = course.Id.ToString(),
                ItemGuid = course.IdGuid,
                ItemName = course.Name
            };

            HttpContext.Session.Remove(SessionVarName);

            // Redirecione para o DatabaseError com os dados apropriados
            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        catch (InvalidOperationException ex)
        {
            // Handle the exception
            Console.WriteLine("An InvalidOperationException occurred: " +
                              ex.Message);

            var dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Foreign Key Constraint Violation",
                ErrorMessage =
                    "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                    $"The {nameof(Course)} with the ID " +
                    $"{course.Id} - {course.Name} {course.IdGuid} " +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(Course),
                ItemId = course.Id.ToString(),
                ItemGuid = course.IdGuid,
                ItemName = course.Name
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
        catch (Exception ex)
        {
            // Catch any other exceptions that might occur
            Console.WriteLine("An error occurred: " + ex.Message);

            var dbErrorViewModel = new DbErrorViewModel
            {
                DbUpdateException = true,
                ErrorTitle = "Foreign Key Constraint Violation",
                ErrorMessage =
                    "</br></br>This entity is being used as a foreign key elsewhere.</br></br>" +
                    $"The {nameof(Course)} with the ID " +
                    $"{course.Id} - {course.Name} {course.IdGuid} " +
                    "cannot be deleted due to there being dependencies from other entities.</br></br>" +
                    "Try deleting possible dependencies and try again. ",
                ItemClass = nameof(Course),
                ItemId = course.Id.ToString(),
                ItemGuid = course.IdGuid,
                ItemName = course.Name
            };

            return RedirectToAction(
                "DatabaseError", "Errors", dbErrorViewModel);
        }
    }


    private async Task<bool> CourseExists(int id)
    {
        // return _context.Courses.Any(e => e.Id == id);
        return await _courseRepository.ExistAsync(id);
    }
}
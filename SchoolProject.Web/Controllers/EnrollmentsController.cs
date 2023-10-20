using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Repositories.Enrollments;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     EnrollmentsController class.
/// </summary>
//[Authorize(Roles = "Admin,SuperUser,Functionary")]
public class EnrollmentsController : Controller
{
    // Obtém o tipo da classe atual
    private const string CurrentClass = nameof(Enrollment);
    private const string CurrentAction = nameof(Index);
    internal const string SessionVarName = "ListOfAll" + CurrentClass;
    internal const string SortProperty = "Name";


    private readonly DataContextMySql _context;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IWebHostEnvironment _hostingEnvironment;

    internal string BucketName = CurrentClass.ToLower();


    /// <summary>
    ///     EnrollmentsController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="enrollmentRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public EnrollmentsController(
        DataContextMySql context,
        IWebHostEnvironment hostingEnvironment,
        IEnrollmentRepository enrollmentRepository)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
        _enrollmentRepository = enrollmentRepository;
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
    ///     EnrollmentNotFound action.
    /// </summary>
    /// <returns></returns>
    public IActionResult EnrollmentNotFound => View();


    private List<Enrollment> GetEnrollmentsWithCoursesAndStudents()
    {
        //var citiesWithCountries =
        //    _cityRepository?.GetCitiesWithCountriesAsync();

        var enrollmentsWithStudent =
            _context.Enrollments
                .Include(e => e.Discipline)
                .Include(e => e.Student)
                .Include(e => e.CreatedBy)
                .Include(e => e.UpdatedBy).ToList();

        return enrollmentsWithStudent;
    }


    private List<Enrollment> SessionData<T>() where T : class
    {
        // Obtém todos os registos
        List<Enrollment> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue(SessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<Enrollment>>(json) ??
                new List<Enrollment>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = GetEnrollmentsWithCoursesAndStudents();

            PaginationViewModel<T>.Initialize(_hostingEnvironment);

            var json = PaginationViewModel<Enrollment>
                .StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            HttpContext.Session.Set(SessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    // GET: Enrollments
    /// <summary>
    ///     Index method, for the main view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult Index(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Enrollment>();
        return View(recordsQuery);
    }


    // GET: Enrollments
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="sortOrder"></param>
    /// <param name="sortProperty"></param>
    /// <returns></returns>
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = SortProperty)
    {
        // Envia o tipo da classe para a vista
        ViewData["CurrentClass"] = CurrentClass;

        var recordsQuery = SessionData<Enrollment>();
        return View(recordsQuery);
    }


    // GET: Enrollments
    /// <summary>
    ///     IndexCards method for the cards view.
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

        var recordsQuery = SessionData<Enrollment>();

        var model = new PaginationViewModel<Enrollment>(
            recordsQuery,
            pageNumber, pageSize,
            recordsQuery.Count,
            sortOrder, sortProperty
        );

        return View(model);
    }


    // GET: Enrollments/Details/5
    /// <summary>
    ///     Details method.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var enrollment = await _context.Enrollments
            .Include(e => e.Discipline)
            .Include(e => e.Student)
            .Include(e => e.CreatedBy)
            .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        return enrollment == null
            ? new NotFoundViewResult(nameof(EnrollmentNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(enrollment);
    }


    // GET: Enrollments/Create
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code");

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address");

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "Id");

        return View();
    }

    // POST: Enrollments/Create
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Create method, for adding a new enrollment.
    /// </summary>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Enrollment enrollment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(enrollment);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code",
                enrollment.DisciplineId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                enrollment.StudentId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                enrollment.CreatedBy.Id);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                enrollment.UpdatedBy?.Id);

        return View(enrollment);
    }


    // GET: Enrollments/Edit/5
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                "Id", "Code",
                enrollment.DisciplineId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                "Id", "Address",
                enrollment.StudentId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                enrollment.CreatedBy.Id);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                "Id", "FullName",
                enrollment is {UpdatedBy: not null}
                    ? enrollment.UpdatedBy.Id
                    : null);

        return View(enrollment);
    }

    // POST: Enrollments/Edit/5
    // To protect from over-posting attacks,
    // enable the specific properties you want to bind to.
    // For more details,
    // see http://go.microsoft.com/fwlink/?LinkId=317598.
    /// <summary>
    ///     Edit method, for editing a enrollment.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enrollment"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Enrollment enrollment)
    {
        if (id != enrollment.StudentId)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(enrollment);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnrollmentExists(enrollment.StudentId))
                    return new NotFoundViewResult(nameof(EnrollmentNotFound),
                        CurrentClass, id.ToString(), CurrentController,
                        nameof(Index));

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["DisciplineId"] =
            new SelectList(_context.Disciplines,
                nameof(enrollment.Discipline.Id),
                nameof(enrollment.Discipline.Code),
                enrollment.DisciplineId);

        ViewData["StudentId"] =
            new SelectList(_context.Students,
                nameof(enrollment.Student.Id),
                nameof(enrollment.Student.FullName),
                enrollment.StudentId);

        ViewData["CreatedById"] =
            new SelectList(_context.Users,
                nameof(enrollment.CreatedBy.Id),
                nameof(enrollment.CreatedBy.FullName),
                enrollment.CreatedBy.Id);

        ViewData["UpdatedById"] =
            new SelectList(_context.Users,
                nameof(enrollment.UpdatedBy.Id),
                nameof(enrollment.UpdatedBy.FullName),
                enrollment.UpdatedBy?.Id);

        return View(enrollment);
    }

    // GET: Enrollments/Delete/5
    /// <summary>
    ///     Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return new NotFoundViewResult(nameof(EnrollmentNotFound),
                CurrentClass, id.ToString(), CurrentController, nameof(Index));

        var enrollment = await _context.Enrollments
            .Include(e => e.Discipline)
            .Include(e => e.Student)
            .Include(e => e.CreatedBy)
            .Include(e => e.UpdatedBy)
            .FirstOrDefaultAsync(m => m.StudentId == id);

        return enrollment == null
            ? new NotFoundViewResult(nameof(EnrollmentNotFound), CurrentClass,
                id.ToString(), CurrentController, nameof(Index))
            : View(enrollment);
    }


    // POST: Enrollments/Delete/5
    /// <summary>
    ///     DeleteConfirmed method, for deleting a enrollment.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var enrollment = await _context.Enrollments.FindAsync(id);

        if (enrollment != null) _context.Enrollments.Remove(enrollment);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    private bool EnrollmentExists(int id)
    {
        return _context.Enrollments.Any(e => e.StudentId == id);
    }
}
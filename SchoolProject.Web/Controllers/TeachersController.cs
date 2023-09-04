using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Repositories.Teachers;
using SchoolProject.Web.Models;

namespace SchoolProject.Web.Controllers;

/// <summary>
///     TeachersController class.
/// </summary>
public class TeachersController : Controller
{
    private const string BucketName = "teachers";
    private readonly DataContextMySql _context;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ITeacherRepository _teacherRepository;


    /// <summary>
    ///     TeachersController constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="teacherRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public TeachersController(
        DataContextMySql context,
        ITeacherRepository teacherRepository
        , IWebHostEnvironment hostingEnvironment
    )
    {
        _context = context;
        _teacherRepository = teacherRepository;
        _hostingEnvironment = hostingEnvironment;
    }


    // GET: Teachers
    /// <summary>
    ///     Index method, for the main view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeachersList());
    }


    private IEnumerable<Teacher> GetTeachersList()
    {
        return _context.Teachers.ToListAsync().Result;
    }


    private async Task<IEnumerable<Teacher>> GetTeachersListAsync()
    {
        return await _context.Teachers
            .Include(t => t.Country).ThenInclude(c => c.Nationality)
            .Include(t => t.Country).ThenInclude(c => c.CreatedBy)
            .Include(t => t.City).ThenInclude(c => c.CreatedBy)
            .Include(t => t.CountryOfNationality)
            .ThenInclude(c => c.Nationality)
            .Include(t => t.CountryOfNationality).ThenInclude(c => c.CreatedBy)
            .Include(t => t.Birthplace).ThenInclude(c => c.Nationality)
            .Include(t => t.Birthplace).ThenInclude(c => c.CreatedBy)
            .Include(t => t.Gender).ThenInclude(c => c.CreatedBy)
            .Include(t => t.User)
            // Se desejar carregar os cursos associados
            .Include(t => t.TeacherCourses)
            // E seus detalhes, se necessário
            .ThenInclude(tc => tc.Course)
            .ToListAsync();
    }


    // GET: Teachers
    /// <summary>
    ///     IndexCards method for the cards view.
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult IndexCards(int pageNumber = 1, int pageSize = 10)
    {
        return View(GetTeachersList());
    }


    // GET: Teachers
    // /// <summary>
    // /// Index1 method, for the main view, for testing purposes.
    // /// </summary>
    // /// <param name="pageNumber"></param>
    // /// <param name="pageSize"></param>
    // /// <returns></returns>
    // [HttpGet]
    // public IActionResult Index1(int pageNumber = 1, int pageSize = 10)
    // {
    //     var records = GetTeachersListForCards(pageNumber, pageSize);
    //
    //     var model = new PaginationViewModel<Teacher>
    //     {
    //         Records = records,
    //         PageNumber = pageNumber,
    //         PageSize = pageSize,
    //         TotalCount = _context.Teachers.Count(),
    //     };
    //
    //     return View(model);
    // }


    private List<Teacher> GetTeachersList(
        int pageNumber, int pageSize)
    {
        return GetTeachersList()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }


    // TODO: Corrigir todos os erros e passar para métodos dentro do modelo PaginationViewModel
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
    public async Task<IActionResult> IndexCards1Async(
        int pageNumber = 1, int pageSize = 10,
        string sortOrder = "asc", string sortProperty = "FirstName")
    {
        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1
        if (pageSize < 1) pageSize = 10; // Tamanho da página mínimo é 10


        // TODO: Obter todos os registros
        IEnumerable<Teacher> allTeachers;


        // Tente obter a lista de professores da sessão
        if (HttpContext.Session.TryGetValue("AllTeachers", out var teacherData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(teacherData);
            allTeachers = JsonConvert.DeserializeObject<List<Teacher>>(json);
        }
        else
        {
            // Caso contrário, obtenha a lista completa de professores do banco de dados
            allTeachers = await GetTeachersListAsync();


            // Armazene a lista na sessão para uso futuro
            var json = JsonConvert.SerializeObject(allTeachers,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting =
                        Formatting.Indented // Indent the JSON for readability
                });

            // TODO: passar para o modelo PaginationViewModel
            try
            {
                // Specify the file path where you want to save the JSON file
                var filePath = Path.Combine(_hostingEnvironment.ContentRootPath,
                    "Data", "teachers.json");

                // Save the JSON to the file
                System.IO.File.WriteAllText(filePath, json);

                // return Ok("JSON file saved successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }


            Console.WriteLine("JSON file saved successfully!");

            HttpContext.Session.Set("AllTeachers",
                Encoding.UTF8.GetBytes(json));
        }


        // Obter todos os registros
        // var recordsQuery = GetTeachersList().AsQueryable();

        // Aplicar ordenação com base em sortOrder e sortProperty
        // recordsQuery =
        //     ApplySorting(recordsQuery, sortOrder, sortProperty);

        // Obter uma página específica de usuários
        var records = _context.Teachers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var model = new PaginationViewModel<Teacher>(
            records,
            pageNumber, pageSize,
            _context.Teachers.Count(),
            sortOrder, sortProperty
        );

        return View(model);
    }


    private IQueryable<Teacher> ApplySorting(
        IQueryable<Teacher> query, string sortOrder, string sortProperty)
    {
        // Verifica se sortOrder é válido
        var validSortOrders = new[] {"asc", "desc"};


        // Tratar ordenação inválida, por exemplo, aplicar ordenação padrão
        if (!validSortOrders.Contains(sortOrder)) sortOrder = "asc";


        // Tratar a propriedade padrão para ordenação, defina a propriedade padrão aqui
        if (string.IsNullOrEmpty(sortProperty)) sortProperty = "FirstName";


        // Obtém o tipo da classe
        // Type userType = typeof(UserWithRolesViewModel);
        var userType = typeof(Teacher);

        // Verifica se a propriedade de ordenação existe na classe
        var propertyInfo =
            userType.GetProperty(sortProperty,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance) ??
            userType.GetProperty("FirstName",
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);


        // Cria uma expressão de ordenação dinâmica
        var parameter = Expression.Parameter(typeof(Teacher), "x");
        var property = Expression.Property(parameter, propertyInfo);
        var lambda = Expression.Lambda(property, parameter);


        // Aplica a ordenação com base na expressão dinâmica
        var orderByMethod =
            sortOrder == "asc" ? "OrderBy" : "OrderByDescending";

        var orderByExpression = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new[] {userType, propertyInfo.PropertyType},
            query.Expression,
            lambda
        );


        // Retorna o resultado ordenado
        return query.Provider.CreateQuery<Teacher>(orderByExpression);
    }


    // GET: Teachers/Details/5
    /// <summary>
    ///     Details method, for the details view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (teacher == null) return NotFound();

        return View(teacher);
    }


    // GET: Teachers/Create
    /// <summary>
    ///     Create method, for the create view.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
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
        if (!ModelState.IsValid) return View(teacher);

        _context.Add(teacher);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    // GET: Teachers/Edit/5
    /// <summary>
    ///     Edit method, for the edit view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher == null) return NotFound();

        return View(teacher);
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
    public async Task<IActionResult> Edit(int id, Teacher teacher)
    {
        if (id != teacher.Id) return NotFound();

        if (!ModelState.IsValid) return View(teacher);

        try
        {
            _context.Update(teacher);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TeacherExists(teacher.Id))
                return NotFound();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Teachers/Delete/5
    /// <summary>
    ///     Delete method, for the delete view.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .FirstOrDefaultAsync(m => m.Id == id);

        if (teacher == null) return NotFound();

        return View(teacher);
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
        var teacher = await _context.Teachers.FindAsync(id);

        if (teacher != null) _context.Teachers.Remove(teacher);

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool TeacherExists(int id)
    {
        return _context.Teachers.Any(e => e.Id == id);
    }
}
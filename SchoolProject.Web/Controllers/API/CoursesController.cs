using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Repositories.Courses;
using SchoolProject.Web.Helpers.Users;
using JsonSerializer = System.Text.Json.JsonSerializer;


// For more information on enabling Web API for empty projects,
// visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace SchoolProject.Web.Controllers.API;

/// <summary>
///     Courses Controller API Controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [Authorize(Policy = "Bearer")]
//[Authorize(Roles = "Admin")]
public class CoursesController : ControllerBase
{
    private readonly DataContextMySql _context;
    private readonly ICourseRepository _courseRepository;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IUserHelper _userHelper;


    /// <summary>
    ///     Courses Controller API Controller constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userHelper"></param>
    /// <param name="courseRepository"></param>
    /// <param name="hostingEnvironment"></param>
    public CoursesController(
        IUserHelper userHelper,
        DataContextMySql context,
        ICourseRepository courseRepository,
        IWebHostEnvironment hostingEnvironment
    )
    {
        _context = context;
        _userHelper = userHelper;
        _courseRepository = courseRepository;
        _hostingEnvironment = hostingEnvironment;
    }


    // GET: api/<CoursesController>
    /// <summary>
    ///     Get all Courses
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    // [Route("api/Courses/")]
    public IActionResult Get()
    {
        var schoolClasses = _context.Courses

            // School class courses include
            .Include(c => c.CourseDisciplines)
            // .ThenInclude(scc => scc.Course)

            .Include(c => c.CourseDisciplines)
            // .ThenInclude(scc => scc.Discipline)

            // school class students include
            .Include(c => c.CourseStudents)
            // .ThenInclude(scs => scs.Student)
            //.ThenInclude(s => s.Country)
            //.ThenInclude(s => s.Nationality)

            // include students 
            // .Include(c => c.Students)
            // .ThenInclude(s => s.Country)
            // .ThenInclude(s => s.Nationality)
            // .Include(c => c.Students)
            // .ThenInclude(s => s.AppUser)

            // include disciplines
            // .Include(c => c.Disciplines)

            // include enrollments
            .Include(c => c.Enrollments)

            // other includes
            .Include(c => c.CreatedBy)
            .Include(c => c.UpdatedBy)
            .ToList();


        // this return should be used, we need to use System.Text.Json, because of th following error:
        // System.Text.Json.JsonException: A possible object cycle was detected.
        // This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
        // Consider using ReferenceHandler.Preserve on JsonSerializerOptions to support cycles.
        // return Ok(schoolClasses);


        //
        // Parece que você está misturando o uso de System.Text.Json e Newtonsoft.Json no mesmo trecho de código, o que pode causar erros.
        // Vamos corrigir isso para usar apenas System.Text.Json.
        // Aqui está o código corrigido:
        //


        // --------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------- //


        // serialização usando System.Text.Json
        var options = new JsonSerializerOptions
        {
            // Use ReferenceHandler.Preserve para preservar referências circulares
            ReferenceHandler = ReferenceHandler.Preserve,

            // Indent o JSON para melhor legibilidade
            WriteIndented = true,

            // Defina um valor adequado para limitar a profundidade da serialização.
            MaxDepth = 10
        };


        // converte em json para enviar aos pedidos da API
        var jsonMariconsoft =
            JsonSerializer.Serialize(schoolClasses, options);

        return Ok(jsonMariconsoft);


        // --------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------- //

        // Supondo que você obteve os cursos do banco de dados
        var courses =_context.Courses.ToList(); 

        var courseDTOs = courses
            .Select(course => CourseDto.MapToDto(course))
            .ToList();

        var options1 = new System.Text.Json.JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true,
            MaxDepth = 10 // Defina a profundidade máxima, se necessário
        };

        var jsonMariconsoft1 = JsonSerializer.Serialize(courseDTOs, options1);

        return Ok(jsonMariconsoft1);

        // --------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------- //


        // serialização usando Newtonsoft.Json 
        var settings = new JsonSerializerSettings
        {
            // Use PreserveReferencesHandling para preservar referências circulares
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,

            // Indent the JSON for readability
            Formatting = Formatting.Indented
        };

        // converte em json para enviar aos pedidos da API
        var json = JsonConvert.SerializeObject(
            schoolClasses, settings);

        // return Ok(json);


        // --------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------- //


        // https://stackoverflow.com/questions/58113329/asp-net-core-3-0-system-text-json-jsonexception-a-possible-object-cycle-was
        // este processo de serializar e deserializar é necessário para remover as referências circulares
        // mas consome muita memoria até que o sistema rebenta
        // consome 4,7 gb de memoria    
        // Armazene a lista na sessão para uso futuro


        // serialização usando Newtonsoft.Json 
        var json1 = JsonConvert.SerializeObject(schoolClasses,
            new JsonSerializerSettings
            {
                // A possible object cycle was detected.
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // Use PreserveReferencesHandling para preservar referências circulares
                // PreserveReferencesHandling = PreserveReferencesHandling.Objects,

                // Indent the JSON for readability
                Formatting = Formatting.Indented
            });


        return Ok(json1);
    }


    // GET: api/<CoursesController>
    /// <summary>
    ///     Get all school classes with users
    /// </summary>
    /// <returns></returns>
    // [HttpGet]
    // public IActionResult Get() => Ok(_schoolClassRepository.GetAllWithUsers());

    // GET api/<CoursesController>/5
    // [Route("api/Courses/{id:int}")]
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var course = _context.Courses

            .Include(c => c.CourseDisciplines)
            .ThenInclude(scc => scc.Discipline)

            .Include(c => c.CourseStudents)
            .ThenInclude(scc => scc.Student)

            .Include(c => c.Enrollments)

            .FirstOrDefaultAsync(m => m.Id == id);

        
        // ------------------------------------------------------------------------ //


        // serialização usando System.Text.Json
        var options = new JsonSerializerOptions
        {
            // Use ReferenceHandler.Preserve para preservar referências circulares
            ReferenceHandler = ReferenceHandler.Preserve,

            // Indent o JSON para melhor legibilidade
            WriteIndented = true,

            // Defina um valor adequado para limitar a profundidade da serialização.
            MaxDepth = 10
        };


        // converte em json para enviar aos pedidos da API
        var jsonMariconsoft =
            JsonSerializer.Serialize(course, options);

        return Ok(jsonMariconsoft);


        // ------------------------------------------------------------------------ //

        // serialização usando Newtonsoft.Json 
        //var settings = new JsonSerializerSettings
        //{
        //    // Use PreserveReferencesHandling para preservar referências circulares
        //    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            
        //    // Indent the JSON for readability
        //    Formatting = Formatting.Indented,

        //    // Defina um valor adequado para limitar a profundidade da serialização.
        //    MaxDepth = 10,
        //};

        // converte em json para enviar aos pedidos da API
        //var json = JsonConvert.SerializeObject(
        //    course, settings);

        //return Ok(json);


        // return Ok(course);
    }


    // ---------------------------------------------------------------------- //
    // Default values
    // ---------------------------------------------------------------------- //


    // GET: api/<CoursesController>
    // [HttpGet]
    // public IEnumerable<string> Get() => new string[] {"value1", "value2"};


    // GET api/<CoursesController>/5
    // [HttpGet("{id}")]
    // public string Get(int id) => "value";


    // POST api/<CoursesController>
    // [HttpPost]
    // public void Post([FromBody] string value)
    // {
    // }


    // PUT api/<CoursesController>/5
    // [HttpPut("{id:int}")]
    // public void Put(int id, [FromBody] string value)
    // {
    // }


    // DELETE api/<CoursesController>/5
    // [HttpDelete("{id:int}")]
    // public void Delete(int id)
    // {
    // }
}
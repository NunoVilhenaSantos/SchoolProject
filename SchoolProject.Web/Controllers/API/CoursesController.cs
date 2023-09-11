using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SchoolProject.Web.Data.DataContexts.MySQL;

using SchoolProject.Web.Data.Repositories.SchoolClasses;
using SchoolProject.Web.Helpers.Users;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Linq;


// For more information on enabling Web API for empty projects,
// visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace SchoolProject.Web.Controllers.API;

/// <summary>
/// School Classes API Controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
// [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [Authorize(Policy = "Bearer")]
// [Authorize(Roles = "Admin")]
public class CoursesController : ControllerBase
{
    private readonly ISchoolClassRepository _schoolClassRepository;
    private readonly DataContextMySql _context;
    private readonly IUserHelper _userHelper;


    //private readonly IWebHostEnvironment _hostingEnvironment;


    /// <summary>
    /// School Classes API Controller constructor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userHelper"></param>
    /// <param name="schoolClassRepository"></param>
    public CoursesController(
        DataContextMySql context,
        IUserHelper userHelper,
        ISchoolClassRepository schoolClassRepository)
    {
        _context = context;
        _userHelper = userHelper;
        _schoolClassRepository = schoolClassRepository;
    }


    // GET: api/<SchoolClassesController>
    /// <summary>
    /// Get all school classes
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    // [Route("api/Courses/")]
    public IActionResult Get()
    {
        var schoolClasses = _context.Courses
            
            // School class courses include
            //.Include(c => c.CourseDisciplines)
            //.ThenInclude(scc => scc.Discipline)

            // school class students include
            //.Include(c => c.SchoolClassStudents)
             //.ThenInclude(scs => scs.Student)
             //.ThenInclude(s => s.Country)
             //.ThenInclude(s => s.Nationality)

             // include students 
             .Include(c => c.Students)
            // .ThenInclude(s => s.Country)
            // .ThenInclude(s => s.Nationality)
            // .Include(c => c.Students)
            // .ThenInclude(s => s.User)

            // include disciplines
            .Include(c => c.Disciplines)

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

        // Substitua a serialização usando Newtonsoft.Json pela serialização usando System.Text.Json
        var options = new JsonSerializerOptions
        {
            // Use ReferenceHandler.Preserve para preservar referências circulares
            ReferenceHandler = ReferenceHandler.Preserve,

            // Indent o JSON para melhor legibilidade
            WriteIndented = true
        };


        // converte em json para enviar aos pedidos da API
        var jsonMariconsoft = System.Text.Json.JsonSerializer.Serialize(schoolClasses, options);

        return Ok(jsonMariconsoft);


        // --------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------- //



        // Configura as configurações para a serialização
        var settings = new JsonSerializerSettings
        {
            // Use PreserveReferencesHandling para preservar referências circulares
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,

            // Indent the JSON for readability
            Formatting = Formatting.Indented
        };

        // converte em json para enviar aos pedidos da API
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(schoolClasses, settings);

        // return Ok(json);


        // --------------------------------------------------------------------------- //
        // --------------------------------------------------------------------------- //

        // https://stackoverflow.com/questions/58113329/asp-net-core-3-0-system-text-json-jsonexception-a-possible-object-cycle-was
        // este processo de serializar e deserializar é necessário para remover as referências circulares
        // mas consome muita memoria até que o sistema rebenta
        // consome 4,7 gb de memoria    
        // Armazene a lista na sessão para uso futuro
        var json1 = JsonConvert.SerializeObject(schoolClasses,
            new JsonSerializerSettings
            {
                // A possible object cycle was detected.
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // Use PreserveReferencesHandling para preservar referências circulares
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,

                // Indent the JSON for readability
                Formatting = Formatting.Indented
            });


        return Ok(json1);

    }


    // GET: api/<SchoolClassesController>
    /// <summary>
    /// Get all school classes with users
    /// </summary>
    /// <returns></returns>
    // [HttpGet]
    // public IActionResult Get() => Ok(_schoolClassRepository.GetAllWithUsers());

    // GET api/<SchoolClassesController>/5
    [Route("api/Courses/{id:int}")]
    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var schoolClass = _context.Courses
            .FirstOrDefaultAsync(m => m.Id == id);


        // converte em json para enviar aos pedidos da API
        var json = JsonConvert.SerializeObject(schoolClass,
            new JsonSerializerSettings
            {
                // A possible object cycle was detected.
                //
                // This can either be due to a cycle or
                // if the object depth is larger than the maximum allowed depth of 32.
                //
                // Consider using ReferenceHandler.Preserve
                // on JsonSerializerOptions to support cycles.
                //
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // Indent the JSON for readability
                Formatting = Formatting.Indented
            });



        return Ok(schoolClass);
    }


    // ---------------------------------------------------------------------- //
    // Default values
    // ---------------------------------------------------------------------- //


    // GET: api/<SchoolClassesController>
    // [HttpGet]
    // public IEnumerable<string> Get() => new string[] {"value1", "value2"};


    // GET api/<SchoolClassesController>/5
    // [HttpGet("{id}")]
    // public string Get(int id) => "value";


    // POST api/<SchoolClassesController>
    // [HttpPost]
    // public void Post([FromBody] string value)
    // {
    // }


    // PUT api/<SchoolClassesController>/5
    // [HttpPut("{id:int}")]
    // public void Put(int id, [FromBody] string value)
    // {
    // }


    // DELETE api/<SchoolClassesController>/5
    // [HttpDelete("{id:int}")]
    // public void Delete(int id)
    // {
    // }
}
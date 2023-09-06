using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace SchoolProject.Web.Helpers;

/// <summary>
///     PaginationViewHelper class to help in the indexcards views using pagination view model.
/// </summary>
/// <typeparam name="T"> T will assume each class</typeparam>
public class PaginationViewHelper<T> where T : class
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebHostEnvironment _hostingEnvironment;

    /// <summary>
    ///
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    /// <param name="httpContextAccessor"></param>
    public PaginationViewHelper(IWebHostEnvironment hostingEnvironment,
        IHttpContextAccessor httpContextAccessor)
    {
        _hostingEnvironment = hostingEnvironment;
        _httpContextAccessor = httpContextAccessor;
    }


    /// <summary>
    ///     generate sort properties to be display in the view,
    ///     using the class sent
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetSortProperties()
    {
        // Check if sortProperty is valid
        var modelType = typeof(T);

        var publicProperties = modelType.GetProperties(
            BindingFlags.Public |
            BindingFlags.Instance);

        var sortProperties =
            publicProperties.Select(prop => new SelectListItem
            {
                Value = prop.Name,
                Text = prop.Name
            }).ToList();

        return sortProperties;
    }


    private List<T> ApplySorting(
        IQueryable<T> query, string sortOrder,
        List<SelectListItem> sortOrderList, string sortProperty)
    {
        // Check if sortOrder is valid
        if (!sortOrderList.Select(item => item.Value).Contains(sortOrder))
            sortOrder = "asc";

        // Check if sortProperty is valid
        var modelType = typeof(T);

        // Check if sortProperty exists in the class
        var propertyInfo =
            modelType.GetProperty(sortProperty,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance) ??
            modelType.GetProperty("FirstName",
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);


        if (propertyInfo == null) return new List<T>();


        var parameter = Expression.Parameter(modelType, "x");
        var property = Expression.Property(parameter, propertyInfo);
        var lambda = Expression.Lambda(property, parameter);


        var orderByMethod =
            sortOrder == "asc" ? "OrderBy" : "OrderByDescending";


        var orderByExpression = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new[] {modelType, propertyInfo.PropertyType},
            query.Expression,
            lambda
        );

        // return query.Provider.CreateQuery<T>(orderByExpression);
        var recordsQuerySorted =
            query.Provider.CreateQuery<T>(orderByExpression);

        return recordsQuerySorted.ToList();

        // Obter uma página específica de um determinado tamanho
        // RecordsQuery = recordsQuerySorted
        //     .Skip((PageNumber - 1) * PageSize)
        //     .Take(PageSize)
        //     .ToList();
        //
        // return RecordsQuery;
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="sessionVarName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<T> SessionData(string sessionVarName, List<T> queryRecords)
    {
        // Access HttpContext through IHttpContextAccessor
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            // Handle the case where HttpContext is null
            return new List<T>();
        }

        // Check if sortProperty is valid
        var modelType = typeof(T);

        // Obtém todos os registos
        List<T> recordsQuery;

        // Tente obter a lista de professores da sessão
        if (httpContext.Session.TryGetValue(sessionVarName, out var allData))
        {
            // Se a lista estiver na sessão, desserializa-a
            var json = Encoding.UTF8.GetString(allData);

            recordsQuery =
                JsonConvert.DeserializeObject<List<T>>(json) ??
                new List<T>();
        }
        else
        {
            // Caso contrário, obtenha a lista completa do banco de dados
            // Chame a função GetTeachersList com o tipo T
            recordsQuery = queryRecords;

            var json = StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            httpContext.Session.Set(sessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    /// <summary>
    /// Get session variables to be used in the view, to avoid stressing the db
    /// </summary>
    /// <param name="sessionVarName"></param>
    /// <returns></returns>
    public List<T> GetSessionData(string sessionVarName)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // Check if httpContext is null
        if (httpContext == null) return new List<T>();

        // Check if session data exists
        if (!httpContext.Session.TryGetValue(
                sessionVarName, out var allData))
            return new List<T>();

        var json = Encoding.UTF8.GetString(allData);

        var recordsQuery = JsonConvert.DeserializeObject<List<T>>(json);

        return recordsQuery ?? new List<T>();
    }


    /// <summary>
    /// Set session variables to be used in the view, to avoid stressing the db
    /// </summary>
    /// <param name="sessionVarName"></param>
    /// <param name="data"></param>
    public void SetSessionData(string sessionVarName, List<T> data)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // Handle the case where HttpContext is null
        if (httpContext == null) return;

        // Serialize the data to JSON
        var json = StoreListToFileInJson(data);

        // Store the serialized data in the session
        httpContext.Session.Set(sessionVarName, Encoding.UTF8.GetBytes(json));
    }


    /// <summary>
    /// Stores the json list in a file
    /// </summary>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public string StoreListToFileInJson(IEnumerable<T> enumerable)
    {
        // Armazene a lista na sessão para uso futuro
        var json = JsonConvert.SerializeObject(enumerable,
            new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                // Indent the JSON for readability
                Formatting = Formatting.Indented
            });

        try
        {
            // Obtenha o nome da classe T
            var typeName = typeof(T).Name + "s";

            // Specify the file path where you queremos salvar o JSON file
            var filePath =
                Path.Combine(_hostingEnvironment.ContentRootPath,
                    "Data", typeName, ".json");


            // Save the JSON to the file
            File.WriteAllText(filePath, json);

            Console.WriteLine("JSON file saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return json;
    }
}
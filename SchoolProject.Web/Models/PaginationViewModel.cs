using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SchoolProject.Web.Models.Users;

namespace SchoolProject.Web.Models;

/// <summary>
///     PaginationViewModel class for all view models.
/// </summary>
/// <typeparam name="T"> T will assume each class</typeparam>
public class PaginationViewModel<T> where T : class
{
    // host environment
    private static IWebHostEnvironment _hostingEnvironment;

    // http context accessor
    private static IHttpContextAccessor _httpContextAccessor;


    // Lista de propriedades que são classes e devem ser tratadas como "Code"
    private readonly List<string> _codeProperties = new()
    {
        "Discipline", "Discipline"
    };


    // Lista de propriedades que são classes e devem ser tratadas como "FirstName"
    private readonly List<string> _firstNameProperties = new()
    {
        "Employee", "Customer", "AppUser", "CreatedBy", "UpdatedBy", "Author",
        "Student", "Teacher"
    };


    // Lista de propriedades que são classes e devem ser tratadas como "Name"
    private readonly List<string> _nameProperties = new()
    {
        "Country", "Nationality", "City", "Gender", "CountryOfNationality",
        "Category", "Discipline", "Discipline"
    };


    // Lista de propriedades que são classes e devem ser tratadas como "Title"
    private readonly List<string> _titleProperties = new()
    {
        "Book", "BookEdition", "BookStock"
    };


    // Define a list of valid page sizes
    private readonly List<int> validPageSizes = new() {10, 25, 50, 100};


    /// <summary>
    ///     PaginationViewModel constructor.
    /// </summary>
    /// <param name="records">List&lt;T&gt; where T is a class</param>
    /// <param name="pageNumber">integer</param>
    /// <param name="pageSize">integer</param>
    /// <param name="totalCount">integer</param>
    /// <param name="sortOrder">string for the order</param>
    /// <param name="sortProperty">string for the order by using the public properties of each class</param>
    public PaginationViewModel(
        List<T> records, int pageNumber, int pageSize,
        int totalCount, string sortOrder, string sortProperty)
    {
        Records = records;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;


        // Validar parâmetros de página e tamanho da página
        if (pageNumber < 1) pageNumber = 1; // Página mínima é 1


        // Tamanho da página mínimo é 10
        if (!validPageSizes.Contains(pageSize))
            // If the provided pageSize is not in the list of valid page sizes,
            // set it to the minimum valid size, which is 10.
            pageSize = validPageSizes.Min();


        // Check if sortOrder is valid, default to "asc" if not
        SortOrder =
            SortOrderList.Select(item => item.Value).Contains(sortOrder)
                ? sortOrder
                : "asc";


        SortProperty = sortProperty;
        SortPropertiesList = GetSortProperties();


        // Obtenha o nome da classe T
        var typeName = typeof(T).Name;


        IQueryable<T> recordsQuerySorted;
        if (typeName == nameof(UserWithRolesViewModel))
            // Se a classe for UserWithRolesViewModel,
            // aplique a ordenação específica
            //recordsQuerySorted = ApplySortingUserWithRoles(
            //    records.AsQueryable(), sortOrder, sortProperty);
            // TODO: Fix this error....
            recordsQuerySorted = ApplySortingUserWithRoles1(
                records.AsQueryable(), sortOrder, sortProperty);
        else
            // Caso contrário, aplique a ordenação padrão
            recordsQuerySorted = ApplySorting(
                records.AsQueryable(), sortOrder, sortProperty);

        // Obter uma página específica de um determinado tamanho
        RecordsQuery = recordsQuerySorted
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }


    // -------------------------------------------------------------- //
    // properties for the pagination
    // -------------------------------------------------------------- //


    /// <summary>
    ///     Records for the class T.
    /// </summary>
    public List<T> Records { get; set; }

    /// <summary>
    ///     RecordsQuery for the class T.
    /// </summary>
    public List<T> RecordsQuery { get; set; }


    /// <summary>
    ///     PageNumber property.
    /// </summary>
    public int PageNumber { get; set; }


    /// <summary>
    ///     PageSize property.
    /// </summary>
    public int PageSize { get; set; }


    /// <summary>
    ///     SortOrder select list item.
    /// </summary>
    public List<SelectListItem> PageSizeList => new()
    {
        new SelectListItem {Value = "10", Text = "10"},
        new SelectListItem {Value = "25", Text = "25"},
        new SelectListItem {Value = "50", Text = "50"},
        new SelectListItem {Value = "100", Text = "100"}
    };


    /// <summary>
    ///     TotalCount property.
    /// </summary>
    public int TotalCount { get; set; }


    /// <summary>
    ///     SortOrder property.
    /// </summary>
    public string SortOrder { get; set; }


    /// <summary>
    ///     SortOrder select list item.
    /// </summary>
    public List<SelectListItem> SortOrderList => new()
    {
        new SelectListItem {Value = "asc", Text = "Ascending"},
        new SelectListItem {Value = "desc", Text = "Descending"}
    };


    /// <summary>
    ///     Sort Property for the class.
    /// </summary>
    public string SortProperty { get; set; }


    /// <summary>
    ///     SortProperties property.
    /// </summary>
    public List<SelectListItem> SortPropertiesList { get; set; }


    /// <summary>
    ///     TotalPages property.
    /// </summary>
    public int TotalPages => (int) Math.Ceiling((double) TotalCount / PageSize);


    /// <summary>
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    public static void Initialize(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }


    /// <summary>
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    /// <param name="httpContextAccessor"></param>
    public static void Initialize(
        IWebHostEnvironment hostingEnvironment,
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
        var modelType = typeof(T);

        var publicProperties = modelType.GetProperties(
            BindingFlags.Public |
            BindingFlags.Instance);

        var sortProperties =
            publicProperties.Select(prop => new SelectListItem
            {
                Value = prop.Name, Text = prop.Name
            }).ToList();

        return sortProperties;
    }


    /// <summary>
    ///     Get or set data in the session.
    /// </summary>
    /// <typeparam name="T">The type of data to store in the session.</typeparam>
    /// <param name="sessionVarName">The name of the session variable.</param>
    /// <param name="queryRecords">A list of records to initialize from if the session data is not found.</param>
    /// <returns>The list of records from the session or the provided queryRecords.</returns>
    public List<T> SessionData(string sessionVarName, List<T> queryRecords)
    {
        var modelType = typeof(T);

        // Access HttpContext through IHttpContextAccessor
        var httpContext = _httpContextAccessor.HttpContext;

        // Handle the case where HttpContext is null
        if (httpContext == null) return new List<T>();

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

            Initialize(_hostingEnvironment, _httpContextAccessor);

            var json = StoreListToFileInJson(recordsQuery);

            // Armazene a lista na sessão para uso futuro
            httpContext.Session.Set(sessionVarName,
                Encoding.UTF8.GetBytes(json));
        }

        return recordsQuery;
    }


    /// <summary>
    ///     Stores the json list in a file
    /// </summary>
    /// <param name="enumerable"></param>
    /// <returns></returns>
    public static string StoreListToFileInJson(IEnumerable<T> enumerable)
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
            // Specify the directory path
            var directoryPath =
                Path.Combine(_hostingEnvironment.ContentRootPath, "Data",
                    "JsonSessionData");

            // Check if the directory exists, and create it if it doesn't
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);


            // Obtenha o nome da classe T
            var typeName = typeof(T).Name;

            // Specify the file path where you queremos salvar o JSON file
            var filePath =
                Path.Combine(directoryPath, $"{typeName}.json");


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


    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="sessionVarName"></param>
    public static void StoreListToFileInJson1(
        IEnumerable<T> enumerable, string sessionVarName)
    {
        try
        {
            // Specify the directory path
            var directoryPath = Path.Combine(
                _hostingEnvironment.ContentRootPath, "Data", "Json");

            // Check if the directory exists, and create it if it doesn't
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            // Obtain the name of the class T
            var typeName = typeof(T).Name;

            // Specify the file path where you want to save the JSON file
            var filePath = Path.Combine(directoryPath, $"{typeName}-list.json");


            // Create a StreamWriter to write to the file
            using (var streamWriter = new StreamWriter(filePath))
            using (JsonWriter jsonWriter = new JsonTextWriter(streamWriter))
            {
                // Create a JsonSerializer with settings
                // var jsonSerializer = new JsonSerializer
                // {
                //     ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //     Formatting = Formatting.Indented
                // };

                // Create a JsonSerializer with settings
                var jsonSerializer = new JsonSerializer
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.Indented,

                    // Defina o nível máximo de profundidade desejado
                    MaxDepth = 1
                };

                // Serialize and write each item to the file
                jsonWriter.WriteStartArray();
                foreach (var item in enumerable)
                {
                    jsonSerializer.Serialize(jsonWriter, item);
                    break;
                }

                jsonWriter.WriteEndArray();
            }

            Console.WriteLine("JSON file saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    private IQueryable<T> ApplySorting(
        IQueryable<T> query, string sortOrder, string sortProperty)
    {
        if (string.IsNullOrEmpty(sortProperty)) return query;

        var parameter = Expression.Parameter(typeof(T), "x");
        var propertyNames = sortProperty.Split('.');


        Expression propertyAccess = parameter;
        foreach (var propertyName in propertyNames)
        {
            var propertyInfo =
                propertyAccess.Type.GetProperty(propertyName) != null
                    ? propertyAccess.Type.GetProperty(propertyName)
                    : throw new ArgumentException(
                        $"Property {propertyName} " +
                        $"not found in type {propertyAccess.Type.Name}");

            if (propertyInfo.PropertyType.IsClass &&
                propertyInfo.PropertyType != typeof(string))
            {
                // Trate propriedades que são classes aninhadas aqui
                if (_nameProperties.Any(
                        prop =>
                            prop.Equals(propertyName,
                                StringComparison.OrdinalIgnoreCase)))
                {
                    propertyAccess =
                        Expression.Property(propertyAccess, propertyInfo);
                    propertyAccess =
                        Expression.Property(propertyAccess, "Name");
                }
                else if (_firstNameProperties.Any(
                             prop =>
                                 prop.Equals(propertyName,
                                     StringComparison.OrdinalIgnoreCase)))
                {
                    propertyAccess =
                        Expression.Property(propertyAccess, propertyInfo);
                    propertyAccess =
                        Expression.Property(propertyAccess, "FirstName");
                    //propertyAccess =
                    //    Expression.Property(propertyAccess, "FullName");
                }
                else if (_codeProperties.Any(
                             prop =>
                                 prop.Equals(propertyName,
                                     StringComparison.OrdinalIgnoreCase)))
                {
                    propertyAccess =
                        Expression.Property(propertyAccess, propertyInfo);
                    propertyAccess =
                        Expression.Property(propertyAccess, "Code");
                }
            }
            else
            {
                propertyAccess =
                    Expression.Property(propertyAccess, propertyInfo);
            }


            // Check if the property is nullable (e.g., DateTime?)
            if (propertyInfo.PropertyType.IsGenericType &&
                propertyInfo.PropertyType.GetGenericTypeDefinition() ==
                typeof(Nullable<>))
                // Handle null values by using the GetValueOrDefault method
                propertyAccess = Expression.Call(propertyAccess,
                    "GetValueOrDefault", null);
        }

        var orderByExp = Expression.Lambda(propertyAccess, parameter);

        // var orderByMethod =
        //     sortOrder == "asc" ? "OrderBy" : "OrderByDescending";

        var orderByMethod =
            sortOrder == "desc" ? "OrderByDescending" : "OrderBy";


        var orderByCall = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new[] {typeof(T), propertyAccess.Type},
            query.Expression,
            Expression.Quote(orderByExp)
        );

        return query.Provider.CreateQuery<T>(orderByCall);
    }


    private IQueryable<T> ApplySortingUserWithRoles(
        IQueryable<T> query, string sortOrder, string sortProperty)
    {
        if (string.IsNullOrEmpty(sortProperty)) return query;

        var parameter = Expression.Parameter(typeof(T), "x");


        var propertyInfo =
            typeof(T).GetProperty(sortProperty) ??
            typeof(T).GetNestedType("AppUser")?.GetProperty(sortProperty);


        //throw new ArgumentException(
        //        $"Property {sortProperty} " +
        //        $"not found in type UserWithRolesViewModels");

        Expression propertyAccess =
            Expression.Property(parameter, propertyInfo);

        var orderByExp =
            Expression.Lambda<Func<T, object>>(
                Expression.Convert(propertyAccess, typeof(object)), parameter);

        var orderByMethod =
            sortOrder == "asc" ? "OrderBy" : "OrderBy";

        var orderByCall = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new[] {typeof(T), propertyInfo.PropertyType},
            query.Expression,
            Expression.Quote(orderByExp)
        );

        return query.Provider.CreateQuery<T>(orderByCall);
    }


    private IQueryable<T> ApplySortingUserWithRoles1(
        IQueryable<T> query, string sortOrder, string sortProperty)
    {
        if (string.IsNullOrEmpty(sortProperty)) return query;

        var parameter = Expression.Parameter(typeof(T), "x");

        // Modify the propertyInfo retrieval to navigate into the "AppUser" property
        var userPropertyInfo = typeof(T).GetProperty("AppUser");
        var propertyInfo =
            userPropertyInfo.PropertyType.GetProperty(sortProperty);

        if (propertyInfo == null)
            throw new ArgumentException(
                $"Property {sortProperty} not found in type {typeof(T).FullName}");

        Expression propertyAccess = Expression.Property(
            Expression.Property(parameter, userPropertyInfo), propertyInfo);

        var orderByExp = Expression.Lambda<Func<T, object>>(
            Expression.Convert(propertyAccess, typeof(object)), parameter);

        var orderByMethod =
            sortOrder == "asc" ? "OrderBy" : "OrderBy";

        var orderByCall = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new[] {typeof(T), propertyInfo.PropertyType},
            query.Expression,
            Expression.Quote(orderByExp)
        );

        return query.Provider.CreateQuery<T>(orderByCall);
    }
}
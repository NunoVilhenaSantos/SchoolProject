using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace SchoolProject.Web.Models;

/// <summary>
///     PaginationViewModel class for all view models.
/// </summary>
/// <typeparam name="T"> T will assume each class</typeparam>
public class PaginationViewModel<T> where T : class
{
    private static IWebHostEnvironment _hostingEnvironment;

    /// <summary>
    ///     PaginationViewModel constructor.
    /// </summary>
    /// <param name="records">List&lt;T&gt; where T is a class</param>
    /// <param name="pageNumber">integer</param>
    /// <param name="pageSize">integer</param>
    /// <param name="totalCount">integer</param>
    /// <param name="sortOrder">string for the order</param>
    /// <param name="sortProperty">string for the order by using the public properties of each class</param>
    public PaginationViewModel(List<T> records, int pageNumber, int pageSize,
        int totalCount, string sortOrder, string sortProperty)
    {
        Records = records;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;

        // Check if sortOrder is valid, default to "asc" if not
        SortOrder = SortOrderList.Select(item => item.Value).Contains(sortOrder)
            ? sortOrder
            : "asc";

        SortProperty = sortProperty;
        SortPropertiesList = GetSortProperties();

        var recordsQuerySorted = ApplySorting(
            records.AsQueryable(), sortOrder, sortProperty);

        // Obter uma página específica de um determinado tamanho
        RecordsQuery = recordsQuerySorted
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }


    /// <summary>
    ///
    /// </summary>
    /// <param name="hostingEnvironment"></param>
    public static void Initialize(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }


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
    public List<SelectListItem> PageSizeList { get; } = new()
    {
        new() {Value = "10", Text = "10"},
        new() {Value = "25", Text = "25"},
        new() {Value = "50", Text = "50"},
        new() {Value = "100", Text = "100"}
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
    public List<SelectListItem> SortOrderList { get; } = new()
    {
        new() {Value = "asc", Text = "Ascending"},
        new() {Value = "desc", Text = "Descending"}
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
                Value = prop.Name,
                Text = prop.Name
            }).ToList();

        return sortProperties;
    }


    private List<T> ApplySorting(
        IQueryable<T> query, string sortOrder, string sortProperty)
    {
        // Check if sortOrder is valid
        if (!SortOrderList.Select(item => item.Value).Contains(sortOrder))
            SortOrder = "asc";


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
        RecordsQuery = recordsQuerySorted
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        return RecordsQuery;
    }


    /// <summary>
    /// Stores the json list in a file
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
            // Obtenha o nome da classe T
            var typeName = typeof(T).Name + "s";

            // Specify the file path where you queremos salvar o JSON file
            var filePath =
                Path.Combine(_hostingEnvironment.ContentRootPath,
                    "Data", typeName, ".json");


            // Save the JSON to the file
            System.IO.File.WriteAllText(filePath, json);

            Console.WriteLine("JSON file saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        Console.WriteLine("JSON file saved successfully!");

        return json;
    }
}
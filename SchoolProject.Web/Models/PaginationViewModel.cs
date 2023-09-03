using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolProject.Web.Models;

/// <summary>
/// PaginationViewModel class for all view models.
/// </summary>
/// <typeparam name="T"> T will assume each class</typeparam>
public class PaginationViewModel<T> where T : class
{
    /// <summary>
    /// PaginationViewModel constructor.
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
        RecordsQuery = ApplySorting(Records.AsQueryable());
    }


    /// <summary>
    /// Records for the class T.
    /// </summary>
    public List<T> Records { get; set; }

    /// <summary>
    /// RecordsQuery for the class T.
    /// </summary>
    public IQueryable<T> RecordsQuery { get; set; }


    /// <summary>
    /// PageNumber property.
    /// </summary>
    public int PageNumber { get; set; }


    /// <summary>
    /// PageSize property.
    /// </summary>
    public int PageSize { get; set; }


    /// <summary>
    /// SortOrder select list item.
    /// </summary>
    public List<SelectListItem> PageSizeList { get; } = new()
    {
        new SelectListItem {Value = "10", Text = "10"},
        new SelectListItem {Value = "25", Text = "25"},
        new SelectListItem {Value = "50", Text = "50"},
        new SelectListItem {Value = "100", Text = "100"},
    };


    /// <summary>
    /// TotalCount property.
    /// </summary>
    public int TotalCount { get; set; }


    /// <summary>
    /// SortOrder property.
    /// </summary>
    public string SortOrder { get; set; }


    /// <summary>
    /// SortOrder select list item.
    /// </summary>
    public List<SelectListItem> SortOrderList { get; } = new()
    {
        new SelectListItem {Value = "asc", Text = "Ascending"},
        new SelectListItem {Value = "desc", Text = "Descending"},
    };


    /// <summary>
    /// Sort Property for the class.
    /// </summary>
    public string SortProperty { get; set; }


    /// <summary>
    /// SortProperties property.
    /// </summary>
    public List<SelectListItem> SortPropertiesList { get; set; }


    /// <summary>
    /// TotalPages property.
    /// </summary>
    public int TotalPages => (int) Math.Ceiling((double) TotalCount / PageSize);


    /// <summary>
    /// generate sort properties to be display in the view,
    /// using the class sent
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetSortProperties()
    {
        Type modelType = typeof(T);

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


    private IQueryable<T> ApplySorting(IQueryable<T> query)
    {
        // Check if sortOrder is valid
        if (!SortOrderList.Select(item => item.Value).Contains(SortOrder))
            SortOrder = "asc";


        // Check if sortProperty is valid
        Type modelType = typeof(T);


        // Check if sortProperty exists in the class
        var propertyInfo =
            modelType.GetProperty(SortProperty,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance) ??
            modelType.GetProperty("FirstName",
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);


        if (propertyInfo == null) return new List<T>().AsQueryable();


        var parameter = Expression.Parameter(modelType, "x");
        var property = Expression.Property(parameter, propertyInfo);
        var lambda = Expression.Lambda(property, parameter);


        var orderByMethod =
            SortOrder == "asc" ? "OrderBy" : "OrderByDescending";


        var orderByExpression = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new[] {modelType, propertyInfo.PropertyType},
            query.Expression,
            lambda
        );

        return query.Provider.CreateQuery<T>(orderByExpression);
    }
}
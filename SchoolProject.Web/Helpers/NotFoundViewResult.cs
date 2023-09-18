using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SchoolProject.Web.Data.Entities.Countries;

namespace SchoolProject.Web.Helpers;

/// <summary>
/// </summary>
public class NotFoundViewResult : ViewResult
{
    // public string ClassName { get; set; }

    // public string? ItemId { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="viewName"></param>
    /// <param name="className"></param>
    /// <param name="itemId"></param>
    /// <param name="controller"></param>
    /// <param name="action"></param>
    public NotFoundViewResult(
        string viewName, string className, string? itemId, string controller,
        string action)
    {
        ViewName = viewName;
        StatusCode = (int) HttpStatusCode.NotFound;
        // ClassName = className;
        // ItemId = itemId;

        var viewData = new ViewDataDictionary(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary())
        {
            ["ViewName"] = ViewName,
            ["StatusCode"] = StatusCode,
            ["ClassName"] = className,
            ["ItemId"] = itemId,
            ["Controller"] = controller,
            ["Action"] = action
        };

        ViewData = viewData;

        // var viewResult = new ViewResult
        // {
        //     ViewData = viewData
        // };

        // Configure o modelo da view corretamente
        // return viewResult;
    }


    /// <summary>
    /// </summary>
    /// <returns></returns>
    public ViewResult Test()
    {
        var viewData = new ViewDataDictionary<Country>(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary());

        viewData = new ViewDataDictionary<Country>(viewData);

        var viewResult = new ViewResult
        {
            ViewData = viewData
        };


        return viewResult;
    }
}
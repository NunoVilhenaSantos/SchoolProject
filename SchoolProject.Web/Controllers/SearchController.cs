using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.DataContexts.MySQL;

namespace SchoolProject.Web.Controllers;

public class SearchController : Controller
{
    /// <summary>
    ///     Data repository
    /// </summary>
    // private readonly IBookEditionRepository _bookEditionRepository;
    private readonly DataContextMySql _dataContextMySql;


    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dataContextMySql"></param>
    public SearchController(DataContextMySql dataContextMySql)
    {
        _dataContextMySql = dataContextMySql;
    }

    internal static string ControllerName =>
        HomeController.SplitCamelCase(nameof(SearchController));


    /// <summary>
    ///     Gets the current controller name without the "Controller" suffix.
    /// </summary>
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


    // GET: SearchController
    // public ActionResult CarouselEditions()
    // {
    //     // var model = _bookEditionRepository.GetCarouselEditions();
    //     // return View(model);
    //     return View();
    // }


    // public ActionResult CarouselScroll(string category, int id)
    // {
    //     // var model = _bookEditionRepository
    //     //     .CarouselEditionsInfiniteScroll(category, id);
    //     //
    //     // return PartialView("_Carousel", model);
    //     return View();
    // }


    // -------------------------------------------------------------- //

    private void AddModelError(string errorMessage)
    {
        ModelState.AddModelError(string.Empty, errorMessage);
    }

    // -------------------------------------------------------------- //
}
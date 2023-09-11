using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Data.Repositories;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.Users;


namespace SchoolProject.Web.Controllers.API;

/// <summary>
///
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
// [Authorize(Policy = "Bearer")]
// [Authorize(Roles = "Admin")]
public class ProductsController : Controller
{
    //private readonly IProductsRepository _productsRepository;
    private readonly IUserHelper _userHelper;


    /// <summary>
    ///
    /// </summary>
    /// <param name="userHelper"></param>
    public ProductsController(
        IUserHelper userHelper
        //IProductsRepository productsRepository
    )
    {
        _userHelper = userHelper;
        //_productsRepository = productsRepository;
    }


    // GET: api/Products
    [HttpGet]
    public IActionResult GetProducts()
    {
        // var products = _productsRepository.GetAll();

        // foreach (var p in products)
        // {
        //     p.User = _userHelper.GetUserByIdAsync(p.User.Id).Result;
        // }

        // return Ok(products);

        // return Ok(_productsRepository.GetAllWithUsers());

        return Ok(Empty);
    }

    // GET: api/Products/5
    [HttpGet("id", Name = "Get")]
    public string GetProduct(int id)
    {
        return "value";
    }

    // POST: api/Products
    [HttpPost]
    public void PostProduct([FromBody] string value, int id)
    {
    }

    // PUT: api/Products/5
    [HttpPut("id")]
    public void PutProduct(int id, [FromBody] string value)
    {
    }

    // DELETE: api/Products/5
    [HttpDelete("id")]
    public void DeleteProduct(int id)
    {
    }
}
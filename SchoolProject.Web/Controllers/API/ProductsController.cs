using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Controllers.API;

/// <summary>
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
    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetProducts()
    {
        // var products = _productsRepository.GetAll();

        // foreach (var p in products)
        // {
        //     p.AppUser = _userHelper.GetUserByIdAsync(p.AppUser.Id).Result;
        // }

        // return Ok(products);

        // return Ok(_productsRepository.GetAllWithUsers());

        return Ok(Empty);
    }


    // GET: api/Products/5
    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("id", Name = "Get")]
    public string GetProduct(int id)
    {
        return "value";
    }


    // POST: api/Products
    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="id"></param>
    [HttpPost]
    public void PostProduct([FromBody] string value, int id)
    {
    }


    // PUT: api/Products/5
    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    [HttpPut("id")]
    public void PutProduct(int id, [FromBody] string value)
    {
    }


    // DELETE: api/Products/5
    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("id")]
    public void DeleteProduct(int id)
    {
    }
}
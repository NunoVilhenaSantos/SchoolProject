using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Models.UsersDataDeletion;

namespace SchoolProject.Web.Controllers.API;

[Route(template: "api/[controller]")]
[ApiController]
public class UserDataDeletionController : ControllerBase
{
    //public IActionResult Index()
    //{
    //    return View();
    //}


    [HttpPost(template: "/datadeletion")]
    public IActionResult HandleDataDeletionRequest(
        [FromBody] FacebookDataDeletionRequest request)
    {
        // Implement the data deletion process here
        // ...

        // Generate the response JSON
        var response = new FacebookDataDeletionResponse
        {
            Url = "https://www.<your_website>.com/deletion?id=abc123",
            ConfirmationCode = "abc123"
        };

        return Ok(value: response);
    }
}
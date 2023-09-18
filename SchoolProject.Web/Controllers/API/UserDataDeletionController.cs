using Microsoft.AspNetCore.Mvc;
using SchoolProject.Web.Models.UsersDataDeletion;

namespace SchoolProject.Web.Controllers.API;

/// <summary>
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserDataDeletionController : ControllerBase
{
    /// <summary>
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("/datadeletion")]
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

        return Ok(response);
    }
}
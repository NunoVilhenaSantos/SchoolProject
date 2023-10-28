namespace SchoolProject.Web.Helpers;

/// <summary>
///     AppResponse class for all view models.
/// </summary>
public class AppResponse
{
    /// <summary>
    ///     AppResponse IsSuccess to inform if the action was done or not.
    /// </summary>
    public required bool IsSuccess { get; set; }


    /// <summary>
    ///     AppResponse Message to inform the appUser about the action.
    /// </summary>
    public string? Message { get; set; }


    /// <summary>
    ///     AppResponse results to store the object data.
    /// </summary>
    public object? Results { get; set; }
    
}
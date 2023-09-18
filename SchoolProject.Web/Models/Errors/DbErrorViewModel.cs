namespace SchoolProject.Web.Models.Errors;

/// <summary>
/// </summary>
public class DbErrorViewModel
{
    /// <summary>
    /// </summary>
    public required bool DbUpdateException { get; set; }


    /// <summary>
    /// </summary>
    public required string ErrorTitle { get; set; }


    /// <summary>
    /// </summary>
    public required string ErrorMessage { get; set; }


    /// <summary>
    /// </summary>
    public required string ItemClass { get; set; }


    /// <summary>
    /// </summary>
    public required int ItemId { get; set; }


    /// <summary>
    /// </summary>
    public required Guid ItemGuid { get; set; }


    /// <summary>
    /// </summary>
    public required string ItemName { get; set; }
}
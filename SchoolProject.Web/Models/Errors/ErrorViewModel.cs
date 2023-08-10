namespace SchoolProject.Web.Models.Errors;

public class ErrorViewModel
{
    /// <summary>
    ///   Identificador da requisição.
    /// </summary>
    public string RequestId { get; set; }


    /// <summary>
    ///    Código de status HTTP.
    /// </summary>
    public int StatusCode { get; set; }


    /// <summary>
    ///  Indica se o identificador da requisição deve ser exibido.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(value: RequestId);
}
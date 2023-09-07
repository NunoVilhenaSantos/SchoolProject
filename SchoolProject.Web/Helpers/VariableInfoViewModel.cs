using System.ComponentModel;

namespace SchoolProject.Web.Helpers;

/// <summary>
/// </summary>
public class VariableInfoViewModel
{
    /// <summary>
    /// </summary>
    [DisplayName("Nome da Variável")]
    public required string VariableName { get; set; }

    /// <summary>
    ///     Nome da Classe
    /// </summary>
    [DisplayName("Nome da Classe")]
    public required string ClassName { get; set; }


    /// <summary>
    ///     Contagem de Objetos na classe
    /// </summary>
    [DisplayName("Contagem de Objetos")]
    public required int ObjectsCount { get; set; }

    /// <summary>
    ///     Tamanho em bytes
    /// </summary>
    [DisplayName("Tamanho em Bytes")]
    public required int SizeInBytes { get; set; }

    /// <summary>
    ///     Tamanho em kilobytes
    /// </summary>
    [DisplayName("Tamanho em Kilobytes")]
    public decimal SizeInKilobytes => SizeInBytes / 1024.0m;

    /// <summary>
    ///     Tamanho em megabytes
    /// </summary>
    [DisplayName("Tamanho em Megabytes")]
    // public decimal SizeInMegabytes => SizeInBytes / (1024.0m * 1024.0m);
    public decimal SizeInMegabytes => SizeInKilobytes / 1024.0m;

    /// <summary>
    ///     Tamanho em gigabytes
    /// </summary>
    [DisplayName("Tamanho em Gigabytes")]
    // public decimal SizeInGigabytes => SizeInBytes / (1024.0m * 1024.0m * 1024.0m);
    public decimal SizeInGigabytes => SizeInMegabytes / 1024.0m;
}
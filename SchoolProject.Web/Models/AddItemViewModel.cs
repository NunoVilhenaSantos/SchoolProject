using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolProject.Web.Models;

public class AddItemViewModel
{
    [Display(Name = "Product")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "You must select a product.")]
    public int ProductId { get; set; }


    [Display(Name = "Quantity")]
    [Range(minimum: 1, maximum: double.MaxValue, ErrorMessage = "You must select a quantity.")]
    public double Quantity { get; set; }


    public IEnumerable<SelectListItem> Products { get; set; }


    // [Display(Name = "Remarks")]
    // [MaxLength(250,
    //     ErrorMessage = "The field {0} only can contain {1} characters length.")]
    // public string Remarks { get; set; }
}
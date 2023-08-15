using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models;

public class AddItemViewModel
{
    [Display(Name = "Product")]
    [Range(1, int.MaxValue, ErrorMessage = "You must select a product.")]
    public int ProductId { get; set; }


    [Display(Name = "Quantity")]
    [Range(1, double.MaxValue, ErrorMessage = "You must select a quantity.")]
    public double Quantity { get; set; }


    public IEnumerable<SelectListItem> Products { get; set; }


    // [Display(Name = "Remarks")]
    // [MaxLength(250,
    //     ErrorMessage = "The field {0} only can contain {1} characters length.")]
    // public string Remarks { get; set; }
}
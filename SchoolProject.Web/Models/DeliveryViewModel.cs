using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolProject.Web.Models;

public class DeliveryViewModel
{
    [Required] [Key] public int Id { get; set; }


    [DisplayName(displayName: "Delivery Date")]
    [DataType(dataType: DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}",
        ApplyFormatInEditMode = false)]
    public DateTime? DeliveryDate { get; set; }
}
using System.ComponentModel;
using SchoolProject.Web.Data.Entities;

namespace SchoolProject.Web.Models;

public class OwnerViewModel : Owner
{
    [DisplayName("Image")] public IFormFile? ImageFile { get; set; }
}
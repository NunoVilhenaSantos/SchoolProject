using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SchoolProject.Web.Models.Roles
{
    public class IdentityRoleViewModel : IdentityRole
    {
        [Key] public string Id { get; set; }


        [Display(Name = "Role Name")] public string Name { get; set; }


        [Display(Name = "Normalized Role Name")]
        public string NormalizedName { get; set; }


        [Display(Name = "Concurrency Stamp")]
        public string ConcurrencyStamp { get; set; }
    }
}
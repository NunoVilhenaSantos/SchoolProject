using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.Entities.Genders;

namespace SchoolProject.Web.Data.Repositories.Genders;

/// <inheritdoc />
public interface IGenderRepository : IGenericRepository<Gender>
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerable<SelectListItem> GetComboGenders();
}
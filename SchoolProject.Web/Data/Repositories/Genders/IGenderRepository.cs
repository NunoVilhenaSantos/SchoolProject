using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Genders;

namespace SchoolProject.Web.Data.Repositories.Genders;

/// <inheritdoc />
public interface IGenderRepository : IGenericRepository<Gender>
{
    IEnumerable<SelectListItem> GetComboGenders();
}
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Genders;

namespace SchoolProject.Web.Data.Repositories.Genders;

/// <inheritdoc />
public class GenderRepository : GenericRepository<Gender>, IGenderRepository
{
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;

    /// <inheritdoc />
    public GenderRepository(
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    ) : base(dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }

    public IEnumerable<SelectListItem> GetComboGenders()
    {
        var list = _dataContext.Genders
            .Select(p => new SelectListItem
                {Text = p.Name, Value = p.Id.ToString(),}).ToList();

        list.Insert(0, new SelectListItem
            {Text = "(Select a Gender...)", Value = "0",});

        return list;
    }
}
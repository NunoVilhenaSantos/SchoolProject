using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Genders;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Repositories.Genders;

/// <inheritdoc cref="SchoolProject.Web.Data.Repositories.Genders.IGenderRepository" />
public class GenderRepository : GenericRepository<Gender>, IGenderRepository
{
    // authenticated user
    private readonly AuthenticatedUserInApp _authenticatedUserInApp;


    // data context
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;

    // helpers
    private readonly IStorageHelper _storageHelper;
    private readonly IUserHelper _userHelper;


    /// <inheritdoc />
    public GenderRepository(
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite,
        AuthenticatedUserInApp authenticatedUserInApp,
        IStorageHelper storageHelper, IUserHelper userHelper) : base(
        dataContext, dataContextMySql, dataContextMsSql, dataContextSqLite)
    {
        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
        _authenticatedUserInApp = authenticatedUserInApp;
        _storageHelper = storageHelper;
        _userHelper = userHelper;
    }


    /// <inheritdoc />
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
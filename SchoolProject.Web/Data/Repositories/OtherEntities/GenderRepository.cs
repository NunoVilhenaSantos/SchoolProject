using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.OtherEntities;

namespace SchoolProject.Web.Data.Repositories.OtherEntities;

/// <inheritdoc />
public class GenderRepository : GenericRepository<Gender>, IGenderRepository
{
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;

    /// <inheritdoc />
    protected GenderRepository(
        DataContextMySql dataContext, DataContextMySql dataContextMySql,
        DataContextMsSql dataContextMsSql, DataContextSqLite dataContextSqLite
    ) : base(dataContext: dataContext, dataContextMySql: dataContextMySql, dataContextMsSql: dataContextMsSql, dataContextSqLite: dataContextSqLite)
    {
        _dataContext = dataContext;
        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }
}
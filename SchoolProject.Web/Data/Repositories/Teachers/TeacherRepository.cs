using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Teachers;

namespace SchoolProject.Web.Data.Repositories.Teachers;

/// <inheritdoc />
public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    private readonly DataContextMySql _dataContext;
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;

    private readonly DataContextSqLite _dataContextSqLite;

    /// <inheritdoc />
    protected TeacherRepository(
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
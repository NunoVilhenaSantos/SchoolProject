namespace SchoolProject.Web.Data.DataContexts.MSSQL;

/// <inheritdoc />
public class DcMsSqlLocal : DataContextMsSql
{
    /// <inheritdoc />
    public DcMsSqlLocal(DbContextOptions<DcMsSqlLocal> options) :
        base(options)
    {
    }
}
namespace SchoolProject.Web.Data.DataContexts.MSSQL;

public class DcMsSqlLocal : DataContextMsSql
{
    /// <inheritdoc />
    public DcMsSqlLocal(DbContextOptions<DcMsSqlLocal> options) :
        base(options)
    {
    }
}
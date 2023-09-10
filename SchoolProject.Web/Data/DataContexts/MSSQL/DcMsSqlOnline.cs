namespace SchoolProject.Web.Data.DataContexts.MSSQL;

/// <inheritdoc />
public class DcMsSqlOnline : DataContextMsSql
{
    /// <inheritdoc />
    public DcMsSqlOnline(DbContextOptions<DcMsSqlOnline> options) :
        base(options)
    {
    }
}
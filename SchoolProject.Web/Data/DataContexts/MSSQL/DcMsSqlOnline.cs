namespace SchoolProject.Web.Data.DataContexts.MSSQL;

public class DcMsSqlOnline : DataContextMsSql
{
    /// <inheritdoc />
    public DcMsSqlOnline(DbContextOptions<DcMsSqlOnline> options) :
        base(options: options)
    {
    }
}
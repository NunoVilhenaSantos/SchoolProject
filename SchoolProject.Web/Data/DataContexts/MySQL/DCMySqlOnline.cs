namespace SchoolProject.Web.Data.DataContexts.MySQL;

/// <inheritdoc />
public class DcMySqlOnline : DataContextMySql
{
    /// <inheritdoc />
    public DcMySqlOnline(DbContextOptions<DcMySqlOnline> options) :
        base(options)
    {
    }
}
namespace SchoolProject.Web.Data.DataContexts;

public class DcMsSqlOnline:DataContextMsSql
{
    /// <inheritdoc />
    public DcMsSqlOnline(DbContextOptions<DcMsSqlOnline> options) :
        base(options)
    {
    }
}

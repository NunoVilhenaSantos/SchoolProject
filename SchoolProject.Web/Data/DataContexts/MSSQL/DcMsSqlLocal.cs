namespace SchoolProject.Web.Data.DataContexts;

public class DcMsSqlLocal : DataContextMsSql
{
    /// <inheritdoc />
    public DcMsSqlLocal(DbContextOptions<DcMsSqlLocal> options) :
        base(options)
    {
    }
}